using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StateManager : MonoBehaviour
{
    public int health = 200;
    public int energy = 0;
    public int combo = 0;


    public float horizontal;
    public float vertical;
    public bool attack1;
    public bool attack2;
    public bool attack3;
    public bool crouch;
    public bool block;
    public bool canChainAttack;
    public bool showCombo;

    public bool canAttack;
    public bool gettingHit;
    public bool landed;
    public bool currentlyAttacking;
    public bool ultimateAvailable;

    public bool lookRight;
    public bool dontMove;
    public bool onGround;
    
    public Slider healthSlider;
    public Slider energySlider;
    SpriteRenderer sRenderer;

    public AudioManager audioManager;

    public ChainAttack chainAttack;
    public TMP_Text chainCount;

    [HideInInspector]
    public HandleDamageColliders handleDC;
    [HideInInspector]
    public HandleAnimations handleAnim;
    [HideInInspector]
    public HandleMovement handleMovement;

    public GameObject[] movementColliders;

    ParticleSystem blood;
    // Start is called before the first frame update
    void Start()
    {
        handleDC = GetComponent<HandleDamageColliders>();
        handleAnim = GetComponent<HandleAnimations>();
        handleMovement = GetComponent<HandleMovement>();
        sRenderer = GetComponentInChildren<SpriteRenderer>();
        blood = GetComponentInChildren<ParticleSystem>();
        audioManager = AudioManager.GetInstance();
        chainAttack = GetComponent<ChainAttack>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        sRenderer.flipX = lookRight;

        onGround = isOnGround();

        if (healthSlider != null || energySlider != null)
        {
            healthSlider.value = health * 0.01f;
            energySlider.value = energy * 0.01f;
        }


        if(chainCount != null)
        {
            chainCount.text = "Combo x" + combo.ToString();
        }

        if(showCombo)
        {
            chainCount.gameObject.SetActive(true);
        }
        else chainCount.gameObject.SetActive(false);

        if (health <= 0)
        {
            if(LevelManager.GetInstance().countdown)
            {
                LevelManager.GetInstance().EndTurnFunction();
                handleAnim.anim.Play("Dead");
            }
        }

        if(attack1 || attack2 || attack3)
        {
            audioManager.punch();
        }

        if (energy == 100)
        {
            ultimateAvailable = true;
        }

        if(gettingHit)
        {
            canChainAttack = false;
            //chainAttack.chainReset();
        }
        
    }

    private void Update()
    {
        
    }

    public void increaseEnergy(int increase)
    {
        if(energy <= 100)
            energy += increase;
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
        horizontal = 0;
        vertical = 0;
        combo = 0;
        attack1 = false;
        attack2 = false;
        attack3 = false;
        crouch = false;
        gettingHit = false;
        currentlyAttacking = false;
        dontMove = false;
        block = false;
        ultimateAvailable = false;
        showCombo = false;
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
        if (!block)
        {
            if (!gettingHit)
            {
                switch (damageType)
                {
                    case HandleDamageColliders.DamageType.light:
                        StartCoroutine(CloseImmortality(0.3f));
                        break;
                    case HandleDamageColliders.DamageType.heavy:
                        handleMovement.AddVelocityOnCharacter(
                            (((!lookRight) ? Vector3.right * 2 : Vector3.right * -2) + Vector3.up * 10).normalized
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
                otherChar.canChainAttack = true;
                otherChar.chainAttack.attack = true;
                gettingHit = true;
                audioManager.getHit();
            }
        }
    }
    
    IEnumerator CloseImmortality(float timer)
    {
        yield return new WaitForSeconds(timer);
        gettingHit = false;
    }



}
