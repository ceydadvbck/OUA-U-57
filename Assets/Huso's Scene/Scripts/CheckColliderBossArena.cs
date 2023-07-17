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
        //Oyuncularýn sayýsýný alabilmek için listeye ekledim
        players.AddRange(GameObject.FindGameObjectsWithTag("Player"));
    }
    private void Update()
    {
        if (i == 2) //Collider içindeki oyuncu sayýsý ile eleman sayýlarý eþit ise
        {
            bossIsActive = true;
        }
        if (bossIsActive && i >0)
        {
            arenaPlayerCheck.SetActive(true);
            boss.GetComponent<Animator>().SetTrigger("BossStandUp");
        }
        
    }
    //Hem giriþi hem çýkýþý kontrol ettim aynýu anda ikiside içinde oldugunda boss harekete geçecek ve kapý kapanacak
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
