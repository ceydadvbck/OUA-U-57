using UnityEngine;
using UnityEngine.Animations.Rigging;
using Mirror;

[RequireComponent(typeof(NetworkIdentity))]
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

    [SyncVar(hook = nameof(RpcRangerArrowAttack))] GameObject arrowPrefab;

    [SerializeField] private bool classicArrowShoot;
    [SerializeField] private bool tripleArrowShoot;

    public void Start()
    {
        aimLookAt = GameObject.FindGameObjectWithTag("AimLookAt");
        animator = GetComponent<Animator>();

        mainCam = GameObject.FindGameObjectWithTag("MainCamera");

        initialOffset = bodyAim.data.offset;

        Cursor.lockState = CursorLockMode.Locked;
    }


    public void Update()
    {

        if (!hasAuthority) return;


        RangerBodyAimRegulation();


        if (Input.GetMouseButtonDown(1))
        {
            RangerAim();
        }
        if (Input.GetMouseButtonUp(1))
        {
            animator.SetBool("ArrowAim", false);
            animator.SetBool("ArrowAimShoot", false);

            rangerAiming = false;

        }

        if (rangerAiming)
        {
            bodyAim.weight = 1f;
            if (Input.GetButtonDown("Fire1"))
            {
                if (Time.time > classicAttackTime)
                {
                    animator.SetBool("ArrowAimShoot", true);

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

                    tripleArrowShoot = true;
                    classicArrowShoot = false;
                    tripleAttackTime = 0f;
                    tripleAttackTime = Time.time + tripleAttackCoolDown;

                }

            }
        }
        else
        {
            RangerAimNot();
          
            rightHand.weight = 0f;
            leftHand.weight = 0f;
            shootArrowLine.SetActive(false);
            arrowInHand[0].SetActive(false);

        }
    }
    #region BODY REGULATÝON
    void RangerBodyAimRegulation()
    {
        CmdRangerBodyAimRegulation();
    }
    [Command]
    void CmdRangerBodyAimRegulation() 
    {
        RpcRangerBodyAimRegulation();
    }
    [ClientRpc]
    public void RpcRangerBodyAimRegulation( )
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
    #endregion

    #region RANGER AÝM
    [Command]
    public void RangerAim()
    {
        RpcRangerAim();
    }
    [ClientRpc]
    public void RpcRangerAim()
    {
        bodyAim.weight += 1f;

        animator.SetBool("ArrowAim", true);
        rangerAiming = true;

    }
    public void RangerAimNot()
    {
        CmdRangerAimNot();
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

   
    public void RangerArrowAttack(GameObject newPrefab)
    {
        CmdRangerArrowAttack( newPrefab);
    }

    [Command]
    public void CmdRangerArrowAttack(GameObject newPrefab)
    {
        RpcRangerArrowAttack(arrowPrefab,newPrefab);
    }

    [ClientRpc]
    public void RpcRangerArrowAttack(GameObject oldValue, GameObject newValue)
    {
        
        arrowPrefab = classicArrowShoot ? arrow : iceArrow;
        arrowPrefab = oldValue;
        if (!tripleArrowShoot)
        {
            animator.SetBool("ArrowAimShoot", false);
            arrowInHand[0].SetActive(false);

            GameObject arrowClone = GameObject.Instantiate(arrowPrefab, arrowSpawnPoint[0].transform.position, arrowSpawnPoint[0].transform.rotation) as GameObject;
            Rigidbody arrowRb = arrowClone.GetComponentInChildren<Rigidbody>();
            arrowRb.AddForce(arrowSpawnPoint[0].transform.forward * arrowForce, ForceMode.Impulse);
        }
        else
        {
            TripleArrowAttack();
            animator.SetBool("ArrowAimShoot", false);
        }
    }


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

}
