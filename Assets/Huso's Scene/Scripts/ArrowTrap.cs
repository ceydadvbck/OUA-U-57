using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrap : MonoBehaviour
{

    public float hiz = 5f; // Hareket hýzý
    public float mesafe = 10f; // Ýlerlemesi gereken mesafe
    private Vector3 baslangicPozisyonu; // Baþlangýç konumu
    private float ilerleme;

    void Start()
    {
        baslangicPozisyonu = transform.position;
    }

    void Update()
    {
        float hareketMiktari = hiz * Time.deltaTime;
        transform.Translate(Vector3.down * hareketMiktari); //Z ekseninde hareket için down kullandým.
        ilerleme += hareketMiktari;
        if (ilerleme >= mesafe)
        {
            transform.position = baslangicPozisyonu;
            ilerleme = 0f;
        }
    }
}
