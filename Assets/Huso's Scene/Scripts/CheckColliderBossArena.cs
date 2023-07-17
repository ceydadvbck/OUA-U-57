using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CheckColliderBossArena : MonoBehaviour
{
    List<GameObject> players = new List<GameObject>();
    int i;
    bool bossIsActive = false;

    [SerializeField] GameObject boss;
    [SerializeField] GameObject arenaPlayerCheck;

    private void Start()
    {
        //Oyuncular�n say�s�n� alabilmek i�in listeye ekledim
        players.AddRange(GameObject.FindGameObjectsWithTag("Player"));
    }
    private void Update()
    {
        if (i == 2) //Collider i�indeki oyuncu say�s� ile eleman say�lar� e�it ise
        {
            bossIsActive = true;
        }
        if (bossIsActive && i >0)
        {
            arenaPlayerCheck.SetActive(true);
            boss.GetComponent<Animator>().SetTrigger("BossStandUp");
        }
        
    }
    //Hem giri�i hem ��k��� kontrol ettim ayn�u anda ikiside i�inde oldugunda boss harekete ge�ecek ve kap� kapanacak
    private void OnTriggerStay(Collider other)
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
