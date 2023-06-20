using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Cinemachine;


public class CharacterMovement : NetworkBehaviour
{

    AnimationStateController animationState;
    CharacterController characterController;
    Animator anim;

    [Header("Ground")]
    public bool isGrounded;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    [SerializeField] public float groundRad;

    [Header("Move")]
    public float speed;
    private Vector3 velocity;
    public float rotSpeed;
    bool isMove;
    public float jumpSpeed;
    float jumpTime;
    public float jumpRate;
    public float gravityScale;
    bool jumpPressed = false;


    [SerializeField] private Transform mainCamera;
    public void Start()
    {
        characterController = GetComponent<CharacterController>();
        animationState = GetComponent<AnimationStateController>();
        anim = GetComponent<Animator>();

        OnApplicationFocus(true);
    }

    public void Update()
    {
      
        Move();

        //Height and Falling control with Ray 
        RaycastHit hit;
        Ray ray = new Ray(transform.position, -Vector3.up);

        if (Physics.Raycast(ray, out hit))
        {
            float distance = hit.distance;

            //Play falling animation if height is greater than 6f  
            if (distance > 6f) anim.SetBool("Falling", true);

        }

    }

    public override void OnStartServer()
    {
        GameObject.FindGameObjectWithTag("PlayerFollowCamera").GetComponent<CinemachineFreeLook>().Follow = transform.GetChild(0).transform;
        GameObject.FindGameObjectWithTag("PlayerFollowCamera").GetComponent<CinemachineFreeLook>().LookAt = transform.GetChild(0).transform;

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

        if (isMove) characterController.Move(movement);

        #endregion

        #region Rotate

        Vector3 camForward = Camera.main.transform.forward;
        camForward.y = 0f;
        if (!Input.GetKey(KeyCode.F))
        {
            transform.forward = Vector3.Lerp(transform.forward, camForward, 5f * Time.deltaTime);
        }
        #endregion

        #region SpeedControl

        speed = animationState.runSpeed ? 5 : 1.5f;

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

        /* #region Crouch

         if (CrouchControl())
         {
             anim.SetBool("Crouch", true);
         }
         else
         {
             anim.SetBool("Crouch", false);
         }


         #endregion*/
    }

    /* public bool CrouchControl()
     {
         if (Input.GetKey(KeyCode.LeftControl)) return true;
         if (Input.GetKeyUp(KeyCode.LeftControl)) return false;
         else return false;
     }*/

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


    private void OnApplicationFocus(bool focus) //CursorSet
    {
        if (focus) Cursor.lockState = CursorLockMode.Locked;

        else Cursor.lockState = CursorLockMode.None;
    }
}