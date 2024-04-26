using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;

public class TMPmanager : MonoBehaviour
{
    public GameObject text;
    
    public Color color;

    public bool glow;
    public bool underlay;

    private TextMeshProUGUI tmpTEXT;

    // Start is called before the first frame update
    void Start()
    {
        tmpTEXT = text.GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {

        if(glow)
        {
            if(tmpTEXT.fontMaterial.GetFloat(ShaderUtilities.ID_GlowPower) > 0 )
            {
                tmpTEXT.fontSharedMaterial.SetColor(ShaderUtilities.ID_GlowColor, color);
            }
            else
            {
                tmpTEXT.fontMaterial.SetFloat(ShaderUtilities.ID_GlowColor, 1);
                tmpTEXT.fontSharedMaterial.SetColor(ShaderUtilities.ID_GlowColor, color);
            }
            

        }

        if(underlay)
        {
            tmpTEXT.fontMaterial.SetFloat(ShaderUtilities.ID_UnderlayOffsetX, 1);
        }
    }
}
