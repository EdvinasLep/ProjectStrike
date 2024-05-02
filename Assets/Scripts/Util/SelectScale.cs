using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectScale : MonoBehaviour
{
    public bool isToggled;
    public bool wasToggled;

    public int x;
    public int y;

    public string charName;

    // Update is called once per frame
    void Update()
    {
        if(isToggled)
        {
            wasToggled = true;
            ScalePortrait(1.8f);
        }
        else if (!isToggled && wasToggled)
        {
            ScalePortrait(1.5f);
            wasToggled = false;
        }
    }

    private void ScalePortrait(float value)
    {
        gameObject.transform.localScale = new Vector3(Mathf.Lerp(transform.localScale.x, value, 1.6f), Mathf.Lerp(transform.localScale.y, value, 1.6f), Mathf.Lerp(transform.localScale.z, value, 1.6f));
    }
}
