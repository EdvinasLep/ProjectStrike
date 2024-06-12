using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public int numberOfUsers;
    public bool egypt;
    public bool siberia;
    public List<PlayerBase> players = new List<PlayerBase>(); // list of players and their types

    public List<CharacterBase> characterList = new List<CharacterBase>();
    public CharacterBase returnCharacterWithID(string id)
    {
        CharacterBase retVal = null;

        for (int i = 0; i < characterList.Count; i++)
        {
            if (string.Equals(characterList[i].charId, id))
            {
                retVal = characterList[i];
                break;
            }
        }

        return retVal;
    }

    public PlayerBase returnPlayerFromStates(StateManager states)
    {
        PlayerBase retval = null;

        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].playerStates == states)
            {
                retval = players[i];
                break;
            }
        }
        return retval;
    }

    public PlayerBase returnOppositePlayer(PlayerBase p1)
    {
        PlayerBase retVal = null;

        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] != p1)
            {
                retVal = players[i];
                break;
            }
        }

        return retVal;
    }

    public static CharacterManager instance;
    public static CharacterManager GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
}

[System.Serializable]
public class CharacterBase
{
    public string charId;
    public GameObject prefab;
    public GameObject portrait;
}

[System.Serializable]
public class PlayerBase
{
    public string playerId;
    public string inputId;
    public PlayerType playerType;
    public bool hasCharacter;
    public GameObject playerPrefab;
    public GameObject portraitPrefab;
    public StateManager playerStates;
    public SmoothSlider sliders;
    public int score;

    public enum PlayerType
    {
        user,
        ai
    }
}