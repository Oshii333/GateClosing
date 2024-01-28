using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioList", menuName = "ScriptableObjects/AudioList")]
public class AudioListSO : ScriptableObject
{
    public List<AudioList> audioLists;
}
