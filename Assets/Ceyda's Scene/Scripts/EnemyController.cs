using UnityEngine;
using UnityEngine.AI;
using Mirror;
using System.Collections.Generic;
using System.Collections;

public class EnemyController : NetworkBehaviour
{   
    [SyncVar]
    public float lookRadius = 10f; 
    Animator animator;
    UnityEngine.AI.NavMeshAgent agent;

    AudioSource audioSource;
    [SerializeField] AudioClip acSkeleton;
    bool skeletonSoulSound;

    GameObject closestTarget = null;
    [HideInInspector] public GameObject[] target;

    public int damage;
    float distanceToClosest;
    bool chase;

    void Start()
    {
        chase = true;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        target = GameObject.FindGameObjectsWithTag("Player");
        agent.enabled = false;
    }


    void Update()
    {
        if (target != null && target.Length > 0)
        {
            foreach (var currentTarget in target)
            {
                if (currentTarget != null)
                {
                    float distance = Vector3.Distance(currentTarget.transform.position, transform.position);

                    if (distance < lookRadius)
                    {
                        closestTarget = currentTarget;

                        animator.SetTrigger("GettingUp");

                        if (!skeletonSoulSound)
                        {
                            audioSource.PlayOneShot(acSkeleton);
                            skeletonSoulSound = true;
                        }

                    }
                }
            }

            if (closestTarget != null)
            {
                distanceToClosest = Vector3.Distance(closestTarget.transform.position, transform.position);

                if (distanceToClosest <= lookRadius)
                {
                    if (distanceToClosest <= agent.stoppingDistance)
                    {
                        CharacterHealth targetStats = closestTarget.GetComponent<CharacterHealth>();
                        if (targetStats != null )
                        {
                            animator.SetBool("Attack", true);
                            animator.SetBool("Chase", false);
                           
                        }

                        FaceTarget();
                    }
                    else if(!chase)
                    {
                        ChaseControl();
                    }

                }
            }
        }
    }

    void ChaseControl()
    {
        agent.SetDestination(closestTarget.transform.position);
       
        animator.SetBool("Chase", true);
        animator.SetBool("Attack", false);
       
        agent.enabled = true;
        chase = false;
    }

    void Damage() //AnimationEvent
    {
        if (distanceToClosest <= agent.stoppingDistance)
        {
            closestTarget.transform.gameObject.GetComponent<CharacterHealth>().TakeDamage(damage);
        }
    }


    void FixedUpdate()
    {
        if (isServer)
        {
            SyncEnemyPositionAndRotation();
        }
    }

    [Command]
    void SyncEnemyPositionAndRotation()
    {
        // Pozisyon ve rotasyon bilgilerini diger oyunculara gondermek icin ClientRpc kullan
        RpcSyncEnemyPositionAndRotation(transform.position, transform.rotation);
    }

    [ClientRpc]
    void RpcSyncEnemyPositionAndRotation(Vector3 position, Quaternion rotation)
    {
        agent.Warp(position);
        transform.rotation = rotation;
    }
    [Server]
    void FaceTarget()
    {
        foreach (var currentTarget in target)
        {
            if (currentTarget != null)
            {
                Vector3 direction = (currentTarget.transform.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }
        }
    }

    [Server]
    void OnDrawGizmosSelected()
    {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}