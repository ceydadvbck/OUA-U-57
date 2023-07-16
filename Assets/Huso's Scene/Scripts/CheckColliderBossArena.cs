using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CheckColliderBossArena : MonoBehaviour
{
    List<GameObject> players = new List<GameObject>();
    int i; //Sayac deðiþkeni için.
    bool bossIsActive = false; //Bütün oyuncular collider içine girerse true olacak ve boss harekete geçecek.
    private void Start()
    {
        //Oyuncularýn sayýsýný alabilmek için listeye ekledim
        players.AddRange(GameObject.FindGameObjectsWithTag("Player"));
    }
    private void Update()
    {
        if (i == players.Count) //Collider içindeki oyuncu sayýsý ile eleman sayýlarý eþit ise
        {
            bossIsActive = true;
        }
        if (bossIsActive)
        {
            //Boss Harekete geçmesi için gerekli methot yazýlacak.
            //Kapý kapanacak.
        }
        Debug.Log(i);
    }
    //Hem giriþi hem çýkýþý kontrol ettim aynýu anda ikiside içinde oldugunda boss harekete geçecek ve kapý kapanacak
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
