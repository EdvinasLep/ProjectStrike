using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class LevelManager : MonoBehaviour
{
    WaitForSeconds oneSec;
    public Transform[] SpawnPositions;

    CameraManager camM;
    CharacterManager charM;
    LevelUI levelUI;

    public int maxTurns = 2;
    int currentTurn = 1;

    public bool countdown;
    public int maxTurnTimer = 20;
    int currentTimer;
    float internalTimer;

    SmoothSlider smoothSlider;
  
    void Start()
    {
        charM = CharacterManager.GetInstance();
        levelUI = LevelUI.GetInstance();
        camM = CameraManager.GetInstance();

        oneSec = new WaitForSeconds(1);

        levelUI.AnnouncerTextLine1.gameObject.SetActive(false);
        levelUI.AnnouncerTextLine2.gameObject.SetActive(false);

        //levelUI.ChainCounters[0].gameObject.SetActive(false);
        //levelUI.ChainCounters[1].gameObject.SetActive(false);

        smoothSlider = GetComponent<SmoothSlider>();

        StartCoroutine("StartGame");
    }

    void FixedUpdate()
    {
        if (charM.players[0].playerStates.transform.position.x <
            charM.players[1].playerStates.transform.position.x)
        {
            charM.players[0].playerStates.lookRight = true;
            charM.players[1].playerStates.lookRight = false;
        }
        else
        {
            charM.players[0].playerStates.lookRight = false;
            charM.players[1].playerStates.lookRight = true;
        }

        

    }

    private void Update()
    {
        if(countdown)
        {
            HandleTurnTimer();
        }

        //if (charM.players[0].playerStates.gettingHit)
        //{
        //    charM.players[0].playerStates.landed = true;
        //}
        //else if (charM.players[1].playerStates.gettingHit)
        //{
        //    charM.players[0].playerStates.landed = true;
        //}
    }

    void HandleTurnTimer()
    {
        levelUI.LevelTimer.text = currentTimer.ToString();

        internalTimer += Time.deltaTime;

        if(internalTimer > 1)
        {
            currentTimer--;
            internalTimer = 0;
        }

        if(currentTimer <= 0)
        {
            EndTurnFunction(true);
            countdown = false;
        }
    }

    IEnumerator StartGame()
    {
        yield return CreatePlayers();
        yield return InitTurn();
    }

    IEnumerator InitTurn()
    {
        levelUI.AnnouncerTextLine1.gameObject.SetActive(false);
        levelUI.AnnouncerTextLine2.gameObject.SetActive(false);

        currentTimer = maxTurnTimer;
        countdown = false;

        yield return InitPlayers();
        yield return EnableControl();
    }

    IEnumerator CreatePlayers()
    {
        for(int i = 0; i < charM.players.Count; i++)
        {
            GameObject go = Instantiate(charM.players[i].playerPrefab, 
                SpawnPositions[i].position,
                Quaternion.identity) as GameObject;

            charM.players[i].playerStates = go.GetComponent<StateManager>();
            charM.players[i].playerStates.chainCount = levelUI.ChainCounters[i];
            //charM.players[i].playerStates.healthSlider = levelUI.healthSliders[i];
            //charM.players[i].playerStates.energySlider = levelUI.energySliders[i];
            //charM.players[i].playerStates.blockSlider = levelUI.blockSliders[i];

            charM.players[i].sliders = go.GetComponent<SmoothSlider>();
            charM.players[i].sliders.healthSlider = levelUI.healthSliders[i];
            charM.players[i].sliders.energySlider = levelUI.energySliders[i];
            charM.players[i].sliders.blockSlider = levelUI.blockSliders[i];
            charM.players[i].sliders.delayedHealthSlider = levelUI.delayedHealthSlider[i];
            camM.players.Add(go.transform);
        }
        yield return null;
    }

    IEnumerator InitPlayers()
    {
        for(int i = 0; i < charM.players.Count; i++)
        {
            charM.players[i].playerStates.health = 100;
            charM.players[i].playerStates.handleAnim.anim.Play("Locomotion");
            charM.players[i].playerStates.transform.position = SpawnPositions[i].position; // reset player position, if not in next round spawns in last position
        }
        yield return null;
    }

    IEnumerator EnableControl()
    {
        levelUI.AnnouncerTextLine1.gameObject.SetActive(true);
        levelUI.AnnouncerTextLine1.text = "Turn " + currentTurn;
        levelUI.AnnouncerTextLine1.color = Color.white;
        yield return new WaitForSeconds(2);

        levelUI.AnnouncerTextLine1.text = "3";
        levelUI.AnnouncerTextLine1.color = Color.green;
        yield return oneSec;
        levelUI.AnnouncerTextLine1.text = "2";
        levelUI.AnnouncerTextLine1.color = Color.yellow;
        yield return oneSec;
        levelUI.AnnouncerTextLine1.text = "1";
        levelUI.AnnouncerTextLine1.color = Color.red;
        yield return oneSec;

        levelUI.AnnouncerTextLine1.text = "FIGHT!";
        levelUI.AnnouncerTextLine1.color = Color.red;

        for(int i = 0; i < charM.players.Count; i++)
        {
            if (charM.players[i].playerType == PlayerBase.PlayerType.user)
            {
                InputHandler ih = charM.players[i].playerStates.gameObject.GetComponent<InputHandler>();
                ih.playerInput = charM.players[i].inputId;
                ih.enabled = true;
            }
        }

        yield return oneSec;
        levelUI.AnnouncerTextLine1.gameObject.SetActive(false);
        countdown = true;   
    }
    void DisableControl()
    {
        for(int i = 0; i < charM.players.Count; i++)
        {
            charM.players[i].playerStates.ResetStateInputs();

            if (charM.players[i].playerType == PlayerBase.PlayerType.user)
            {
                charM.players[i].playerStates.GetComponent<InputHandler>().enabled = false;
            }
        }
    }

    public void EndTurnFunction(bool timeout = false)
    {
        countdown = false;

        levelUI.LevelTimer.text = maxTurnTimer.ToString();

        if(timeout)
        {
            levelUI.AnnouncerTextLine1.gameObject.SetActive(true);
            levelUI.AnnouncerTextLine1.text = "Time Out!";
            levelUI.AnnouncerTextLine1.color = Color.cyan;
        }
        else
        {
            levelUI.AnnouncerTextLine1.gameObject.SetActive(true);
            levelUI.AnnouncerTextLine1.text = "K.O.!";
            levelUI.AnnouncerTextLine1.color = Color.red;
        }
        DisableControl();
        StartCoroutine("EndTurn");
    }

    IEnumerator EndTurn()
    {
        yield return new WaitForSeconds(3);
        PlayerBase vPlayer = FindWinningPlayer();
        if (vPlayer == null)
        {
            levelUI.AnnouncerTextLine1.text = "Draw";
            levelUI.AnnouncerTextLine1.color = Color.blue;
        }
        else
        {
            levelUI.AnnouncerTextLine1.text = vPlayer.playerId + " Wins!";
            levelUI.AnnouncerTextLine1.color = Color.red;
        }

        yield return new WaitForSeconds(2);

        if(vPlayer != null)
        {
            if(vPlayer.playerStates.health == 100)
            {
                levelUI.AnnouncerTextLine2.gameObject.SetActive(true);
                levelUI.AnnouncerTextLine2.text = "FLAWLESS VICTORY";
                levelUI.AnnouncerTextLine2.color = Color.red;
            }
        }

        yield return new WaitForSeconds(3);
        currentTurn++;

        bool matchOver = IsMatchOver();

        if (!matchOver)
        {
            StartCoroutine("InitTurn");
        }
        else
        {
            for(int i = 0; i < charM.players.Count; i++)
            {
                charM.players[i].score = 0;
                charM.players[i].hasCharacter = false;
            }
            SceneManager.LoadSceneAsync("select");
        }
    }

    

    bool IsMatchOver()
    {
        bool retVal = false;

        for(int i = 0; i < charM.players.Count; i++)
        {
            if (charM.players[i].score >= maxTurns)
            {
                retVal = true;
                break;
            }
        }

        return retVal;
    }

    PlayerBase FindWinningPlayer()
    {
        PlayerBase retVal = null;
        StateManager targetPlayer = null;

        if (charM.players[0].playerStates.health != charM.players[1].playerStates.health)
        {
            if (charM.players[0].playerStates.health < charM.players[1].playerStates.health)
            {
                charM.players[1].score++;
                targetPlayer = charM.players[1].playerStates;
                levelUI.AddWinIndicator(1);
            }
            else
            {
                charM.players[0].score++;
                targetPlayer = charM.players[0].playerStates;
                levelUI.AddWinIndicator(0);
            }

            retVal = charM.returnPlayerFromStates(targetPlayer);
        }
        return retVal;
    }

    public static LevelManager instance;
    public static LevelManager GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
    }
}
