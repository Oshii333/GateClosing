using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    [SerializeField] public AudioClip guardAgroSound;
    [SerializeField] public List<AudioClip> tannoyClips;
    [SerializeField] public List<AudioClip> mainTracks;
    [SerializeField] public float speedUpDelay;

    private float timePassed = 0;
    private int currentMusicTrack = 0;
    private AudioSource mainTrackAudioSource;

    public AudioClip GetTannoy()
    {
        return tannoyClips[Random.Range(0, tannoyClips.Count - 1)];
    }

    private void Start()
    {
        mainTrackAudioSource = gameObject.AddComponent<AudioSource>();
        mainTrackAudioSource.clip = mainTracks[currentMusicTrack];
        mainTrackAudioSource.volume = 0.5f;
        mainTrackAudioSource.loop = true;
        mainTrackAudioSource.Play();
    }

    private void Update()
    {
        timePassed += Time.deltaTime;
        if (timePassed >= speedUpDelay)
        {
            currentMusicTrack++;
            if (currentMusicTrack <= mainTracks.Count - 1)
            {
                mainTrackAudioSource.clip = mainTracks[currentMusicTrack];
                mainTrackAudioSource.loop = true;
                mainTrackAudioSource.volume = 0.5f;
                mainTrackAudioSource.Play();
                timePassed = 0;
            }

        }
    }
}


[System.Serializable]
public class AudioList
{
    public string name;

    public List<CalloutType> calloutTypes;
}

[System.Serializable]
public class CalloutType
{
    public List<AudioClip> clipList = new List<AudioClip>();
}

