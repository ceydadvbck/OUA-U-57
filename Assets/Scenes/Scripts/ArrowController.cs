using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [SerializeField] Vector3 arrowTarget;
    public float speed;
    [HideInInspector] Rigidbody rb;
 
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


        if (other.gameObject.CompareTag("Skeleton"))
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
