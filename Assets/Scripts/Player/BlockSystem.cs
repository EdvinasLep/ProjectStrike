using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSystem : MonoBehaviour
{
    public int blockHealth;

    public bool block;
    public bool isHit;
    public bool blockOvercharge;

    public float downTime;
    public float cooldown = 0.5f;

    StateManager states;

    void Start()
    {
        states = GetComponent<StateManager>();
        blockHealth = 0;
        block = false;
        isHit = false;
        downTime = 0f;
        blockOvercharge = false;
    }

    private void FixedUpdate()
    {       
        if(blockHealth >= 100)
        {
            blockOvercharge = true;
            states.canBlock = false;
        }

        if(blockOvercharge && blockHealth == 0)
        {
            blockOvercharge = false;
            states.canBlock = true;
        }

        increaseBlockHealth();
        decreaseBlockHealth();


    }

    private void Update()
    {
        states.blockHealth = blockHealth;
        block = states.block;
    }


    private void increaseBlockHealth()
    {
        if (isHit && block)
        {
            blockHealth = Mathf.Min(100, blockHealth + 20);
            isHit = false;
        }
    }
    private void decreaseBlockHealth()
    {
        if (blockHealth > 0 && !isHit && !block)
        {
            downTime += Time.deltaTime;

            if (downTime > cooldown)
            {
                blockHealth = Mathf.Max(0, blockHealth - 10);
                downTime = 0f;
            }
        }
    }
}
