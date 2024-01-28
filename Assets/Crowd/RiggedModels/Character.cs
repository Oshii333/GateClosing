using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public AudioListSO audioListSO;
    public AudioSource audioSource;
    public MaterialListSO materialListSO;
    public SkinnedMeshRenderer meshRenderer;
    public Animator animator;
    public Transform hips;
    public int characterIndex;
    public int calloutType;

    public bool playAudio;
    int lastRandomAudio;

    public bool randomColor;
    int lastRandomColor;


    public void OnEnable()
    {
        audioSource.spatialize = true;
        audioSource.spatialBlend = 1;
        if (randomColor)
        {
            SetMaterial();
        }
    }

    //public void Update()
    //{
    //    if (playAudio)
    //    {
    //        PlayAudio();
    //        playAudio = false;
    //    }

    //    if (randomColor)
    //    {
    //        SetMaterial();
    //        randomColor = false;
    //    }
    //}


    public void PlayAudio()
    {
        List<AudioClip> filteredClips = audioListSO.audioLists[characterIndex].calloutTypes[calloutType].clipList;
        audioSource.clip = filteredClips[Random.Range(0, filteredClips.Count)];
        audioSource.Play();
    }

    public void SetMaterial()
    {
        List<Material> materialList = materialListSO.materialList;
        meshRenderer.material = materialList[Random.Range(0, materialList.Count)];
    }

}
