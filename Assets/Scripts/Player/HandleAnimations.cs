using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HandleAnimations : MonoBehaviour
{
    public Animator anim;
    StateManager states;
    public AudioManager audioManager;
    public bool isDead;

    public float attackRate = .3f;
    //public AttacksBase[] attacks = new AttacksBase[2];

    // Start is called before the first frame update
    void Start()
    {
        states = GetComponent<StateManager>();
        anim = GetComponent<Animator>();
        //audioManager = AudioManager.GetInstance();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //states.dontMove = anim.GetBool("DontMove");
        if(states.gettingHit)
        {
            anim.SetTrigger("Damaged");
        }
        
        //anim.SetBool("OnAir", !states.onGround);
        //anim.SetBool("Crouch", states.crouch);
        anim.SetBool("Attack1", states.attack1);
        anim.SetBool("Attack2", states.attack2);
        anim.SetBool("Attack3", states.attack3);
        anim.SetBool("Block", states.block);
        anim.SetBool("Dead", states.isDead);
        
        
        //anim.SetBool("Ultimate", states.ultimateAbility);

        float movement = states.movement.x;
        anim.SetFloat("Movement", movement);

        if (states.movement.y < 0)
        {
            states.crouch = true;
        }
        else
        {
            states.crouch = false;
        }

        //if(!states.isDead)
        //{
        //    anim.set
        //}
        //HandleAttacks();
    }

    //void HandleAttacks()
    //{
    //    if (states.canAttack)
    //    {
    //        if (states.attack1)
    //        {
    //            attacks[0].attack = true;
    //            attacks[0].attackTimer = 0;
    //            attacks[0].timesPressed++;
    //        }

    //        if (attacks[0].attack)
    //        {
    //            attacks[0].attackTimer += Time.deltaTime;

    //            if (attacks[0].attackTimer > attackRate || attacks[0].timesPressed >= 3)
    //            {
    //                attacks[0].attackTimer = 0;
    //                attacks[0].attack = false;
    //                attacks[0].timesPressed = 0;
    //            }

    //        }

    //        if (states.attack2)
    //        {
    //            attacks[1].attack = true;
    //            attacks[1].attackTimer = 0;
    //            attacks[1].timesPressed++;
    //        }

    //        if (attacks[1].attack)
    //        {
    //            attacks[1].attackTimer += Time.deltaTime;

    //            if (attacks[1].attackTimer > attackRate || attacks[1].timesPressed >= 3)
    //            {
    //                attacks[1].attackTimer = 0;
    //                attacks[1].attack = false;
    //                attacks[1].timesPressed = 0;
    //            }

    //        }
    //        anim.SetBool("Attack1", attacks[0].attack);
    //        anim.SetBool("Attack2", attacks[1].attack);
    //    }
    //}

    public void JumpAnim()
    {
        anim.SetBool("Attack1", false);
        anim.SetBool("Attack2", false);
        anim.SetBool("Jump", true);
        StartCoroutine(CloseBoolInAnim("Jump"));
    }

    IEnumerator CloseBoolInAnim(string name)
    {
        yield return new WaitForSeconds(0.5f);
        anim.SetBool(name, false);
    }
}

//[System.Serializable]
//public class AttacksBase
//{
//    public bool attack;
//    public float attackTimer;
//    public int timesPressed;
//}
