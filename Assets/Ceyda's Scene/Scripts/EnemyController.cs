using UnityEngine;
using UnityEngine.AI;
using Mirror;
public class EnemyController : NetworkBehaviour
{   
    [SyncVar]
    public float lookRadius = 10f;  // Detection range for player
    Animator animator;
    UnityEngine.AI.NavMeshAgent agent; // Reference to the NavMeshAgent
    CharacterCombat combat;
        

    [HideInInspector] public GameObject[] target;

    bool isRunning;
    bool isAttacking;

    void Start()
    {
        animator = GetComponent<Animator>();
        

        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        combat = GetComponent<CharacterCombat>();

        target = GameObject.FindGameObjectsWithTag("Player");
    }


    void Update()
    {

        if (target != null && target.Length > 0)
        {
            GameObject closestTarget = null;
            float closestDistance = Mathf.Infinity;

            foreach (var currentTarget in target)
            {
                if (currentTarget != null)
                {
                    float distance = Vector3.Distance(currentTarget.transform.position, transform.position);

                    if (distance < closestDistance)
                    {
                       

                        closestTarget = currentTarget;
                        closestDistance = distance;
                        
                        animator.Play("Zombie Run");

                    }
                }
            }

            if (closestTarget != null)
            {
                float distanceToClosest = Vector3.Distance(closestTarget.transform.position, transform.position);

                if (distanceToClosest <= lookRadius)
                {

                    agent.SetDestination(closestTarget.transform.position);

                    

                    if (distanceToClosest <= agent.stoppingDistance)
                    {
                        CharacterHealth targetStats = closestTarget.GetComponent<CharacterHealth>();
                        if (targetStats != null)
                        {
                            // combat.Attack(targetStats);
                        }

                        FaceTarget();
                    }
                }
            }
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
        if (!Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, lookRadius);
        }
    }
}