using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleMovement : MonoBehaviour
{
    Rigidbody2D rb;
    StateManager states;
    SpineAnimHandler anim;

    public float jumpForce = 400f;
    public float move = 10f;
    public float accelaration = 60;
    public float airAccelaration = 15;
    public float maxSpeed = 240;
    public float jumpSpeed = 14;
    public float jumpDuration = 150;
    float actualSpeed;
    bool justJumped;
    bool canVariableJump;
    float jmpTimer;
    private Vector3 m_Velocity = Vector3.zero;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        states = GetComponent<StateManager>();
        anim = GetComponent<SpineAnimHandler>();
        rb.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        if(!states.dontMove)
        {
            HorizontalMovement();
            Jump();
        }
    }

    void HorizontalMovement()
    {
        actualSpeed = this.maxSpeed;

        if(states.onGround && !states.currentlyAttacking)
        {
            //rb.AddForce(new Vector2((states.horizontal * actualSpeed) - rb.velocity.x * this.accelaration, 0));
            //Vector3 targetVelocity = new Vector2(states.horizontal * move, rb.velocity.y);
            // And then smoothing it out and applying it to the character
            //rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref m_Velocity, 0.05f);
            rb.velocity = new Vector2(states.movement.x * actualSpeed, rb.velocity.y);
        }

        // sliding fix
        if(states.movement.x == 0 && states.onGround)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }
    
    void Jump()
    {
        //if(states.vertical > 0)
        //{
        //    if(!justJumped)
        //    {
        //        justJumped = true;
        //        if(states.onGround)
        //        {
        //            //anim.JumpAnim();
        //            //rb.velocity = new Vector3(rb.velocity.x, this.jumpSpeed);
        //            rb.AddForce(new Vector2(0f, jumpForce));
        //            jmpTimer = 0;
        //            canVariableJump = true;
        //        }
        //    }
        //    else
        //    {
        //        if (canVariableJump)
        //        {
        //            jmpTimer += Time.deltaTime;
        //            if (jmpTimer < this.jumpDuration / 1000)
        //            {
        //                //rb.velocity = new Vector3(rb.velocity.x, this.jumpSpeed);
        //            }
        //        }
        //    }
        //}     
        //else
        //{
        //    justJumped = false;
        //}
    }

    public void AddVelocityOnCharacter(Vector3 direction, float timer)
    {
        StartCoroutine(AddVelocity(timer, direction));
    }

    IEnumerator AddVelocity(float timer, Vector3 direction)
    {
        float elapsedTime = 0;

        while (elapsedTime < timer)
        {
            elapsedTime += Time.deltaTime;

            rb.AddForce(direction * 5);
            yield return null;
        }
    }
}
