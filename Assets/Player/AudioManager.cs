using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    [SerializeField] public List<AudioList> audioList;
    [SerializeField] public List<AudioClip> tannoyClips;
    [System.Serializable]
    public class AudioList
    {
        public string name;

        public List<CalloutType> calloutTypes;
    }
    public AudioClip GetAudio(int character, int calloutType, int index)
    {
        return audioList[character].calloutTypes[calloutType].clipList[index];
    }

    public AudioClip GetTannoy()
    {
        return tannoyClips[Random.Range(0, tannoyClips.Count - 1)];
    }
}

[System.Serializable]
public class CalloutType
{
    public List<AudioClip> clipList = new List<AudioClip>();
}

