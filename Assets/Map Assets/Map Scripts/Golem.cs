using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;

public class Golem : MonoBehaviour
{
    NavMeshAgent agent;//Golem
    private Animator anim;

    public GameObject target;
    private Vector3 startPosition;
    private Vector3 distance;
    public float CanSee;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        startPosition = transform.position;
        anim.SetBool("Walk", false);
    }

    private void Update()
    {
        MoveToTarget();
        CheckDistanceToTarget();
        PlayAttackAnimation();
    }

    private void MoveToTarget()
    {
        agent.SetDestination(target.transform.position);
        if (agent.velocity.magnitude > 0)
        {
            anim.SetBool("Walk", true);
        }
        else
        {
            anim.SetBool("Walk", false);
        }
    }

    private void CheckDistanceToTarget()
    {
        distance = agent.transform.position - target.transform.position;
        if (distance.magnitude > CanSee)
        {
            agent.stoppingDistance = 0;
            agent.SetDestination(startPosition);
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                Quaternion targetRotation = Quaternion.Euler(0, 90, 0);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime);
            }

        }
        else
        {
            agent.stoppingDistance = 3;
        }

    }

    private void PlayAttackAnimation()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && distance.magnitude <= CanSee)
        {
            anim.SetBool("IsAttacking", true);
        }
        else
        {
            anim.SetBool("IsAttacking", false);
        }
    }
}
