using System.Collections;
using System.Collections.Generic;
//using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;
using UnityEditor;

public class StateManager : MonoBehaviour
{
    public int health = 100;
    public int energy = 0;
    public int combo = 0;
    public int blockHealth = 0;

    public Vector2 movement;

    public bool attack1;
    public bool attack2;
    public bool attack3;
    public bool attack1ready;
    public bool attack2ready;
    public bool attack3ready;
    public bool crouch;
    public bool block;
    public bool canChainAttack;
    public bool ultimateAbility;
    

    public bool canAttack;
    public bool gettingHit;
    public bool landed;
    public bool currentlyAttacking;
    public bool ultimateAvailable;
    public bool canBlock = true;
    public bool isDead;

    public bool lookRight;
    public bool dontMove;
    public bool onGround;

    public AudioManager audioManager;

    public ChainAttack chainAttack;
    public TMP_Text chainCount;
    public BlockSystem blockSystem;
    

    [HideInInspector]
    public HandleDamageColliders handleDC;
    [HideInInspector]
    public Animator anim;
    [HideInInspector]
    public HandleMovement handleMovement;

    public GameObject[] movementColliders;

    ParticleSystem blood;
    // Start is called before the first frame update
    void Start()
    {
        handleDC = GetComponent<HandleDamageColliders>();
        //handleAnim = GetComponent<HandleAnimations>();
        handleMovement = GetComponent<HandleMovement>();
        anim = GetComponent<Animator>();
        blood = GetComponentInChildren<ParticleSystem>();
        audioManager = AudioManager.GetInstance();
        chainAttack = GetComponent<ChainAttack>();
        blockSystem = GetComponent<BlockSystem>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(health <= 0 && !isDead)
        {
            isDead = true;
            anim.SetTrigger("IsDead");
        }
        if (lookRight)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

        onGround = isOnGround();

        if(chainCount != null)
        {
            
            chainCount.text = "COMBO X" + combo.ToString();
        }

        if(chainCount != null && combo > 0)
        {
            chainCount.gameObject.SetActive(true);
        }
        else if(chainCount != null) chainCount.gameObject.SetActive(false);


        if (health <= 0)
        {
            if(LevelManager.GetInstance().countdown)
            {
                LevelManager.GetInstance().EndTurnFunction();
                
            }
        }

        if(attack1 || attack2 || attack3)
        {
            //audioManager.punch();
        }

        if (energy >= 100)
        {
            ultimateAvailable = true;
        }

        if (ultimateAbility)
        {
            StartCoroutine(ResetUltimate(0.5f));
        }

    }

    public IEnumerator ComboLerp()
    {
        float time = 0f;
        
        while(time < 0.15f)
        {
            time += Time.deltaTime;
            chainCount.fontSize = Mathf.Lerp(90, 30, time/0.2f);
            yield return null;
        }
    }

    IEnumerator ResetUltimate(float timer)
    {
        yield return new WaitForSeconds(timer);
        energy = 0;
        ultimateAvailable = false;
        ultimateAbility = false;
    }

    public void increaseEnergy(int increase)
    {
        if(energy <= 100) 
        {
            energy += increase;
            //Debug.Log(increase);
        }
            
    }



    bool isOnGround()
    {
        bool retVal = false;

        LayerMask layer = ~(1 << gameObject.layer | 1 << 3);
        retVal = Physics2D.Raycast(transform.position, -Vector2.up, 0.1f, layer);
        return retVal;


    }

    public void ResetStateInputs()
    {
        combo = 0;
        blockHealth = 0;
        attack1 = false;
        attack2 = false;
        attack3 = false;
        crouch = false;
        gettingHit = false;
        currentlyAttacking = false;
        dontMove = false;
        block = false;
        ultimateAvailable = false;
        ultimateAbility = false;
        canBlock = true;
        canAttack = true;
    }

    public void CloseMovementCollider(int index)
    {
        movementColliders[index].SetActive(false);
    }

    public void OpenMovementCollider(int index)
    {
        movementColliders[index].SetActive(true);
    }

    public void TakeDamage(int damage, int energyInc, HandleDamageColliders.DamageType damageType, StateManager otherChar)
    {
        if (!currentlyAttacking)
        {
            if (!block)
            {
                if (!gettingHit)
                {
                    switch (damageType)
                    {
                        case HandleDamageColliders.DamageType.punch:
                            StartCoroutine(CloseImmortality(0.3f));
                            break;
                        case HandleDamageColliders.DamageType.kick:
                            handleMovement.AddVelocityOnCharacter(
                                (((!lookRight) ? Vector3.right * 2 : Vector3.right * -2) + Vector3.up * 2).normalized
                                , 0.5f
                                );

                            StartCoroutine(CloseImmortality(1f));
                            break;
                        case HandleDamageColliders.DamageType.ultimate:
                            handleMovement.AddVelocityOnCharacter(
                                (((!lookRight) ? Vector3.right * 4 : Vector3.right * -4) + Vector3.up * 4).normalized
                                , 0.5f
                                );

                            StartCoroutine(CloseImmortality(1f));
                            break;
                    }
                    if (blood != null)
                    {
                        blood.Emit(30);
                    }
                    health -= damage;
                    otherChar.increaseEnergy(energyInc);
                    otherChar.chainAttack.attack = true;
                    chainAttack.attack = false;
                    chainAttack.interrupted = true;
                    gettingHit = true;
                    //audioManager.getHit();
                }
            }
            else
            {
                blockSystem.isHit = true;
            }
        }
    }
    
    IEnumerator CloseImmortality(float timer)
    {
        yield return new WaitForSeconds(timer);
        gettingHit = false;
    }



}
