using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PotraitInfo : MonoBehaviour {

    public int posX;
    public int posY;
    public string characterId;
    public Button portraitButton;
    public string characterName;

    void Awake()
    {
        portraitButton = GetComponent<Button>();
    }
}
