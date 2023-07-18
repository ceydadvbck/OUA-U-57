using System.Collections.Generic;
using UnityEngine;

public class CheckColliderBossArena : MonoBehaviour
{
    List<GameObject> players = new List<GameObject>();
    int i;
    bool bossIsActive = false;
    bool startFight;

    [SerializeField] GameObject boss;
    [SerializeField] GameObject arenaPlayerCheck;

    AudioSource audioSource;
    [SerializeField] AudioClip acBossGettingUp;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
      
        players.AddRange(GameObject.FindGameObjectsWithTag("Player"));
    }
    private void Update()
    {
        if (i == 2) //Collider içindeki oyuncu sayýsý ile eleman sayýlarý eþit ise
        {
            bossIsActive = true;
        }
        if (!bossIsActive && i >0 && !startFight)
        {
            startFight = true;
            arenaPlayerCheck.SetActive(true);
            boss.GetComponent<Animator>().SetTrigger("BossStandUp");
            audioSource.PlayOneShot(acBossGettingUp);

        }

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
