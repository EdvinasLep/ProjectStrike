using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoDamage : MonoBehaviour
{
    StateManager states;

    public HandleDamageColliders.DamageType damageType;

    void Start()
    {
        states = GetComponentInParent<StateManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponentInParent<StateManager>())
        {
            StateManager oState = collision.GetComponentInParent<StateManager>();
            if(oState != states)
            {
                if(!oState.currentlyAttacking)
                {
                    switch (damageType)
                    {
                        case HandleDamageColliders.DamageType.light:
                            oState.TakeDamage(10, damageType);
                            break;
                        case HandleDamageColliders.DamageType.heavy:
                            oState.TakeDamage(15, damageType);
                            break;
                    }
                    
                }
            }
        }
    }
}
