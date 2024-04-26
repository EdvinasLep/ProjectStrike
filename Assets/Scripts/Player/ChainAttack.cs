using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;


public class ChainAttack : MonoBehaviour
{
    public int chainCount;
    public int prevCount;
    public int comboMultiplier = 2;
    public bool attack;
    public bool reset;
    public bool interrupted;

    public float timeSinceLastAttack = 0f;
    private float chainWindow = 2.0f;
    
    StateManager states;
    void Start()
    {
        states = GetComponent<StateManager>();
        chainCount = 0;
        prevCount = 0;
        interrupted = false;
    }

    void FixedUpdate()
    {

        if (timeSinceLastAttack < chainWindow)
        {
            timeSinceLastAttack += Time.deltaTime;
        }
        else
        {
            chainReset();
        }

        if (attack)
        {
            if (timeSinceLastAttack < chainWindow )
            {               
                chainCount++;
                attack = false;
                StartCoroutine(states.ComboLerp());
            }
            timeSinceLastAttack = 0f;
        }

        if (prevCount != chainCount)
        {
            states.combo = chainCount;
            prevCount = chainCount;
        }

        if (interrupted)
        {
            chainReset();
            interrupted = false;
        }
    }

    private void chainReset()
    {
        if(!interrupted)
            states.increaseEnergy(chainCount * comboMultiplier);

        chainCount = 0;   
    }
}
