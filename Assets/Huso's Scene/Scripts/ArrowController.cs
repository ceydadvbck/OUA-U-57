using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [SerializeField] Vector3 arrowTarget;
    public float speed;
    [HideInInspector] Rigidbody rb;
    [SerializeField] Material iceMaterial;
    bool iceDelayedFinish = false;

    private SkeletonBody skeletonBody;
    public void Start()
    {
        rb = GetComponent<Rigidbody>();

    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("ArrowTarget"))
        {
            other.gameObject.GetComponent<FixedJoint>().breakForce = 0;
        }
        if (other.gameObject.CompareTag("Skeleton") && gameObject.CompareTag("IceArrow") && iceMaterial != null)
        {
            Destroy(transform.parent.gameObject);
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

        if (other.gameObject.CompareTag("Skeleton") && gameObject.CompareTag("Arrow"))
        {
            Destroy(transform.parent.gameObject);
            SkeletonBody skeletonBody = other.gameObject.GetComponentInParent<SkeletonBody>();
            skeletonBody.GetHit();
            rb.isKinematic = true;
        }
        else if (other.gameObject.CompareTag("Ground"))
        {
            rb.isKinematic = true;
        }
        Destroy(transform.parent.gameObject, 3f);
    }
}
