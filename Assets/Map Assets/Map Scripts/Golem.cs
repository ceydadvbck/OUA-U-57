using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;
using Mirror;

public class Golem : NetworkBehaviour
{
    NavMeshAgent agent;//Golem
    private Animator anim;

    [HideInInspector] public GameObject[] target;
    private Vector3 startPosition;
    private Vector3 distance;
    public float CanSee;

    GameObject closestTarget = null;
    [SyncVar]
    public float lookRadius = 10f;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        startPosition = transform.position;

        target = GameObject.FindGameObjectsWithTag("Player");
    }

    private void Update()
    {
        //MoveToTarget();
        //CheckDistanceToTarget();



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
                    }
                }
            }

            if (closestTarget != null)
            {
                float distanceToClosest = Vector3.Distance(closestTarget.transform.position, transform.position);

                if (distanceToClosest <= lookRadius)
                {
                    if (distanceToClosest <= agent.stoppingDistance)
                    {
                        CharacterHealth targetStats = closestTarget.GetComponent<CharacterHealth>();
                        if (targetStats != null)
                        {
                            anim.SetBool("IsAttacking", true);
                            anim.SetBool("Walk", false);
                        }

                        FaceTarget();
                    }
                    else
                    {
                        anim.SetBool("IsAttacking", false);
                        anim.SetBool("Walk", true);
                        agent.SetDestination(closestTarget.transform.position);
                    }


                }
            }
        }
    }

    //private void MoveToTarget()
    //{
    //    agent.SetDestination(closestTarget.transform.position);
    //    if (agent.velocity.magnitude > 0)
    //    {
    //        anim.SetBool("Walk", true);
    //    }
    //    else
    //    {
    //        anim.SetBool("Walk", false);
    //    }
    //}
    //
    //private void CheckDistanceToTarget()
    //{
    //    distance = agent.transform.position - target.transform.position;
    //    if (distance.magnitude > CanSee)
    //    {
    //        agent.stoppingDistance = 0;
    //        agent.SetDestination(startPosition);
    //        if (agent.remainingDistance <= agent.stoppingDistance)
    //        {
    //            Quaternion targetRotation = Quaternion.Euler(0, 90, 0);
    //            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime);
    //        }
    //
    //    }
    //    else
    //    {
    //        agent.stoppingDistance = 3;
    //    }
    //
    //}

    //private void PlayAttackAnimation()
    //{
    //    if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && distance.magnitude <= CanSee)
    //    {
    //        anim.SetBool("IsAttacking", true);
    //    }
    //    else
    //    {
    //        anim.SetBool("IsAttacking", false);
    //    }
   
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
