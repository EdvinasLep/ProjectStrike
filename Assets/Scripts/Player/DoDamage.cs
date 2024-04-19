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
                        case HandleDamageColliders.DamageType.punch:
                            oState.TakeDamage(10, 5, damageType, states);
                            Debug.Log(damageType.ToString());
                            Debug.Log(oState.ToString());
                            break;
                        case HandleDamageColliders.DamageType.kick:
                            oState.TakeDamage(15, 20, damageType, states);
                            Debug.Log(damageType.ToString());
                            break;
                        case HandleDamageColliders.DamageType.ultimate:
                            oState.TakeDamage(40, 0, damageType, states);
                            Debug.Log(damageType.ToString());
                            break;
                    }
                    
                }
            }
        }
    }
}
