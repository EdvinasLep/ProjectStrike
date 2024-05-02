using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputManager : MonoBehaviour
{
    public bool attack1;
    public bool attack2;
    public bool attack3;
    public bool ultimate;
    public bool block;
    public float horizontal;
    public float vertical;

    public Vector2 movement;
    
    //public InputActionReference _movement;
    //public InputActionReference _attack1;
    //public InputActionReference _attack2;
    //public InputActionReference _attack3;
    //public InputActionReference _block;
    StateManager states;

    // Start is called before the first frame update
    void Start()
    {
        states = GetComponent<StateManager>();
        //var playerInputs = GetComponent<PlayerInput>();
        //playerInputs.SwitchCurrentControlScheme(playerInputs.defaultControlScheme, Keyboard.current);


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        

        states.movement = movement;
        states.attack1 = attack1;
        states.attack2 = attack2;
        states.attack3 = attack3;
        states.block = block;
    }

    public void Movement(InputAction.CallbackContext context)
    {
        movement = context.action.ReadValue<Vector2>();
    }
    public void Attack1(InputAction.CallbackContext context)
    {
        if(context.performed) attack1 = true;
        if(context.canceled) attack1 = false;

    }

    public void Attack2(InputAction.CallbackContext context)
    {
        if (context.performed) attack2 = true;
        if (context.canceled) attack2 = false;
    }

    public void Attack3(InputAction.CallbackContext context)
    {
        if (context.performed) attack3 = true;
        if (context.canceled) attack3 = false;
    }

    public void Block(InputAction.CallbackContext context)
    {
        if(context.performed) block = true;
        if(context.canceled) block = false;
    }
}
