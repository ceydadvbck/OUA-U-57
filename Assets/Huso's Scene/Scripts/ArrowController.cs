using System.Collections;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [Header ("ARROW MOVEMENT")]
    public float speed;
    Rigidbody rb;

    [Space(10)]

    [Header ("ICE ARROW")]
    [Range(0,10f)] public float iceDomainRadius;
    [SerializeField] private Material iceMaterial;
    [SerializeField] private GameObject iceImpactParticle;
    [SerializeField] private GameObject iceCrackParticle;
    bool ice›mpactActive;



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

        if (other.gameObject.CompareTag("Ground") && gameObject.CompareTag("IceArrow") && iceImpactParticle != null && !ice›mpactActive)
        {
            StartCoroutine(ArrowEffectControl());
            GameObject clone›ceImpact = Instantiate(iceImpactParticle, gameObject.transform);

            Vector3 contactPoint = other.GetContact(0).point;
            Instantiate(iceCrackParticle, contactPoint, Quaternion.identity);
          
            Destroy(clone›ceImpact, 1.2f);
        }

        if (other.gameObject.CompareTag("Skeleton") && gameObject.CompareTag("IceArrow") && iceMaterial != null)
        {
            Destroy(transform.parent.gameObject);
            rb.isKinematic = true;


            Collider[] hits = Physics.OverlapSphere(other.GetContact(0).point, 2f);
            foreach (Collider hit in hits)
            {
                if (hit.gameObject == this.gameObject) continue;

                if (hit.gameObject.CompareTag("Skeleton"))
                {
                    SkeletonBody skeletonBody = hit.gameObject.GetComponent<SkeletonBody>();
                    if (skeletonBody != null)
                    {
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
