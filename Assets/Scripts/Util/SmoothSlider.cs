using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmoothSlider : MonoBehaviour
{
    
    public float blockHealth = 0;
    public float energy = 0;
    public float health = 0;

    public Slider blockSlider;
    public Slider energySlider;
    public Slider healthSlider;
    public Slider delayedHealthSlider;

    StateManager states;

    void Start()
    {
        states = GetComponent<StateManager>();
        if(healthSlider != null ) delayedHealthSlider.value = healthSlider.value;
    }

    void FixedUpdate()
    {
        blockHealth = states.blockHealth;
        energy = states.energy;
        health = states.health;

        float blockTargetValue = blockHealth * 0.01f;
        float energyTargetValue = energy * 0.01f;
        float healthTargetValue = health * 0.01f;

        if (blockSlider != null) blockSlider.value = Mathf.Lerp(blockSlider.value, blockTargetValue, Time.deltaTime * 10);
        if(energySlider != null) energySlider.value = Mathf.Lerp(energySlider.value, energyTargetValue, Time.deltaTime * 10);
        if(healthSlider != null) healthSlider.value = health * 0.01f;

        if(healthSlider != null)
        {
            if (delayedHealthSlider.value != healthSlider.value)
            {
                delayedHealthSlider.value = Mathf.Lerp(delayedHealthSlider.value, healthTargetValue, Time.deltaTime * 1);
            }
        }

        if (blockSlider != null)
        {
            if (blockHealth > 0)
            {
                blockSlider.gameObject.SetActive(true);
            }
            else blockSlider.gameObject.SetActive(false);
        }
        
    }
}

