using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{

    public AudioClip clip;
    [HideInInspector] public AudioSource source;

    public string audioName;
    [Range(0, 1)] public float volume = 1f;
    [Range(0, 1)] public float pitch = 1f;
    public bool loop;
    [Range(0, 1)] public float spetialBlend = 0f;

}
