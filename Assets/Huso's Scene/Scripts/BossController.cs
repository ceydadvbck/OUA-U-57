using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    Animator anim;


    [Header("Spawn")]
    public float spawnTime;
    public  Transform[] spawnSkeleton;
    private System.Random rand = new System.Random();
    [HideInInspector] public double _weight;

    [Header("Skill")]
   [SerializeField] private GameObject RedMagicParticle;
   [SerializeField] private GameObject WhiteMagicParticle;
   [SerializeField] private GameObject SlashMagicParticle;



    bool isStandUp;
    [SerializeField] private GameObject skeleton;

    public void Start()
    {
        anim = GetComponent<Animator>();
        SkeletonSpawn();
    }



    public void SkeletonSpawn()
    {
        InvokeRepeating("SpawnRandomEnemy", 0f, spawnTime);
    }


    private void SpawnRandomEnemy()
    {
        for (int i = 0; i < spawnSkeleton.Length; i++)
        {
            Vector3 spawnPoint =  spawnSkeleton[i].transform.position;

            Instantiate(skeleton, spawnPoint, Quaternion.identity);
        }
    }

}



