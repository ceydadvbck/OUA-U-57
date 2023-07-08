using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    public Transform playerTransform; // Oyuncu karakterinin Transform bile�eni

    private AudioSource audioSource;
    private float maxDistance; // Sesin kesilece�i maksimum mesafe

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        maxDistance = audioSource.maxDistance; // AudioSource'deki maxDistance de�erini al
    }

    void Update()
    {
        // Oyuncu karakteri ile obje aras�ndaki mesafeyi hesapla
        float distance = Vector3.Distance(playerTransform.position, transform.position);

        // Sesin hacmini ayarla
        if (distance > maxDistance)
        {
            audioSource.volume = 0f; // Uzakla��ld���nda sesi kapat
        }
        else
        {
            // Uzakl�k ile birlikte sesin hacmini ayarla
            float volume = 1f - (distance / maxDistance);
            audioSource.volume = volume;
        }
    }
}
