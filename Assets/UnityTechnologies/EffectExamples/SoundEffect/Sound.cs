using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    public Transform playerTransform; // Oyuncu karakterinin Transform bileþeni

    private AudioSource audioSource;
    private float maxDistance; // Sesin kesileceði maksimum mesafe

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        maxDistance = audioSource.maxDistance; // AudioSource'deki maxDistance deðerini al
    }

    void Update()
    {
        // Oyuncu karakteri ile obje arasýndaki mesafeyi hesapla
        float distance = Vector3.Distance(playerTransform.position, transform.position);

        // Sesin hacmini ayarla
        if (distance > maxDistance)
        {
            audioSource.volume = 0f; // Uzaklaþýldýðýnda sesi kapat
        }
        else
        {
            // Uzaklýk ile birlikte sesin hacmini ayarla
            float volume = 1f - (distance / maxDistance);
            audioSource.volume = volume;
        }
    }
}
