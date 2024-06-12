using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiController : MonoBehaviour
{
    StateManager states;
    public StateManager enemyStates;

    public float closeCombatTolerance = 3;

    public float normalRate = 1;
    float nrmTimer;

    public float closeRate = 0.5f;
    float clTimer;

    public float blockingRate = 1.5f;
    float blTimer;

    public float aiStateReset = 1;
    float aiTimer;

    bool initiateAI;
    bool closeCombat;

    bool gotRandom;
    float storeRandom;

    bool checkForBlocking;
    bool blocking;
    bool blockMultiplier;

    bool randomizeAttacks;
    int numberOfAttacks;
    int curNumAttacks;

    public float jumpRate = 1;
    float jRate;
    bool jump;
    float jTimer;

    public AttackPatterns[] attackPatterns;

    public enum AIState
    {
        closeState,
        normalState,
        resetAI
    }

    public AIState currentState;

    // Start is called before the first frame update
    void Start()
    {
        states = GetComponent<StateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckDistance();
        States();
        AIAgent();
    }

    void States()
    {
        switch(currentState)
        {
            case AIState.closeState:
                CloseState();
                break;
            case AIState.normalState:
                NormalState();
                break;
            case AIState.resetAI:
                ResetAI();
                break;
        }

        Blocking();
        //Jumping(); Implement later
    }



    void AIAgent()
    {
        if(initiateAI)
        {
            currentState = AIState.resetAI;
            float multiplier = 0;

            if(!gotRandom)
            {
                storeRandom = ReturnRandom();
                gotRandom = true;
            }

            if(!closeCombat)
            {
                multiplier += 30;
            }
            else
            {
                multiplier -= 30;
            }

            if(storeRandom + multiplier <50)
            {
                Attack();
            }
            else
            {
                Movement();
            }
        }
    }

    void Attack()
    {
        if(!gotRandom)
        {
            storeRandom = ReturnRandom();
            gotRandom= true;
        }

        //Normal attacks
        //if(storeRandom < 75)
        //{
            if(!randomizeAttacks)
            {
                numberOfAttacks = (int)Random.Range(1, 3);
                randomizeAttacks = true;
            }

            if(curNumAttacks < numberOfAttacks)
        {
            //!!!implement random percentages for attacks later!!
            int attackNumber = Random.Range(0, attackPatterns.Length);
            StartCoroutine(OpenAttack(attackPatterns[attackNumber], 0));
            curNumAttacks++;
        }
        //}
        //else (special attacks not implemented yet!!
        //{
        //    if(curNumAttacks < 1)
        //    {
        //        states.SpecialAttack = true;
        //        curNumAttacks++;
        //    }
        //}
    }

    void Movement()
    {
        if (!gotRandom)
        {
            storeRandom = ReturnRandom();
            gotRandom = true;
        }

        if(storeRandom < 90)
        {
            if (enemyStates.transform.position.x < transform.position.x) // move towards enemy
            {
                states.movement.x = -1;
            }
            else
            {
                states.movement.x = 1;
            }
        }
        else
        {
            if (enemyStates.transform.position.x < transform.position.x) // move away from enemy
            {
                states.movement.x = 1;
            }
            else
            {
                states.movement.x = -1;
            }

        }
    }

    void ResetAI()
    {
        aiTimer += Time.deltaTime;

        if(aiTimer > aiStateReset)
        {
            initiateAI = false;
            states.movement.x = 0;
            //states.vertical = 0;
            aiTimer = 0;
            gotRandom = false;

            storeRandom = ReturnRandom();
            if(storeRandom < 50)
            {
                currentState = AIState.normalState;
            }
            else
            {
                currentState = AIState.closeState;
            }

            curNumAttacks = 1;
            randomizeAttacks = false;
        }
    }

    void CheckDistance()
    {
        float distance = Vector3.Distance(transform.position, enemyStates.transform.position);

        if(distance < closeCombatTolerance)
        {
            if(currentState != AIState.resetAI)
            {
                currentState = AIState.closeState;
            }

            closeCombat = true;
        }
        else
        {
            if (currentState != AIState.resetAI)
            {
                currentState = AIState.normalState;
            }

            if(closeCombat)
            {
                if(!gotRandom)
                {
                    storeRandom = ReturnRandom();
                    gotRandom = true;
                }

                if(storeRandom < 60)
                {
                    Movement();
                }
            }
            closeCombat = false;
        }
    }

    void Blocking()
    {
        if (states.gettingHit)
        {
            if (!gotRandom)
            {
                storeRandom = ReturnRandom();
                gotRandom = true;
            }

            if (storeRandom < 50 && states.canBlock && states.blockHealth != 100)
            {
                blocking = true;
                states.gettingHit = false;
                states.block = true;
            }
        }

        if (blocking)
        {
            blTimer += Time.deltaTime;

            if(blTimer > blockingRate)
            {
                states.block = false;
                blTimer = 0;
            }
        }
    }
    void NormalState()
    {
        nrmTimer += Time.deltaTime;

        if(nrmTimer > normalRate)
        {
            initiateAI = true;
            nrmTimer = 0;
        }
    }

    void CloseState()
    {
        clTimer += Time.deltaTime;

        if(clTimer > closeRate)
        {
            clTimer = 0;
            initiateAI = true;
        }
    }

    void Jumping()
    {
        if(!enemyStates.onGround)
        {
            int ranValue = (int)ReturnRandom();

            if(ranValue < 50)
            {
                jump = true;
            }
        }

        if(jump)
        {
            //states.vertical = 1;
            jRate = ReturnRandom();
            jump = false;
        }
        else
        {
            //states.vertical = 0;
        }

        jTimer = Time.deltaTime;
        if(jTimer > jumpRate*10)
        {
            if(jRate < 50)
            {
                jump = true;
            }
            else
            {
                jump = false;
            }
            jTimer = 0;
        }
    }

    float ReturnRandom()
    {
        float retVal = Random.Range(0, 101);
        return retVal;
    }

    IEnumerator OpenAttack(AttackPatterns attackPattern, int i)
    {
        int index = i;
        float delay = attackPattern.attacks[index].delay;
        states.attack1 = attackPattern.attacks[index].attack1;
        states.attack2 = attackPattern.attacks[index].attack2;
        yield return new WaitForSeconds(delay);

        states.attack1 = false;
        states.attack2 = false;

        if(index < attackPattern.attacks.Length -1)
        {
            index++;
            StartCoroutine(OpenAttack(attackPattern, index));
        }

    }
    [System.Serializable]
    public class AttackPatterns
    {
        public AttacksBase[] attacks;
    }

    [System.Serializable]
    public class AttacksBase 
    {
        public bool attack1;
        public bool attack2;
        public float delay;
    }
}
