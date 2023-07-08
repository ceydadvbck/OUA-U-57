using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeController : MonoBehaviour
{
    BarbarianAction axeAction;
    Rigidbody rbAxe;
    [SerializeField] Animator barbarianAnim;
    public bool activated;
    [SerializeField] float rSpeed;
    public void Start()
    {
        axeAction =GetComponentInParent<BarbarianAction>();
        rbAxe = GetComponent<Rigidbody>();
    }

    public void Update()
    {
        if (activated)
        {
            transform.Rotate(0f, 0f, -15f * rSpeed *Time.deltaTime);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        activated = false;
        if (collision.gameObject.CompareTag("Skeleton"))
        {
            SkeletonBody skeletonBody = collision.gameObject.GetComponentInParent<SkeletonBody>();
            skeletonBody.GetHit();
            barbarianAnim.SetBool("AxeHolding", true);
            axeAction.ReturnAxe();
        }
        else
        {
            rbAxe.isKinematic = true;
        }
    }
}
