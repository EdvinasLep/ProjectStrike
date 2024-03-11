using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class IntroSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject startText;
    float timer;
    bool loadingLevel = false;
    bool init = false;

    public int activeElement;
    public GameObject menuObj;
    public ButtonRef[] menuOptions;
    void Start()
    {
        menuObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(!init)
        {
            //flickers Press Start Text
            timer += Time.deltaTime;
            if(timer>0.6f)
            {
                timer = 0;
                startText.SetActive(!startText.activeInHierarchy);
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                init = true;
                startText.SetActive(false);
                menuObj.SetActive(true); // closes text and opens menu
            }
        }
        else 
        {
            if(!loadingLevel)
            {
                menuOptions[activeElement].selected = true;
                if(Input.GetKeyUp(KeyCode.UpArrow))
                {
                    menuOptions[activeElement].selected = false;
                    if(activeElement > 0)
                    {
                        activeElement--;
                    }
                    else activeElement = menuOptions.Length - 1;

                }
                if (Input.GetKeyUp(KeyCode.DownArrow))
                {
                    menuOptions[activeElement].selected = false;
                    if (activeElement < menuOptions.Length - 1)
                    {
                        activeElement++;
                    }
                    else activeElement = 0;
                }
            }

            //selecting an option
            if(Input.GetKeyUp(KeyCode.Space))
            {
                Debug.Log("load");
                loadingLevel = true;
                StartCoroutine("LoadLevel");
                menuOptions[activeElement].transform.localScale *= 1.2f;
            }
        }
    }

    void HandleSelectedOption()
    {
        switch (activeElement)
        {
            case 0:
                CharacterManager.GetInstance().numberOfUsers = 1;
                break;
            case 1:
                CharacterManager.GetInstance().numberOfUsers = 2;
                CharacterManager.GetInstance().players[1].playerType = PlayerBase.PlayerType.user;
                break;
        }
    }

    IEnumerator LoadLevel()
    {
        HandleSelectedOption();
        yield return new WaitForSeconds(0.6f);
        SceneManager.LoadSceneAsync("select", LoadSceneMode.Single);
    }
}
