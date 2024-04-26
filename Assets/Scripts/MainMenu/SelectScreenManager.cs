using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using TMPro;
//using UnityEngine.UIElements;

public class SelectScreenManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int numberOfPlayers = 1;
    public List<PlayerInterfaces> plInterfaces = new List<PlayerInterfaces>();
    public PotraitInfo[] potraitPrefabs;
    public int maxX;
    public int maxY;
    PotraitInfo[,] charGrid;
    public GameObject[] portraitScale;
    
    public GameObject potraitCanvas;

    bool loadLevel;
    public bool bothPlayersSelected;
    CharacterManager charManager;

    public Vector2 SelectorPos1;
    public Vector2 SelectorPos2;

    public Image selectedBackground;
    public Sprite selectedBackgroundSprite;
    public SpriteRenderer selectedBackgroundRenderer;

    public TMP_Text char1;
    public TMP_Text char2;



    #region Singleton
    public static SelectScreenManager instance;
    public static SelectScreenManager GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
    }
    #endregion
    void Start()
    {
        charManager = CharacterManager.GetInstance();
        numberOfPlayers = charManager.numberOfUsers;

        charGrid = new PotraitInfo[maxX, maxY];

        int x = 0;
        int y = 0;

        potraitPrefabs = potraitCanvas.GetComponentsInChildren<PotraitInfo>();

        for(int i = 0; i < potraitPrefabs.Length; i++)
        {
            potraitPrefabs[i].posX += x;
            potraitPrefabs[i].posY += y;

            charGrid[x,y] = potraitPrefabs[i];

            if( x < maxX - 1)
            {
                x++;
            }
            else
            {
                x = 0;
                y++;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!loadLevel)
        {
            for(int i = 0; i < plInterfaces.Count; i++)
            {
                if (i < numberOfPlayers)
                {
                    if (plInterfaces[i].playerBase.hasCharacter)
                    {
                        if (Input.GetButtonUp("Fire2" + charManager.players[i].inputId))
                        {
                            plInterfaces[i].playerBase.hasCharacter = false;
                        }
                    }
                    if (!charManager.players[i].hasCharacter)
                    {
                        plInterfaces[i].playerBase = charManager.players[i];

                        HandleSelectorPosition(plInterfaces[i],i);
                        HandleSelecScreenInput(plInterfaces[i], charManager.players[i].inputId);
                        HandleCharacterPreview(plInterfaces[i], i);
                    }
                }
                else
                {
                    charManager.players[i].hasCharacter = true;
                }
            }
        }

        if(bothPlayersSelected)
        {
            
            StartCoroutine("LoadLevel");
            loadLevel = true;
        }
        else
        {
            if (charManager.players[0].hasCharacter
                && charManager.players[1].hasCharacter)
            {
                bothPlayersSelected = true;
            }
        }
    }
    void HandleSelecScreenInput(PlayerInterfaces p1, string playerId)
    {
        #region gridNavigation
        float vertical = Input.GetAxis("Vertical" + playerId);

        if (vertical != 0)
        {
            if (!p1.hitInputOnce)
            {
                if (vertical > 0)
                {
                    p1.activeY = (p1.activeY > 0) ? p1.activeY - 1 : maxY - 1;
                }
                else
                {
                    p1.activeY = (p1.activeY < maxY - 1) ? p1.activeY + 1 : 0;
                }
                p1.hitInputOnce = true;
            }
        }

        float horizontal = Input.GetAxis("Horizontal" + playerId);

        if (horizontal != 0)
        {
            if (!p1.hitInputOnce) 
            { 
                if (horizontal > 0)
                {
                    p1.activeX = (p1.activeX < maxX - 1) ? p1.activeX + 1 : maxX - 1;
                }
                else if(horizontal <0)
                {
                    p1.activeX = (p1.activeX > 0) ? p1.activeX - 1 : 0;
                }
                p1.timerToReset = 0;
                p1.hitInputOnce = true;
            }
        } 

        //if (vertical == 0 && horizontal == 0)
        //{
        //    p1.hitInputOnce = false;
        //}

        if (p1.hitInputOnce)
        {
            p1.timerToReset += Time.deltaTime;

            if (p1.timerToReset > 0.5f)
            {
                p1.hitInputOnce = false;
                p1.timerToReset = 0;
            }
        }
        #endregion
        if (Input.GetButtonUp("Fire1" + playerId))
        {
            //p1.createdCharacter.GetComponentInChildren<Animator>().Play("Kick");

            p1.playerBase.playerPrefab =
                charManager.returnCharacterWithID(p1.activePotrait.characterId).prefab;

            p1.playerBase.hasCharacter = true;

            toggleSelectedPortrait(p1);

        }
    }

    private void toggleSelectedPortrait(PlayerInterfaces p1)
    {
        p1.activePotrait.GetComponent<Image>().sprite = selectedBackgroundRenderer.sprite;

    }
    IEnumerator LoadLevel()
    {
        //if one player is AI, assign a random character to the prefab
        for(int i = 0; i < charManager.players.Count; i++)
        {
            if (charManager.players[i].playerType == PlayerBase.PlayerType.ai)
            {
                if (charManager.players[i].playerPrefab == null)
                {
                    int ranValue = Random.Range(0, potraitPrefabs.Length);

                    charManager.players[i].playerPrefab =
                        charManager.returnCharacterWithID(potraitPrefabs[ranValue].characterId).prefab;

                    Debug.Log(potraitPrefabs[ranValue].characterId);
                }
            }
        }
        Debug.Log("loading");
        yield return new WaitForSeconds(0);

        if(charManager.siberia)
        {
            Debug.Log("loading");
            SceneManager.LoadSceneAsync("Siberia", LoadSceneMode.Single);
        }
        else if(charManager.egypt)
        {
            Debug.Log("loading");
            SceneManager.LoadSceneAsync("Egyot", LoadSceneMode.Single);
        }
        

    }
    void HandleSelectorPosition(PlayerInterfaces p1, int i)
    {
        p1.selector.SetActive(true);

        p1.activePotrait = charGrid[p1.activeX, p1.activeY]; // find active position
        
        

        //place selector over its position
        Vector2 selectorPosition = p1.activePotrait.transform.localPosition;

        selectorPosition = selectorPosition + new Vector2(potraitCanvas.transform.localPosition.x
            , potraitCanvas.transform.localPosition.y);
        //Debug.Log(selectorPosition);
        
        
        p1.selector.transform.localPosition = selectorPosition;

        if (i == 0)
        {
            SelectorPos1.x = p1.activeX;
            SelectorPos1.y = p1.activeY;
        }
        else if (i == 1)
        {
            SelectorPos2.x = p1.activeX;
            SelectorPos2.y = p1.activeY;
        }

        
        togglePortrait(p1);

    }

    void HandleCharacterPreview(PlayerInterfaces p1,int i)
    {
        if(p1.previewPotrait != p1.activePotrait)
        {
            if(p1.createdCharacter != null)
            {
                Destroy(p1.createdCharacter);
            }

            GameObject go = Instantiate(
                charManager.returnCharacterWithID(p1.activePotrait.characterId).prefab,
                p1.charVisPos.position,
                Quaternion.identity) as GameObject;

            p1.createdCharacter = go;
            p1.previewPotrait = p1.activePotrait;
            //if (string.Equals(p1.playerBase.playerId, charManager.players[0].playerId)) 
            if (i == 0)
            {
                p1.createdCharacter.GetComponent<StateManager>().lookRight = true;
            }
            else p1.createdCharacter.GetComponent<StateManager>().lookRight = false;



        }
    }
    //private void selectButton(UnityEngine.UI.Button button)
    //{
    //    EventSystem.current.SetSelectedGameObject(button.gameObject, new BaseEventData(EventSystem.current));
    //}
    private void togglePortrait(PlayerInterfaces p1)
    {
        for (int i = 0; i < portraitScale.Length; i++)
        {
            bool isSelectedByAnySelector = (portraitScale[i].GetComponent<SelectScale>().x == SelectorPos1.x && portraitScale[i].GetComponent<SelectScale>().y == SelectorPos1.y) ||
                                      (portraitScale[i].GetComponent<SelectScale>().x == SelectorPos2.x && portraitScale[i].GetComponent<SelectScale>().y == SelectorPos2.y);
            portraitScale[i].GetComponent<SelectScale>().isToggled = isSelectedByAnySelector;



            if (portraitScale[i].GetComponent<SelectScale>().x == SelectorPos1.x && portraitScale[i].GetComponent<SelectScale>().y == SelectorPos1.y)
            {
                char1.text = portraitScale[i].GetComponent<SelectScale>().charName;
            }
            else if(portraitScale[i].GetComponent<SelectScale>().x == SelectorPos2.x && portraitScale[i].GetComponent<SelectScale>().y == SelectorPos2.y)
            {
                char2.text = portraitScale[i].GetComponent<SelectScale>().charName;
                
            }

        
        }

        
    }

   

    public void OnPointerEnter(PointerEventData eventData)
    {
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    [System.Serializable]
    public class PlayerInterfaces
    {
        public PotraitInfo activePotrait; // active portrait for player1
        public PotraitInfo previewPotrait;
        public GameObject selector; // selector indicator for player 1
        public Transform charVisPos; // visualization pos for player1
        public GameObject createdCharacter; // creater char for player 1


        public int activeX;
        public int activeY;

        public bool hitInputOnce;
        public float timerToReset;

        public PlayerBase playerBase;
    }
}




