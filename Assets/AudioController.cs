using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController instance;
    public AudioClip attackSound;
    public AudioClip ultimate;

    public AudioClip getHit;
    public AudioClip blocked;
    public bool gettingHit;

    public AudioSource punchPlayer;
    public AudioSource voicePlayer;

    public StateManager states;

    private void Start()
    {
        states = GetComponentInParent<StateManager>();
    }

    private void FixedUpdate()
    {
        if (states.attack1 || states.attack2 || states.attack3)
        {
            if (attackSound != null)
            {
                punchPlayer.Stop();
                punchPlayer.PlayOneShot(attackSound);
            }
        }
        if (states.ultimateAbility)
        {
            if (ultimate != null)
            {
                punchPlayer.Stop();
                punchPlayer.PlayOneShot(ultimate);
            }
        }

        if (states.gettingHit)
        {
            if (getHit != null)
            {
                Debug.Log("Bismillah");
                punchPlayer.PlayOneShot(getHit);
            }
        }
    }

    public void GetHit()
    {
        voicePlayer.PlayOneShot(getHit);
    }

}
