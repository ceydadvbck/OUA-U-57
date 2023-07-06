using UnityEngine;
using Mirror;
using Cinemachine;


public class CharacterMovement : NetworkBehaviour
{

    AnimationStateController animationState;
    CharacterController characterController;
    NetworkAnimator anim;

    [Header("Ground")]
    public bool isGrounded;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    [SerializeField] public float groundRad;

    [Header("Move")]
    public float speed;
    public float runSpeed;
    float speedValue;
    private Vector3 velocity;
    [SerializeField] float rotSpeed;
    bool isMove;
    public float jumpSpeed;
    float jumpTime;
    [SerializeField] float jumpRate;
    [SerializeField] float gravityScale;
    bool jumpPressed = false;

    [SerializeField] private Transform mainCamera;

    public void Start()
    {
        if (!isOwned) return;
        characterController = GetComponent<CharacterController>();
        animationState = GetComponent<AnimationStateController>();
        anim = GetComponent<NetworkAnimator>();

       
    }
    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        GameObject.FindGameObjectWithTag("PlayerFollowCamera").GetComponent<CinemachineFreeLook>().Follow = transform.GetChild(0).transform;
        GameObject.FindGameObjectWithTag("PlayerFollowCamera").GetComponent<CinemachineFreeLook>().LookAt = transform.GetChild(0).transform;
        Debug.Log("xd");
    }


    public void Update()
    {
        if (!isOwned) return;

        Move();
    }


    private void Move()
    {

        #region Move
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, 0f, vertical);
        movement = Vector3.ClampMagnitude(movement, 1f);
        movement = transform.TransformDirection(movement);
        movement *= speed * Time.deltaTime;

        transform.position += movement;

        #endregion

        #region Rotate

        Vector3 camForward = Camera.main.transform.forward;
        camForward.y = 0f;

        transform.rotation = Quaternion.LookRotation(camForward);

        #endregion

        #region SpeedControl

        speedValue = animationState.runSpeed ? runSpeed : speed;

        #endregion

        #region Ground & Jump Control

        isGrounded = Physics.CheckSphere(groundCheck.position, groundRad, groundMask);
        if (isGrounded)
        {
            JumpControl();
        }
        else
        {
            velocity.y += gravityScale * Time.deltaTime;
            characterController.Move(velocity * Time.deltaTime);
        }

        #endregion

    }


    private void JumpControl()
    {
        if (velocity.y < 0f) velocity.y = -2f;


        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (jumpTime < Time.time)
            {
                jumpTime = 0f;
                jumpTime = Time.time + jumpRate;
                jumpPressed = true;

                anim.SetTrigger("Jump");
                velocity.y = jumpSpeed;
                characterController.Move(velocity * Time.deltaTime);
            }
        }
        jumpPressed = false;
    }
    public void JumpForce() => velocity.y += jumpSpeed;


  
}