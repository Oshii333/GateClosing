using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    [SerializeField] public List<AudioList> audioList;
    [SerializeField] public AudioClip guardAgroSound;
    [SerializeField] public List<AudioClip> tannoyClips;
    [SerializeField] public List<AudioClip> mainTracks;
    [SerializeField] public float speedUpDelay;

    private float timePassed = 0;
    private int currentMusicTrack = 0;
    private AudioSource mainTrackAudioSource;


    [System.Serializable]
    public class AudioList
    {
        public string name;

        public List<CalloutType> calloutTypes;
    }
    public AudioClip GetAudio(int character, int calloutType)
    {
        List<AudioClip> filteredClips = audioList[character].calloutTypes[calloutType].clipList;
        return filteredClips[Random.Range(0, filteredClips.Count - 1)];
    }

    public AudioClip GetTannoy()
    {
        return tannoyClips[Random.Range(0, tannoyClips.Count - 1)];
    }

    private void Start()
    {
        mainTrackAudioSource = gameObject.AddComponent<AudioSource>();
        mainTrackAudioSource.clip = mainTracks[currentMusicTrack];
        mainTrackAudioSource.loop = true;
        mainTrackAudioSource.Play();
    }

    private void Update()
    {
        timePassed += Time.deltaTime;
        Debug.Log(timePassed);
        if (timePassed >= speedUpDelay)
        {
            Debug.Log(timePassed);
            Debug.Log("Next Track");
            currentMusicTrack++;
            if (currentMusicTrack <= audioList.Count - 1)
            {
                mainTrackAudioSource.clip = mainTracks[currentMusicTrack];
                mainTrackAudioSource.loop = true;
                mainTrackAudioSource.Stop();
                mainTrackAudioSource.Play();
                timePassed = 0;
            }

        }
    }
}

[System.Serializable]
public class CalloutType
{
    public List<AudioClip> clipList = new List<AudioClip>();
}

