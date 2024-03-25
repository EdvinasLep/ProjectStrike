using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public string playerInput;

    float horizontal;
    float vertical;
    bool attack1;
    bool attack2;
    bool attack3;
    bool ultimate;
    bool block;

    StateManager states;

    private void Start()
    {
        states = GetComponent<StateManager>();
    }

    private void FixedUpdate()
    {
        horizontal = Input.GetAxis("Horizontal" + playerInput);
        vertical = Input.GetAxis("Vertical" + playerInput);
        attack1 = Input.GetButton("Fire1" + playerInput);
        attack2 = Input.GetButton("Fire2" + playerInput);
        attack3 = Input.GetButton("Fire3" + playerInput);

        states.horizontal = horizontal;
        states.vertical = vertical;
        states.attack1 = attack1;
        states.attack2 = attack2;
        states.attack3 = attack3;

        if (states.ultimateAvailable)
        {
            ultimate = Input.GetButton("Ultimate1" + playerInput);
            states.ultimateAbility = ultimate;
        }
        else ultimate = false;

        if (states.canBlock)
        {
            block = Input.GetButton("Block1" + playerInput);
        }
        else block = false;

        states.ultimateAbility = ultimate;
        states.block = block;


    }
}
