using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Slider musicSlider, sfxSlider;

    public void MusicVolume()
    {
        MenuAudioManager.Instance.MusicVolume(musicSlider.value);
    }

    public void SFXVolume()
    {
        MenuAudioManager.Instance.SFXVolume(sfxSlider.value);
    }
}
