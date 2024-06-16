using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiController : MonoBehaviour
{
    StateManager states;
    public StateManager enemyStates;

    public float closeCombatTolerance = 7f;
    public float aggressiveRate = 0.1f;  // Reduced for quicker decisions
    public float defensiveRate = 0.5f;  // Reduced for quicker defensive actions
    public float blockRate = 0.3f;  // Reduced to be more responsive

    private float aiTimer;
    private float nrmTimer;
    private float clTimer;
    private float blTimer;
    private bool initiateAI;
    private bool closeCombat;

    private bool gotRandom;
    private float storeRandom;

    private bool blocking;

    private bool randomizeAttacks;
    private int numberOfAttacks;
    private int curNumAttacks;

    public AttackPatterns[] attackPatterns;

    public enum AIState
    {
        closeState,
        normalState,
        aggressiveState,
        defensiveState,
        resetAI
    }

    public AIState currentState;

    void Start()
    {
        states = GetComponent<StateManager>();
        currentState = AIState.normalState;
    }

    void Update()
    {
        CheckDistance();
        States();
        AIAgent();
    }

    void States()
    {
        switch (currentState)
        {
            case AIState.closeState:
                CloseState();
                break;
            case AIState.normalState:
                NormalState();
                break;
            case AIState.aggressiveState:
                AggressiveState();
                break;
            case AIState.defensiveState:
                DefensiveState();
                break;
            case AIState.resetAI:
                ResetAI();
                break;
        }

        Blocking();
    }

    void AIAgent()
    {
        if (initiateAI)
        {
            currentState = AIState.resetAI;
            float multiplier = 0;

            if (!gotRandom)
            {
                storeRandom = ReturnRandom();
                gotRandom = true;
            }

            if (!closeCombat)
            {
                multiplier += 30;
            }
            else
            {
                multiplier -= 30;
            }

            if (storeRandom + multiplier < 50)
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
        if (!gotRandom)
        {
            storeRandom = ReturnRandom();
            gotRandom = true;
        }

        if (!randomizeAttacks)
        {
            numberOfAttacks = Random.Range(2, 4);  // Increased to 2-3 attacks in a sequence
            randomizeAttacks = true;
        }

        if (curNumAttacks < numberOfAttacks)
        {
            int attackNumber = Random.Range(0, attackPatterns.Length);
            StartCoroutine(OpenAttack(attackPatterns[attackNumber], 0));
            curNumAttacks++;
        }
        else
        {
            randomizeAttacks = false;
            curNumAttacks = 0;
        }
    }

    void Movement()
    {
        if (!gotRandom)
        {
            storeRandom = ReturnRandom();
            gotRandom = true;
        }

        if (storeRandom < 90)
        {
            if (enemyStates.transform.position.x < transform.position.x)
            {
                states.movement.x = -1; // move towards enemy
            }
            else
            {
                states.movement.x = 1; // move towards enemy
            }
        }
        else
        {
            if (enemyStates.transform.position.x < transform.position.x)
            {
                states.movement.x = 1; // move away from enemy
            }
            else
            {
                states.movement.x = -1; // move away from enemy
            }
        }
    }

    void ResetAI()
    {
        aiTimer += Time.deltaTime;

        if (aiTimer > defensiveRate)
        {
            initiateAI = false;
            states.movement.x = 0;
            aiTimer = 0;
            gotRandom = false;

            storeRandom = ReturnRandom();
            if (storeRandom < 50)
            {
                currentState = AIState.normalState;
            }
            else
            {
                currentState = AIState.closeState;
            }

            curNumAttacks = 0;
            randomizeAttacks = false;
        }
    }

    void CheckDistance()
    {
        float distance = Vector3.Distance(transform.position, enemyStates.transform.position);

        if (distance < closeCombatTolerance)
        {
            if (currentState != AIState.resetAI)
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

            if (closeCombat)
            {
                if (!gotRandom)
                {
                    storeRandom = ReturnRandom();
                    gotRandom = true;
                }

                if (storeRandom < 40)
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

            if (storeRandom < blockRate * 100 && states.canBlock && states.blockHealth != 100)
            {
                states.block = true;
                states.gettingHit = false;
                blocking = true;
            }
        }

        if (blocking)
        {
            blTimer += Time.deltaTime;

            if (blTimer > blockRate)
            {
                states.block = false;
                blocking = false;
                blTimer = 0;
            }
        }
    }

    void NormalState()
    {
        nrmTimer += Time.deltaTime;

        if (nrmTimer > defensiveRate)
        {
            initiateAI = true;
            nrmTimer = 0;
        }
    }

    void CloseState()
    {
        clTimer += Time.deltaTime;

        if (clTimer > aggressiveRate)  // Reduced for quicker close combat actions
        {
            clTimer = 0;
            initiateAI = true;
        }
    }

    void AggressiveState()
    {
        aiTimer += Time.deltaTime;

        if (aiTimer > aggressiveRate)
        {
            initiateAI = true;
            aiTimer = 0;
        }
    }

    void DefensiveState()
    {
        aiTimer += Time.deltaTime;

        if (aiTimer > defensiveRate)
        {
            initiateAI = true;
            aiTimer = 0;
        }
    }

    float ReturnRandom()
    {
        return Random.Range(0, 101);
    }

    IEnumerator OpenAttack(AttackPatterns attackPattern, int index)
    {
        float delay = attackPattern.attacks[index].delay;
        states.attack1 = attackPattern.attacks[index].attack1;
        states.attack2 = attackPattern.attacks[index].attack2;
        yield return new WaitForSeconds(delay);

        states.attack1 = false;
        states.attack2 = false;

        if (index < attackPattern.attacks.Length - 1)
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
        public bool attack3;
        public float delay;
    }
}
