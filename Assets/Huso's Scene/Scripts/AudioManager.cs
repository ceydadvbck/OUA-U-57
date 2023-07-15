using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager Instance;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject); //don't destroy it between levels
    }

    public void Start()
    {
       
        foreach (var sound in sounds)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = sound.clip;
            source.volume = sound.volume;
            source.pitch = sound.pitch;
            source.playOnAwake = false;
            source.loop = sound.loop;
            source.spatialBlend = sound.spetialBlend;
            sound.source = source;
        }
        Instance.Play("SoundTrack");

    }

    public void Play(string audioName)
    {
        Sound s = Array.Find(sounds, sound => sound.audioName == audioName);

        if (s != null && s.source != null) s.source.Play();

        else Debug.LogWarning("Ses bulunamadý veya AudioSource null.");

    }

    public void Stop(string audioName)
    {
        Sound s = Array.Find(sounds, sound => sound.audioName == audioName);

        s.source.Stop();
    }
}
