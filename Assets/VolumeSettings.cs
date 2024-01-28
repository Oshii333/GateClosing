using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider, sfxSlider;


    private void Start()
    {
        if (PlayerPrefs.HasKey("musicVolume")) {
            LoadMusicVolume();
        }
        else
        {
            SetMusicVolume();
        }
        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            LoadSFXVolume();
        }
        else
        {
            SetSFXVolume();
        }
    }
    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        audioMixer.SetFloat("Music", volume);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    public void SetSFXVolume()
    {
        float volume = sfxSlider.value;
        audioMixer.SetFloat("SFX", volume);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    private void LoadMusicVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }

    private void LoadSFXVolume()
    {
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
    }
}
