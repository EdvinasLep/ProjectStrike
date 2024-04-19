using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;

public class SpineAnimHandler : MonoBehaviour
{
    public float attackRate = .3f;
    //AttacksBase[] attacks = new AttacksBase[2];

    StateManager states;
    SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset idle, walkfwd, walkback, attack1, attack2, attack3, gettingHit, death, win;
    InputHandler inputHandler;
    public string currentState;
    public string currentAnimation;

    public HandleDamageColliders handleDC;

    public bool att1;
    public bool att2;
    public bool att3;

    public bool gotHit;
    public bool isDead;




    void Start()
    {
        states = GetComponent<StateManager>();
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        handleDC = GetComponent<HandleDamageColliders>();

        skeletonAnimation.AnimationState.Start += HandleAnimationStart;
        skeletonAnimation.AnimationState.Complete += HandleAnimationEnd;
        
        isDead = false;
        //audioManager = AudioManager.GetInstance();


    }

    public void SetAnimation(AnimationReferenceAsset animation, bool loop, float timeScale)
    {
        TrackEntry currentTrack = skeletonAnimation.AnimationState.GetCurrent(0);

        if (currentTrack != null && currentTrack.Animation.Name == animation.name) 
        {
            //Debug.Log($"Trying to set animation: {animation.name}. Current animation: {currentTrack?.Animation.Name}");
            return;
        }
        if (currentTrack != null)
        {
            if (!CanInterrupt(currentTrack.Animation.Name) && !currentTrack.IsComplete)
            {
                //Debug.Log("Current animation cannot be interrupted and is not complete.");
                return;
            }
        }


        //skeletonAnimation.AnimationState.SetAnimation(0, animation, loop).TimeScale = timeScale;
        var newTrack = skeletonAnimation.AnimationState.SetAnimation(0, animation, loop);
        newTrack.TimeScale = timeScale;
        currentAnimation = animation.name;

        //Debug.Log($"Animation set to {animation.name} with loop {loop} and time scale {timeScale}.");
    }

    private void HandleAnimationStart(TrackEntry trackEntry)
    {
        
        switch (trackEntry.Animation.Name)
        {
            case "Huk_L":
                Debug.Log("COLLIDER ATTACK!");
                states.handleDC.OpenCollider(HandleDamageColliders.DCtype.up, 0.1f, HandleDamageColliders.DamageType.punch);
                Debug.Log("Opening Collider");
                break;
            case "Huk_R":
                states.handleDC.OpenCollider(HandleDamageColliders.DCtype.bottom, 0.1f, HandleDamageColliders.DamageType.punch);
                break;
            case "Kik_R":
                states.handleDC.OpenCollider(HandleDamageColliders.DCtype.up, 0.1f, HandleDamageColliders.DamageType.kick);
                break;
            case "Death":
                    Debug.Log("Got hit 3 START");
                break;
        }

        

    }

    private void HandleAnimationEnd(TrackEntry trackEntry)
    {
        states.handleDC.CloseCollliders();
        if (trackEntry.Animation.Name == attack1.name)
        {
            att1 = false;
            states.currentlyAttacking = false;
        }
        if (trackEntry.Animation.Name == attack2.name)
        {
            att2 = false;
            states.currentlyAttacking = false;
        }
        if (trackEntry.Animation.Name == attack3.name)
        {
            att3 = false;
            states.currentlyAttacking = false;
        }
        if (trackEntry.Animation.Name == gettingHit.name)
        {
            //Debug.Log("Got hit 2");
            gotHit = false;
        }
        if(trackEntry.Animation.Name == death.name)
        {
            isDead = true;
        }

        SetCharacterState();
    }

    private void Update()
    {
        UpdateStates();
        SetCharacterState();
        
        if(gotHit)
        {
            states.canAttack = false;
        }
        else states.canAttack = true;
    }

    public void SetCharacterState()
    {

        if (gotHit)
        {
            SetAnimation(gettingHit, false, 1f);
            //Debug.Log("Got hit 1");

        }

        if (!isDead)
        {
            if (states.health <= 0)
            {
                SetAnimation(death, false, 1f);
            }
        }
            

        if (states.canAttack)
        {
            if (att1)
            {
                SetAnimation(attack1, false, 1f);
                states.currentlyAttacking = true;
                return;
            }
            if (att2)
            {
                SetAnimation(attack2, false, 1f);
                states.currentlyAttacking = true;
                return;
            }
            if (att3)
            {
                SetAnimation(attack3, false, 1f);
                states.currentlyAttacking = true;
                return;
            }
        }
        
        if (Mathf.Abs(states.horizontal) > 0.1f)
        {
            if (states.horizontal > 0)
            {
                SetAnimation(walkfwd, true, 1f); return;
            }
            else
            {
                SetAnimation(walkback, true, 1f); return;
            }
        }
        else if(!isDead) SetAnimation(idle, true, 1f);

        if (states.ultimateAbility)
        {
                    SetAnimation(win, false, 1f);
                    return;
        }
            
    }

    public void UpdateStates()
    {
        if (states.attack1 && !att1) att1 = true;
        if (states.attack2 && !att2) att2 = true;
        if (states.attack3 && !att3) att3 = true;
        if (states.gettingHit && !gotHit) gotHit = true;

    }

    private bool CanInterrupt(string animationName)
    {
        if (animationName == idle.name || animationName == walkfwd.name || animationName == walkback.name) return true;
        else return false;
    }
}


