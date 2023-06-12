using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Animations.Rigging;
public class RangerAction : MonoBehaviour
{
    Animator animator;
    GameObject mainCam;

    [Header ("Arrow")]
    [SerializeField] GameObject arrow;
    [SerializeField] GameObject arrowInHand;
    [SerializeField] float arrowForce;
    [SerializeField] float attackCoolDown;
    float attackTime;
    [SerializeField] Transform arrowSpawnPoint;
    bool rangerAiming = false;

    [Space(10)]

    [Header ("Constraints")]
    [SerializeField] MultiRotationConstraint bodyAim;
    [SerializeField] MultiAimConstraint handAim;
    [SerializeField] MultiRotationConstraint handRot;

    [Space(10)]

    [Header("Aim")]
    [SerializeField] GameObject lookAt;
    GameObject aimLookAt;
    public float transitionDuration = 0.5f;
    private Vector3 initialOffset;
    private Vector3 targetOffset;
    private float transitionTimer;

    public void Start()
    {
        aimLookAt = GameObject.FindGameObjectWithTag("AimLookAt");
        animator = GetComponent<Animator>();

        bodyAim.weight = 0f;
        mainCam = GameObject.FindGameObjectWithTag("MainCamera");

        initialOffset = bodyAim.data.offset;
    }


    public void Update()
    {
        BodyAimRegulation();

        if (Input.GetMouseButtonDown(1))
        {
            RangerAim();
        }
        if (Input.GetMouseButtonUp(1))
        {
            animator.SetBool("ArrowAim", false);
            animator.SetBool("ArrowAimShoot", false);

            rangerAiming = false;
            bodyAim.weight = 0f;
            arrowInHand.SetActive(false);
        }

        if (rangerAiming)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                animator.SetBool("ArrowAimShoot", true);
            }
        }
    }

    public void RangerAim()
    {
        if (attackTime < Time.time)
        {
            attackTime = 0f;
            attackTime = Time.time + attackCoolDown;

            animator.SetBool("ArrowAim", true);
            rangerAiming = true;

            bodyAim.weight = 1f;
        }
    }

    public void RangerAttack()//AnimationEvent
    {
        animator.SetBool("ArrowAimShoot", false);
        arrowInHand.SetActive(false);

        GameObject arrowClone = GameObject.Instantiate(arrow, arrowSpawnPoint.transform.position, arrowSpawnPoint.transform.rotation) as GameObject;
        Rigidbody arrowRb = arrowClone.GetComponentInChildren<Rigidbody>();
        arrowRb.AddForce(arrowSpawnPoint.transform.forward * arrowForce, ForceMode.Impulse);
    }

    public void ActiveArrow()//AnimationEvent
    {
        arrowInHand.SetActive(true);
    }

    void BodyAimRegulation()
    {
        handAim.data.sourceObjects.Add(new(aimLookAt.transform, 1f));
        lookAt.transform.position = Vector3.Lerp(lookAt.transform.position, aimLookAt.transform.position, 1f);

        float rotX = mainCam.transform.rotation.eulerAngles.x;
        targetOffset = new Vector3(0, 95, rotX);

        transitionTimer = 0f;
        transitionTimer += Time.deltaTime;

        float t = Mathf.Clamp01(transitionTimer / transitionDuration);

        bodyAim.data.offset = Vector3.Lerp(initialOffset, targetOffset, t);
    }
}
