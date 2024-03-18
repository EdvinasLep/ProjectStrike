using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class LevelUI : MonoBehaviour {

    public Text AnnouncerTextLine1;
    public Text AnnouncerTextLine2;
    public Text LevelTimer;

    public TMP_Text[] ChainCounters;

    public Slider[] healthSliders;
    public Slider[] energySliders;

    public GameObject[] winIndicatorGrids;
    public GameObject winIndicator;

    public static LevelUI instance;
    public static LevelUI GetInstance()
    {
        return instance;
    }

    void Awake()
    {
        instance = this;
    }

    public void AddWinIndicator(int player)
    {
        GameObject go = Instantiate(winIndicator, transform.position, Quaternion.identity) as GameObject;
        go.transform.SetParent(winIndicatorGrids[player].transform);
    }
}
