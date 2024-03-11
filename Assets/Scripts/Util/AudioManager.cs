using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource getHitSource;
    public AudioSource punchSource;
    public AudioClip hitClip;
    public AudioClip punchClip;

    public static AudioManager instance;

    public static AudioManager GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
    }


    public void getHit()
    {
        getHitSource.clip = hitClip;
        getHitSource.Play();
    }

    public void punch()
    {
        punchSource.clip = punchClip;
        punchSource.Play();
    }
}
