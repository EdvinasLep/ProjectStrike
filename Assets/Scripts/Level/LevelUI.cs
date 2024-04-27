using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class LevelUI : MonoBehaviour {

    public TMP_Text AnnouncerTextLine1;
    public TMP_Text AnnouncerTextLine2;
    public TMP_Text LevelTimer;

    public TMP_Text[] ChainCounters;

    public Slider[] healthSliders;
    public Slider[] delayedHealthSlider;
    public Slider[] energySliders;
    public Slider[] blockSliders;

    public GameObject[] winIndicatorGrids;
    public GameObject winIndicator;

    public GameObject[] portraitContainer;

    public static LevelUI instance;
    public static LevelUI GetInstance()
    {
        return instance;
    }

    void Awake()
    {
        instance = this;
    }

    public void AddWinIndicator(int player, int score)
    {
        //if (score == 0)
        //{
        //    Transform existingIndicator = winIndicatorGrids[player].transform.GetChild(0);
        //    Destroy(existingIndicator.gameObject);
        //    Debug.Log("INDICATOR REMOVED");
        //}
        //else if (score == 1)
        //{
        //    Transform existingIndicator = winIndicatorGrids[player].transform.GetChild(1);
        //    Destroy(existingIndicator.gameObject);
        //}
        Transform existingIndicator = winIndicatorGrids[player].transform.GetChild(0);
        Destroy(existingIndicator.gameObject);
        Debug.Log("INDICATOR REMOVED");

        GameObject go = Instantiate(winIndicator, transform.position, Quaternion.identity) as GameObject;
        go.transform.SetParent(winIndicatorGrids[player].transform);
        go.transform.localScale = Vector3.one;
    }

    public void AddPortrait(int player, GameObject portrait)
    {
        GameObject go = Instantiate(portrait, transform.position, Quaternion.identity)as GameObject;
        go.transform.SetParent(portraitContainer[player].transform);
        go.transform.localScale = Vector3.one * 1.2f;

        if (player == 1)
        {
            Vector3 flip = go.transform.localScale;
            flip.x *= -1;
            go.transform.localScale = flip;
        }
    }
}
