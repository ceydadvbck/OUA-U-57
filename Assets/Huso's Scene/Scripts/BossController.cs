using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    Animator anim;
    [SerializeField] private GameObject skeleton;

    [Space(10)]

    [Header("Spawn")]
    [SerializeField] float skeletonSpawnTime;
    [SerializeField] private float startTime;
    [SerializeField] Transform[] spawnSkeleton;
    private System.Random rand = new System.Random();
    [HideInInspector] public double _weight;
    
    [Space(10)]

    [Header("Skill")]
    [SerializeField] private CapsuleCollider skillAreaCol;
    private Vector3 skillArea;
    [SerializeField] private GameObject RedMagicParticle;
    [Range(0f, 50f)]
    [SerializeField] float redMagicTime;
    [SerializeField] private GameObject WhiteMagicParticle;
    [Range(0f, 50f)]
    [SerializeField] float whiteMagicTime;
    [SerializeField] private GameObject SlashMagicParticle;
    [Range(0f, 50f)]
    [SerializeField] float slashMagicTime;
    GameObject slashMagicClone;


    public void Start()
    {
        anim = GetComponent<Animator>();
        InvokeFunctionControl();
    }


    public void Update()
    {
        if (slashMagicClone != null)
        {
            slashMagicClone.transform.Translate(-Vector3.right * 15f * Time.deltaTime);
        }
    }

    public void InvokeFunctionControl()
    {
        InvokeRepeating("SkeletonSpawnMagicAnimationEvent", startTime, skeletonSpawnTime);
        InvokeRepeating("RedMagicAnimationEvent", startTime, redMagicTime);
        InvokeRepeating("WhiteMagicAnimationEvent", startTime, whiteMagicTime);
        InvokeRepeating("SlashMagicAnimationEvent", startTime, slashMagicTime);
    }


    private void SkeletonSpawnMagic() //AnimatonEvent
    {
        for (int i = 0; i < spawnSkeleton.Length; i++)
        {
            Vector3 spawnPoint = spawnSkeleton[i].transform.position;

            Instantiate(skeleton, spawnPoint, Quaternion.identity);
        }
    }

    private void SkeletonSpawnMagicAnimationEvent()
    {
        anim.SetTrigger("SkeletonSpawnMagic");
    }


    void RedMagic() //AnimationEvent
    {
        skillArea = new Vector3(
        Random.Range(skillAreaCol.bounds.min.x, skillAreaCol.bounds.max.x),
        Random.Range(skillAreaCol.bounds.min.y, skillAreaCol.bounds.max.y),
        Random.Range(skillAreaCol.bounds.min.z, skillAreaCol.bounds.max.z));

        skillArea.y = 0f;
        GameObject redMagicClone = Instantiate(RedMagicParticle, skillArea, Quaternion.identity, skillAreaCol.transform);
        Destroy(redMagicClone, 5f);

    }

    void RedMagicAnimationEvent()
    {
        anim.SetTrigger("RedMagic");
    }


    void WhiteMagic() //AnimationEvent
    {
        skillArea = new Vector3(
        Random.Range(skillAreaCol.bounds.min.x, skillAreaCol.bounds.max.x),
        Random.Range(skillAreaCol.bounds.min.y, skillAreaCol.bounds.max.y),
        Random.Range(skillAreaCol.bounds.min.z, skillAreaCol.bounds.max.z));

        skillArea.y = 0f;
        GameObject whiteMagicClone = Instantiate(WhiteMagicParticle, skillArea, Quaternion.identity, skillAreaCol.transform);
        Destroy(whiteMagicClone, 5f);
    }
    void WhiteMagicAnimationEvent()
    {
        anim.SetTrigger("WhiteMagic");
    }



    void SlashMagic() //AnimationEvent
    {
        slashMagicClone = Instantiate(SlashMagicParticle, new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z), transform.rotation, skillAreaCol.transform);
        slashMagicClone.transform.rotation = Quaternion.Euler(0, Random.Range(45, 135), 0);
        Destroy(slashMagicClone, 2f);
    }

    void SlashMagicAnimationEvent()
    {
        anim.SetTrigger("SlashMagic");
    }
}



