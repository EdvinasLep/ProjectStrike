using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinLoad : MonoBehaviour
{
    // Start is called before the first frame update
    public float spinSpeed = 100f; // Speed of the spinning in degrees per second

    void Update()
    {
        // Rotate the icon around its Z axis
        transform.Rotate(0, 0, spinSpeed * Time.deltaTime);
    }
}
