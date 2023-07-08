using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrap : MonoBehaviour
{

    public float hiz = 5f; // Hareket h�z�
    public float mesafe = 10f; // �lerlemesi gereken mesafe
    private Vector3 baslangicPozisyonu; // Ba�lang�� konumu
    private float ilerleme;

    void Start()
    {
        baslangicPozisyonu = transform.position;
    }

    void Update()
    {
        float hareketMiktari = hiz * Time.deltaTime;
        transform.Translate(Vector3.down * hareketMiktari); //Z ekseninde hareket i�in down kulland�m.
        ilerleme += hareketMiktari;
        if (ilerleme >= mesafe)
        {
            transform.position = baslangicPozisyonu;
            ilerleme = 0f;
        }
    }
}
