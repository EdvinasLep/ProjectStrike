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
