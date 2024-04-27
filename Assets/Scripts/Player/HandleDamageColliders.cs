using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HandleDamageColliders;

public class HandleDamageColliders : MonoBehaviour
{
    public GameObject[] damageCollidersLeft;
    public GameObject[] damageCollidersRight;
    public DoDamage     doDamage;
    DamageType damageType;

    private bool activated;

    public enum DamageType
    {
        punch,
        kick,
        special,
        ultimate
    }

    public enum DCtype
    {
        bottom,
        up
    }

    StateManager states;

    private void Start()
    {
        states = GetComponent<StateManager>();
        doDamage = GetComponent<DoDamage>();
        CloseCollliders();
    }

    public void OpenCollider(DCtype type, float delay, DamageType damageType)
    {
        if (type == DCtype.bottom)
        {
            StartCoroutine(OpenCollider(damageCollidersRight, 0, delay, damageType));
        }
        else
        {
            StartCoroutine(OpenCollider(damageCollidersRight, 1, delay, damageType));
        }
    }

    public void FixedUpdate()
    {
        if(activated)
        {
            //doDamage.checkOverlap();
        }
    }

    IEnumerator OpenCollider(GameObject[] array, int index, float delay, DamageType damageType)
    {
        yield return new WaitForSeconds(delay);
        array[index].SetActive(true);
        Debug.Log("Opening Collider " + index);
        array[index].GetComponent<DoDamage>().damageType = damageType;
    }

    public void CloseCollliders()
    {

        for(int i = 0; i < damageCollidersLeft.Length; i++)
        {
            damageCollidersLeft[i].SetActive(false);
            damageCollidersRight[i].SetActive(false);
        }
    }
}
