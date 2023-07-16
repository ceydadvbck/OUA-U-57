using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CheckColliderBossArena : MonoBehaviour
{
    List<GameObject> players = new List<GameObject>();
    int i; //Sayac de�i�keni i�in.
    bool bossIsActive = false; //B�t�n oyuncular collider i�ine girerse true olacak ve boss harekete ge�ecek.
    private void Start()
    {
        //Oyuncular�n say�s�n� alabilmek i�in listeye ekledim
        players.AddRange(GameObject.FindGameObjectsWithTag("Player"));
    }
    private void Update()
    {
        if (i == players.Count) //Collider i�indeki oyuncu say�s� ile eleman say�lar� e�it ise
        {
            bossIsActive = true;
        }
        if (bossIsActive)
        {
            //Boss Harekete ge�mesi i�in gerekli methot yaz�lacak.
            //Kap� kapanacak.
        }
        Debug.Log(i);
    }
    //Hem giri�i hem ��k��� kontrol ettim ayn�u anda ikiside i�inde oldugunda boss harekete ge�ecek ve kap� kapanacak
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            i++;
    }
    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            i--;
        }
    }
}
