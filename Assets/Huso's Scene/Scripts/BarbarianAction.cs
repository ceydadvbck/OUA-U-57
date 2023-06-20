using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;

public class BarbarianAction : MonoBehaviour
{
    Animator anim;
    [Header("AIM")]
    [SerializeField] private MultiAimConstraint bodyAim;
    [SerializeField] private GameObject lookAt;
    GameObject aimLookAt;

    [Space(10)]

    [Header("AXE")]
    public Rigidbody rbAxe;
    [SerializeField] BoxCollider axeCol;
    [SerializeField] GameObject axeParent;

    [Space(10)]

    [Header("ATTACK COOLDOWN")]
    public float classicAttackCoolDown = 1f;
    public float throwAttackCoolDown = 5f;
    public float groundAttackCoolDown = 20f;


    [Space(10)]


    [Header("CLASSÝC ATTACK")]
    [SerializeField] private float punchRadius;
    float classicAttackTime;

    [Space(10)]

    [Header("THROW")]
    [SerializeField] GameObject throwPoint;
    [SerializeField] float throwPower;
    [SerializeField] private Transform targetAxe;
    [SerializeField] private Transform curvePoint;
    [SerializeField] private Vector3 oldPos;
    private float time = 0.0f;
    float throwAttackTime;

    [Space(10)]

    [Header("GROUND ATTACK")]
    [SerializeField] private Transform groundAttackPoint;
    float groundAttackTime;
    [SerializeField] private GameObject groundAttackParticle;


     bool axeThrow = false;
     bool axeReturn = false;
     bool axeInTheAir = false;
     bool axeGroundAttack = false;
     bool axeHolded = true;
     bool classicAttack = false;
    private UIManager uiManager;

    public void Awake()
    {
        uiManager = UIManager.Instance;
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        aimLookAt = GameObject.FindGameObjectWithTag("AimLookAt");
        anim.SetBool("AxeHolded", true);
    }


    void Update()
    {
        BarbarianBodyAimRegulation();

        if (Input.GetMouseButtonDown(0) && !axeThrow && !axeInTheAir)
        {
            if (Time.time > classicAttackTime)
            {
                anim.SetBool("AxeClassicAttack", true);
                StartCooldown(uiManager.classicCoolDownFill, classicAttackCoolDown);
                StartCoroutine(ClassicAttackControl());

                classicAttack = true;

                classicAttackTime = 0f;
                classicAttackTime = Time.time + classicAttackCoolDown;
            }
        }


        if (Input.GetKeyDown(KeyCode.Q) && !axeGroundAttack && !axeInTheAir)
        {
            if (Time.time > groundAttackTime)
            {
                anim.SetBool("AxeGroundAttack", true);
                StartCooldown(uiManager.groundCoolDownFill, groundAttackCoolDown);
                bodyAim.weight = 0f;

                groundAttackTime = 0f;
                groundAttackTime = Time.time + groundAttackCoolDown;
            }
        }

        #region AXE THROW ANÝMATÝON CONTROL

        if (Input.GetMouseButtonDown(1) && !axeInTheAir)
        {
            anim.SetBool("AxeThrowAim", true);
            anim.SetBool("AxeCaught", false);

            axeThrow = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            anim.SetBool("AxeThrowAim", false);
            axeThrow = false;
        }

        if (axeThrow)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (Time.time > throwAttackTime)
                {
                    StartCooldown(uiManager.throwCoolDownFill, throwAttackCoolDown);
                    anim.SetBool("AxeThrow", true);
                    anim.SetBool("AxeHolded", false);

                    axeInTheAir = true;
                    throwAttackTime = 0f;
                    throwAttackTime = Time.time + throwAttackCoolDown;
                }
            }
        }
        else anim.SetBool("AxeThrow", false);


        if (axeInTheAir)
        {
            if (Input.GetMouseButtonDown(1))
            {
                anim.SetBool("AxeHolding", true);
            }
        }

        if (axeReturn)
        {
            if (time < 1f)
            {
                axeThrow = true;

                rbAxe.position = QuadraticBezierCurvePoint(time, oldPos, curvePoint.position, targetAxe.position);
                rbAxe.rotation = Quaternion.Slerp(rbAxe.transform.rotation, targetAxe.rotation, 50 * Time.deltaTime);
                time += Time.deltaTime;
            }
            else
            {
                ResetAxe();
            }
        }
        #endregion

    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + (transform.forward - new Vector3(0, 0, .3f)) + transform.up, punchRadius);
    }

    void BarbarianBodyAimRegulation()  //Body facing the target
    {
        bodyAim.data.sourceObjects.Add(new(aimLookAt.transform, 1f));
        lookAt.transform.position = Vector3.Lerp(lookAt.transform.position, aimLookAt.transform.position, 1f);
    }

    #region CLASSÝC ATTACK

    void BarbarianClassicAttack() //AnimationEvent
    {
        Collider[] hits = Physics.OverlapSphere(transform.position + transform.forward + transform.up, punchRadius);
      
        foreach (Collider hit in hits)
        {
            if (hit.gameObject == this.gameObject) continue;

            if (hit.gameObject.transform.CompareTag("Skeleton"))
            {
                SkeletonBody skeleton = hit.gameObject.GetComponent<SkeletonBody>();
                skeleton.GetHit();
            }
           
        }
    }
    IEnumerator ClassicAttackControl()
    {
        yield return new WaitForSeconds(1f);
        anim.SetBool("AxeClassicAttack", false);
    }

    #endregion

    #region AXE THROW CONTROL
    public void AxeThrow() //AnimationEvent
    {
        axeHolded = false;
        rbAxe.useGravity = true;
        rbAxe.isKinematic = false;
        rbAxe.transform.parent = null;
        rbAxe.gameObject.GetComponent<AxeController>().activated = true;

        rbAxe.AddForce(throwPoint.transform.TransformDirection(Vector3.forward) * throwPower, ForceMode.Impulse);
        rbAxe.AddTorque(rbAxe.transform.TransformDirection(Vector3.back) * 1000, ForceMode.Acceleration);
    }

    public void ReturnAxe()//AnimationEvent
    {
        rbAxe.gameObject.GetComponent<AxeController>().activated = true;
        time = 0f;
        oldPos = rbAxe.position;
        rbAxe.velocity = Vector3.zero;
        axeReturn = true;
        rbAxe.useGravity = false;
        rbAxe.isKinematic = false;

    }

    public void ResetAxe()
    {

        anim.SetBool("AxeCaught", true);
        anim.SetBool("AxeHolding", false);
        anim.SetBool("AxeHolded", true);

        rbAxe.gameObject.GetComponent<AxeController>().activated = false;

        rbAxe.useGravity = false;
        rbAxe.isKinematic = true;
        rbAxe.transform.parent = axeParent.transform;
        rbAxe.gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
        rbAxe.gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);

        axeInTheAir = false;
        axeReturn = false;
        axeThrow = false;
        axeHolded = true;
    }


    Vector3 QuadraticBezierCurvePoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float v = 1 - t;
        float tt = t * t;
        float uu = v * v;
        Vector3 p = (uu * p0) + (2 * v * t * p1) + (tt * p2);
        return p;

    }
    #endregion

    #region GROUND ATTACK

    void BarbarianGroundAttack() //AnimationEvent
    {
        GameObject cloneGroundAttackParticle = Instantiate(groundAttackParticle, groundAttackPoint.position, gameObject.transform.rotation);
        Destroy(cloneGroundAttackParticle, 2f);

        StartCoroutine(GroundAttackTimeControl(0.75f));
    }

    IEnumerator GroundAttackTimeControl(float time)
    {
        axeGroundAttack = true;

        yield return new WaitForSeconds(time);
        anim.SetBool("AxeGroundAttack", false);
        axeGroundAttack = false;
        bodyAim.weight = 1f;
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

