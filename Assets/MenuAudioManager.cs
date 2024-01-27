using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MenuAudioManager : MonoBehaviour
{
    public AudioSource AudioSource;

    private float musicVolume = 1f;

    void Start()
    {
        AudioSource.Play();
    }
    void Update()
    {
        AudioSource.volume = musicVolume;
    }

    public void updateVolume(float volume)
    {
        musicVolume = volume;
        PlayerPrefs.SetFloat("TrackVolume", volume);
    }

}
