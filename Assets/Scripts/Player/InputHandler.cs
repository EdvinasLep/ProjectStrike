//using UnityEngine;

//public class InputHandler : MonoBehaviour
//{
//    public string playerInput;

//    public bool attack1, attack1Handled;
//    public bool attack2, attack2Handled;
//    public bool attack3, attack3Handled;
//    public float horizontal;
//    public float vertical;
//    public bool ultimate;
//    public bool block;

//    StateManager states;

//    private void Start()
//    {
//        states = GetComponent<StateManager>();
//    }

//    private void FixedUpdate()
//    {
//        horizontal = Input.GetAxis("Horizontal" + playerInput);
//        vertical = Input.GetAxis("Vertical" + playerInput);
//        attack1 = Input.GetButton("Fire1" + playerInput);
//        attack2 = Input.GetButton("Fire2" + playerInput);
//        attack3 = Input.GetButton("Fire3" + playerInput);

//        if (states.canBlock)
//        {
//            block = Input.GetButton("Block1" + playerInput);
//        }
//        else block = false;

//        UpdateStates();
//    }


//    void UpdateStates()
//    {
//        states.horizontal = horizontal;
//        states.vertical = vertical;
//        states.attack1 = attack1;
//        states.attack2 = attack2;
//        states.attack3 = attack3;

//        states.ultimateAbility = ultimate;
//        states.block = block;
//    }
    
//    // Public methods called by animation handler to reset attack flags
//    //public void ResetAttack1()
//    //{
//    //    states.attack1 = false;
//    //    attack1Handled = false;
//    //}

//    //public void ResetAttack2()
//    //{
//    //    states.attack2 = false;
//    //    attack2Handled = false;
//    //}

//    //public void ResetAttack3()
//    //{
//    //    states.attack3 = false;
//    //    attack3Handled = false;
//    //}
//}
