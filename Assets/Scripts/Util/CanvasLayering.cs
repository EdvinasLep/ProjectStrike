using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CanvasLayering : MonoBehaviour
{
    Canvas myCanvas;
    // Start is called before the first frame update

    private void Awake()
    {
        myCanvas = GetComponent<Canvas>();
    }
    void Start()
    {
        myCanvas.sortingLayerName = "Portrait";
        myCanvas.sortingOrder = 1;
        myCanvas.sortingLayerName = "Indicator";
        myCanvas.sortingOrder = 2;
    }
}
