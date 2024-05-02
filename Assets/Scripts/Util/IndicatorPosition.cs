using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorPosition : MonoBehaviour
{
    public GameObject SliderPosition;
    public Vector3 SliderPositionTransform;
    // Start is called before the first frame update
    void Start()
    {
        SliderPositionTransform = SliderPosition.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        SliderPositionTransform = SliderPosition.transform.position;
        transform.position = SliderPositionTransform;
    }
}
