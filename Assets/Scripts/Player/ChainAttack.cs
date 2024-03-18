using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;


public class ChainAttack : MonoBehaviour
{
    public int chainCount;
    public int prevCount;
    public bool attack;

    public float timeSinceLastAttack = 0f;
    private float chainWindow = 2.0f;
    
    StateManager states;
    void Start()
    {
        states = GetComponent<StateManager>();
        chainCount = 0;
        prevCount = 0;
    }

    void Update()
    {

        if (timeSinceLastAttack < chainWindow)
        {
            timeSinceLastAttack += Time.deltaTime;
        }
        else chainCount = 0;

        if (attack)
        {
            if (timeSinceLastAttack < chainWindow )
            {               
                chainCount++;
                attack = false;
                states.showCombo = true;

            }
            else if (timeSinceLastAttack > chainWindow)
            {
                chainReset();
                attack = false;
            }
            timeSinceLastAttack = 0f;
        }

        if(prevCount != chainCount)
        {
            states.combo = chainCount;
            prevCount = chainCount;
        }

    }

    public void chainReset()
    {
        chainCount = 0;
        states.showCombo = false;
    }
}
