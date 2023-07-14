using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBody : MonoBehaviour
{
    [SerializeField] SkeletonBody[] childLimbs;

    [SerializeField] GameObject limbPrefab;
    [SerializeField] private GameObject skeletonFireParticle;
    public void GetHit()
    {
        if (limbPrefab != null)
        {
            GameObject limbClone = Instantiate(limbPrefab, transform.position, transform.rotation);
            Destroy(limbClone, 5f);
        }

        transform.localScale = Vector3.zero;

        Destroy(gameObject);

    }
    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.transform.CompareTag("GroundAttackDamageBox"))
        {
            GetHit();

        }
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.transform.CompareTag("TripleArrow"))
        {
            GameObject skeletonFireParticleClone = Instantiate(skeletonFireParticle, gameObject.transform.position, transform.rotation, gameObject.transform);

            Destroy(skeletonFireParticleClone, 3f);
            Invoke("GetHit", 2f);

        }
    }
}
