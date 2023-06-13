using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Animations.Rigging;
public class RangerAction : MonoBehaviour
{
    Animator animator;
    GameObject mainCam;

    [Header("Arrow")]
    [SerializeField] GameObject arrow;
    [SerializeField] GameObject iceArrow;
    [SerializeField] GameObject tripleArrow;
    [SerializeField] GameObject[] arrowInHand;


    [SerializeField] float arrowForce;
    [SerializeField] float attackCoolDown;
    float attackTime;
    [SerializeField] Transform[] arrowSpawnPoint;
    [SerializeField] bool rangerAiming = false;

    [Space(10)]

    [Header("Constraints")]
    [SerializeField] MultiRotationConstraint bodyAim;
    [SerializeField] MultiAimConstraint handAim;
    [SerializeField] MultiRotationConstraint handRot;
    [SerializeField] TwoBoneIKConstraint rightHand;
    [SerializeField] TwoBoneIKConstraint leftHand;

    [Space(10)]

    [Header("Aim")]
    [SerializeField] GameObject lookAt;
    GameObject aimLookAt;
    public float transitionDuration = 0.5f;
    private Vector3 initialOffset;
    private Vector3 targetOffset;
    private float transitionTimer;
    private bool classicArrowShoot;


    public void Start()
    {
        aimLookAt = GameObject.FindGameObjectWithTag("AimLookAt");
        animator = GetComponent<Animator>();

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
            animator.SetBool("ArrowTripleAimShoot", false);

            rangerAiming = false;
        }

        if (rangerAiming)
        {
            StartCoroutine(IsTripleArrowAttack());

            if (Input.GetButtonDown("Fire1"))
            {
                animator.SetBool("ArrowAimShoot", true);
                classicArrowShoot = true;
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                animator.SetBool("ArrowAimShoot", true);
                classicArrowShoot = false;
            }
        }
        else
        {
            if (bodyAim.weight >0) bodyAim.weight -= 4f * Time.deltaTime;

            rightHand.weight = 0f;
            leftHand.weight = 0f;
            TripleArrowIsActive(false);
        }
    }

    void BodyAimRegulation() //Body facing the target
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

    public void RangerAttack(GameObject arrowPrefab)//ClassicAttack- AnimationEvent
    {

        if (!classicArrowShoot) arrowPrefab = iceArrow;

        else arrowPrefab = arrow;

        animator.SetBool("ArrowAimShoot", false);
        arrowInHand[0].SetActive(false);

        GameObject arrowClone = GameObject.Instantiate(arrowPrefab, arrowSpawnPoint[0].transform.position, arrowSpawnPoint[0].transform.rotation) as GameObject;
        Rigidbody arrowRb = arrowClone.GetComponentInChildren<Rigidbody>();
        arrowRb.AddForce(arrowSpawnPoint[0].transform.forward * arrowForce, ForceMode.Impulse);

    }

    public void ActiveArrow()//ArrowInHand- AnimationEvent
    {
        arrowInHand[0].SetActive(true);
    }

    #region TRÝPLLEATTACK
    public void TripleArrowAttack() //AnimaitonEvent
    {
        TripleArrowIsActive(false);


        GameObject arrowClone = GameObject.Instantiate(tripleArrow, arrowSpawnPoint[0].transform.position, arrowSpawnPoint[0].transform.rotation) as GameObject;
        GameObject arrowClone1 = GameObject.Instantiate(tripleArrow, arrowSpawnPoint[1].transform.position, arrowSpawnPoint[1].transform.rotation) as GameObject;
        GameObject arrowClone2 = GameObject.Instantiate(tripleArrow, arrowSpawnPoint[2].transform.position, arrowSpawnPoint[2].transform.rotation) as GameObject;
        Rigidbody arrowRb = arrowClone.GetComponentInChildren<Rigidbody>();
        arrowRb.AddForce(arrowSpawnPoint[0].transform.forward * arrowForce, ForceMode.Impulse);
        Rigidbody arrowRb1 = arrowClone1.GetComponentInChildren<Rigidbody>();
        arrowRb1.AddForce(arrowSpawnPoint[1].transform.forward * arrowForce, ForceMode.Impulse);
        Rigidbody arrowRb2 = arrowClone2.GetComponentInChildren<Rigidbody>();
        arrowRb2.AddForce(arrowSpawnPoint[2].transform.forward * arrowForce, ForceMode.Impulse);

    }


    void TripleArrowIsActive(bool isTripleArrowActive) // TripleArrowInHand
    {
        for (int i = 0; i < arrowInHand.Length; i++)
        {
            arrowInHand[i].SetActive(isTripleArrowActive);
        }
    }


    IEnumerator IsTripleArrowAttack() // Q Check
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            animator.SetBool("ArrowAim", true);
            animator.SetBool("ArrowTripleAimShoot", true);
            animator.SetBool("ArrowAimShoot", true);
            TripleArrowIsActive(true);
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            animator.SetBool("ArrowAim", false);
            animator.SetBool("ArrowAimShoot", false);
            animator.SetBool("ArrowTripleAimShoot", false);

            yield return new WaitForSeconds(0.5f);
            rangerAiming = false;
        }
    }
    #endregion
}
