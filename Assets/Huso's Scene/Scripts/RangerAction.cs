using UnityEngine;
using UnityEngine.Animations.Rigging;
using Mirror;
using System.Collections;
using UnityEngine.UI;

public class RangerAction : NetworkBehaviour
{
    Animator animator;
    GameObject mainCam;

    [Header("Arrow")]
    [SerializeField] GameObject arrow;
    [SerializeField] GameObject iceArrow;
    [SerializeField] GameObject tripleArrow;
    [SerializeField] float arrowForce;
    [SerializeField] GameObject shootArrowLine;
    [SerializeField] GameObject[] arrowInHand;
    GameObject prefabToInstantiate;


    [Space(10)]

    [Header("CoolDown")]
    public float classicAttackCoolDown;
    public float tripleAttackCoolDown;
    public float iceAttackCoolDown;

    float classicAttackTime = 0f;
    float iceAttackTime = 0f;
    float tripleAttackTime = 0f;

    [Space(10)]
    [SerializeField] Transform[] arrowSpawnPoint;

    [Space(10)]

    [Header("Constraints")]
    [SerializeField] private MultiRotationConstraint bodyAim;
    [SerializeField] private MultiAimConstraint handAim;
    [SerializeField] private MultiRotationConstraint handRot;
    [SerializeField] private TwoBoneIKConstraint rightHand;
    [SerializeField] private TwoBoneIKConstraint leftHand;

    [Space(10)]

    [Header("Aim")]
    [SerializeField] GameObject lookAt;
    [SerializeField] bool rangerAiming = false;
    GameObject aimLookAt;
    public float transitionDuration = 0.5f;
    private Vector3 initialOffset;
    private Vector3 targetOffset;
    private float transitionTimer;

    [Header("UIManager")]
    public Image classicArrowCoolDownFill;
    public Image iceArrowCoolDownFill;
    public Image trippleArrowCoolDownFill;


    [SerializeField] private bool classicArrowShoot;
    [SerializeField] private bool tripleArrowShoot;

    AudioSource audiosource;
    [SerializeField] AudioClip acBow;
    [SerializeField] AudioClip acArrow;


    public void Start()
    {
        aimLookAt = GameObject.FindGameObjectWithTag("AimLookAt");
        animator = GetComponent<Animator>();
        audiosource = GetComponent<AudioSource>();
        mainCam = GameObject.FindGameObjectWithTag("MainCamera");

        initialOffset = bodyAim.data.offset;

        //Cursor.lockState = CursorLockMode.Locked;
    }


    public void Update()
    {

        if (!hasAuthority) return;

        CmdRangerBodyAimRegulation(lookAt.transform.position, targetOffset);

        if (Input.GetMouseButtonDown(1))
        {
            if (!rangerAiming)
            {
                RangerAim(bodyAim.weight);
                audiosource.PlayOneShot(acBow);
            }
        }
        if (Input.GetMouseButtonUp(1))
        {
            animator.SetBool("ArrowAim", false);
            animator.SetBool("ArrowAimShoot", false);

            rangerAiming = false;

        }

        if (rangerAiming)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (Time.time > classicAttackTime)
                {
                    animator.SetBool("ArrowAimShoot", true);
                    StartCooldown(classicArrowCoolDownFill, classicAttackCoolDown);
                    audiosource.PlayOneShot(acArrow);
                    tripleArrowShoot = false;
                    classicArrowShoot = true;
                    classicAttackTime = 0f;
                    classicAttackTime = Time.time + classicAttackCoolDown;

                }

            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (Time.time > iceAttackTime)
                {
                    animator.SetBool("ArrowAimShoot", true);
                    StartCooldown(iceArrowCoolDownFill,  iceAttackCoolDown);
                    audiosource.PlayOneShot(acArrow);
                    tripleArrowShoot = false;
                    classicArrowShoot = false;
                    iceAttackTime = 0f;
                    iceAttackTime = Time.time + iceAttackCoolDown;

                }

            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (Time.time > tripleAttackTime)
                {
                    animator.SetBool("ArrowAimShoot", true);
                    StartCooldown(trippleArrowCoolDownFill, tripleAttackCoolDown);
                    audiosource.PlayOneShot(acArrow);
                    tripleArrowShoot = true;
                    classicArrowShoot = false;
                    tripleAttackTime = 0f;
                    tripleAttackTime = Time.time + tripleAttackCoolDown;

                }

            }
        }
        else
        {

            if (isServer)
            {
                RpcRangerAimNot();
            }
            else
            {
                CmdRangerAimNot();
            }

            rightHand.weight = 0f;
            leftHand.weight = 0f;
            shootArrowLine.SetActive(false);
            arrowInHand[0].SetActive(false);

        }
    }

    #region BODY REGULATÝON

    [Command]
    void CmdRangerBodyAimRegulation(Vector3 lookAtPos, Vector3 offset)
    {
        RpcRangerBodyAimRegulation(lookAtPos, offset);
    }

    [ClientRpc]
    public void RpcRangerBodyAimRegulation(Vector3 lookAtPos, Vector3 offset)
    {
        if (!hasAuthority)
        {
            lookAt.transform.position = lookAtPos;
            bodyAim.data.offset = offset;
            return;
        }

        handAim.data.sourceObjects.Add(new(aimLookAt.transform, 1f));
        lookAt.transform.position = Vector3.Lerp(lookAt.transform.position, aimLookAt.transform.position, 1f);

        float rotX = mainCam.transform.rotation.eulerAngles.x;
        targetOffset = new Vector3(0, 95, rotX);

        transitionTimer = 0f;
        transitionTimer += Time.deltaTime;

        float t = Mathf.Clamp01(transitionTimer / transitionDuration);

        bodyAim.data.offset = Vector3.Lerp(initialOffset, targetOffset, t);
    }



    #endregion

    #region RANGER AÝM
    private void RangerAim(float aimWeight)
    {
        aimWeight = 1f;
        SetAimLocal(aimWeight);
        CmdRangerAim(aimWeight);
    }

    void SetAimLocal(float value)
    {

        if (bodyAim != null)
        {
            bodyAim.weight = value;
        }

        if (animator != null)
        {
            animator.SetBool("ArrowAim", value > 0f);
        }

        rangerAiming = value > 0f;
    }

    [Command]
    private void CmdRangerAim(float weight)
    {
        RpcSetAimLocal(weight); // SetAimLocal fonksiyonunu tüm client'lara senkronize etmek için Rpc kullanýlýyor
    }

    [ClientRpc]
    private void RpcSetAimLocal(float weight)
    {
        SetAimLocal(weight); // SetAimLocal fonksiyonu client'lar üzerinde çaðrýlýyor
    }

    [Command]
    public void CmdRangerAimNot()
    {
        RpcRangerAimNot();
    }
    [ClientRpc]
    public void RpcRangerAimNot()
    {
        if (bodyAim.weight > 0) bodyAim.weight -= 4f * Time.deltaTime;

    }
    #endregion


    #region RANGER ATTACK

    public void RangerArrowAttack() //AnimationEvent
    {
        CmdRangerArrowAttack(classicArrowShoot, tripleArrowShoot);
    }

    [Command]
    public void CmdRangerArrowAttack(bool isClassicArrowShoot, bool isTripleArrowShoot)
    {
        RpcRangerArrowAttack(isClassicArrowShoot, isTripleArrowShoot);
    }

    [ClientRpc]
    public void RpcRangerArrowAttack(bool isClassicArrowShoot, bool isTripleArrowShoot)
    {
        prefabToInstantiate = isClassicArrowShoot ? arrow : iceArrow;
        Debug.Log(isClassicArrowShoot);
        if (prefabToInstantiate != null)
        {
            if (!isTripleArrowShoot)
            {
                animator.SetBool("ArrowAimShoot", false);
                arrowInHand[0].SetActive(false);

                GameObject arrowClone = Instantiate(prefabToInstantiate, arrowSpawnPoint[0].transform.position, arrowSpawnPoint[0].transform.rotation) as GameObject;
                Rigidbody arrowRb = arrowClone.GetComponentInChildren<Rigidbody>();
                arrowRb.AddForce(arrowSpawnPoint[0].transform.forward * arrowForce, ForceMode.Impulse);
            }
            else
            {
                TripleArrowAttack();
                animator.SetBool("ArrowAimShoot", false);
            }
        }
    }


    #endregion


    #region ACTÝVE ARROW
    public void ActiveArrow()//ArrowInHand- AnimationEvent
    {
        CmdActiveArrow();
    }
    [Command]
    public void CmdActiveArrow()
    {
        RpcActiveArrow();
    }

    [ClientRpc]
    public void RpcActiveArrow()
    {
        arrowInHand[0].SetActive(true);
    }
    #endregion


    #region TRÝPLLEATTACK

    [Command]
    public void TripleArrowAttack()
    {
        RpcTripleArrowAttack();
    }
    [ClientRpc]
    public void RpcTripleArrowAttack()
    {
        arrowInHand[0].SetActive(false);

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


    #endregion

    #region COOLDOWN SYSTEM

    public void StartCooldown(Image cooldownImage, float cooldownDuration)
    {
        StartCoroutine(DoCooldown(cooldownImage, cooldownDuration));
    }


    private IEnumerator DoCooldown(Image cooldownImage, float cooldownDuration)
    {
        float currentCooldown = cooldownDuration;
        while (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
            cooldownImage.fillAmount = currentCooldown / cooldownDuration;
            yield return null;
        }
        cooldownImage.fillAmount = 0f;
    }

    #endregion

}
