using UnityEngine;
using System.Collections;
using TMPro;

[System.Serializable]
public class ButtonRef : MonoBehaviour {

    public GameObject selectIndicator;
    private TextMeshProUGUI tmpText;

    public bool selected;

    void Start()
    {
        tmpText = selectIndicator.GetComponentInChildren<TextMeshProUGUI>();
    }

    void Update()
    {
        if(selected)
        {
            tmpText.fontMaterial.SetFloat(ShaderUtilities.ID_UnderlayOffsetX, 1);
        }
        else
        {
            tmpText.fontMaterial.SetFloat(ShaderUtilities.ID_UnderlayOffsetX, 0);
        }
    }
}
