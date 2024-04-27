using UnityEngine;
using System.Collections;
using TMPro;

[System.Serializable]
public class ButtonRef : MonoBehaviour {

    public GameObject selectIndicator;
    private TextMeshProUGUI tmpText;

    public bool selected = false;

    void Start()
    {
        tmpText = selectIndicator.GetComponentInChildren<TextMeshProUGUI>();
        //tmpText.fontMaterial.SetFloat(ShaderUtilities.ID_UnderlayOffsetX, 0);
    }

    void Update()
    {
        if(selected)
        {
            tmpText.fontMaterial.SetFloat(ShaderUtilities.ID_UnderlayOffsetX, 1);
            tmpText.fontMaterial.SetFloat(ShaderUtilities.ID_UnderlayDilate, 1);
        }
        else
        {
            tmpText.fontMaterial.SetFloat(ShaderUtilities.ID_UnderlayOffsetX, 0);
            tmpText.fontMaterial.SetFloat(ShaderUtilities.ID_UnderlayDilate, 0);
        }
    }
}
