using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public CharacterManager characterManager;
    public float delay;
    public AudioSource bgM;
    public Slider masterVolume;

    public AudioSource sfxSource1;
    public AudioSource sfxSource2;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        characterManager = CharacterManager.GetInstance();
        PlayBg();

    }
    public static AudioManager GetInstance()
    {
        return instance;
    }

    public void GetAudioSource()
    {
    }
    public void PlayBg()
    {
        bgM.time = delay;
        bgM.Play();
        //bgmSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        //sfxSource.PlayOneShot(clip);
    }

    public void ChangeMasterVolume()
    {
        bgM.volume = masterVolume.value;
    }

    public void SetSFXVolume(float volume)
    {
        //sfxSource.volume = volume;
    }
}
