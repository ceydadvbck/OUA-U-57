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
        Vector3 capsuleCenter = skillAreaCol.bounds.center;

        Vector3 randomOffset = new Vector3(
            Random.Range(-skillAreaCol.bounds.extents.x, skillAreaCol.bounds.extents.x),
            Random.Range(0, 0),
            Random.Range(-skillAreaCol.bounds.extents.z, skillAreaCol.bounds.extents.z)
        );

        Vector3 skillPosition = capsuleCenter + randomOffset;
        
        GameObject redMagicClone = Instantiate(RedMagicParticle, skillPosition, Quaternion.identity, skillAreaCol.transform);
        redMagicClone.transform.position = new Vector3(redMagicClone.transform.position.x, 0f, redMagicClone.transform.position.z);
        Destroy(redMagicClone, 5f);

    }

    void RedMagicAnimationEvent()
    {
        anim.SetTrigger("RedMagic");
    }


    void WhiteMagic() //AnimationEvent
    {
        Vector3 capsuleCenter = skillAreaCol.bounds.center;

        Vector3 randomOffset = new Vector3(
            Random.Range(-skillAreaCol.bounds.extents.x, skillAreaCol.bounds.extents.x),
            Random.Range(-skillAreaCol.bounds.extents.y, skillAreaCol.bounds.extents.y),
            Random.Range(-skillAreaCol.bounds.extents.z, skillAreaCol.bounds.extents.z)
        );

        Vector3 skillPosition = capsuleCenter + randomOffset;

        GameObject whiteMagicClone = Instantiate(WhiteMagicParticle, skillPosition, Quaternion.identity, skillAreaCol.transform);
        whiteMagicClone.transform.position = new Vector3(whiteMagicClone.transform.position.x, 0f, whiteMagicClone.transform.position.z);
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



