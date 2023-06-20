using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [SerializeField] Vector3 arrowTarget;
    public float speed;
    Rigidbody rb;

    [SerializeField] private Material iceMaterial;
    [SerializeField] private GameObject iceImpactParticle;
    [SerializeField] private GameObject iceCrackParticle;
    bool ice›mpactActive;
    private SkeletonBody skeletonBody;


    public void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("ArrowTarget")) other.gameObject.GetComponent<FixedJoint>().breakForce = 0;

        if (other.gameObject.CompareTag("Skeleton") && gameObject.CompareTag("Arrow"))
        {
            Destroy(transform.parent.gameObject);
            SkeletonBody skeletonBody = other.gameObject.GetComponentInParent<SkeletonBody>();
            skeletonBody.GetHit();
            rb.isKinematic = true;
        }

        if (gameObject.CompareTag("IceArrow") && iceImpactParticle != null && !ice›mpactActive)
        {
            StartCoroutine(ArrowEffectControl());
            GameObject clone›ceImpact = Instantiate(iceImpactParticle, gameObject.transform);

            Vector3 contactPoint = other.GetContact(0).point;
            Instantiate(iceCrackParticle, contactPoint, Quaternion.identity);
          
            Destroy(clone›ceImpact, 1.2f);
        }

        if (other.gameObject.CompareTag("Skeleton") && gameObject.CompareTag("IceArrow") && iceMaterial != null)
        {
            Destroy(transform.parent.gameObject, 1f);
            rb.isKinematic = true;

            skeletonBody = other.transform.gameObject.GetComponentInParent<SkeletonBody>();

            for (int i = 0; i < skeletonBody.transform.childCount; i++)
            {
                Transform child = skeletonBody.transform.GetChild(i);
                SkinnedMeshRenderer skinnedMeshRenderer = child.GetComponentInChildren<SkinnedMeshRenderer>();
                if (skinnedMeshRenderer != null)
                {
                    skinnedMeshRenderer.material = iceMaterial;
                }
            }
        }


        else if (other.gameObject.CompareTag("Ground")) rb.isKinematic = true;

        Destroy(transform.parent.gameObject, 3f);
    }


    IEnumerator ArrowEffectControl()
    {
        ice›mpactActive = true;

        yield return new WaitForSeconds(2f);

        ice›mpactActive = false;
    }
}
