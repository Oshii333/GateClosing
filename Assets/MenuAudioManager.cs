using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MenuAudioManager : MonoBehaviour
{

    public static MenuAudioManager Instance;

    public AudioSource MusicSource, SFXSource;
    public Sound[] MusicSounds, SFXSounds;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(MusicSounds, x => x.name == name);

        MusicSource.clip = s.clip;
        MusicSource.Play();
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(SFXSounds, x => x.name == name);

        SFXSource.PlayOneShot(s.clip);
    }

    public void MusicVolume(float volume)
    {
        MusicSource.volume = volume;
    }

    public void SFXVolume(float volume)
    {
        SFXSource.volume = volume;
    }

    public void StopPlaying() {
        MusicSource.Stop();
    }
}
