using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBody : MonoBehaviour
{
    [SerializeField] SkeletonBody[] childLimbs;

    [SerializeField] GameObject limbPrefab;
    [SerializeField] private GameObject skeletonFireParticle;

    [SerializeField] AudioClip acBone;
    [SerializeField] AudioClip acFreeze;

    AudioSource audioSource;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void GetHit()
    {
        if (limbPrefab != null)
        {
            GameObject limbClone = Instantiate(limbPrefab, transform.position, transform.rotation);
            Destroy(limbClone, 5f);
            limbClone.GetComponent<AudioSource>().PlayOneShot(acBone);
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

    public void FreezeSound()
    {
        audioSource.PlayOneShot(acFreeze);
    }
}
