using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;



public class AnimationStateController : NetworkBehaviour
{
    private float moveX = 0.0f;
    private float moveZ = 0.0f;
    private float crouchMoveX = 0.0f;
    private float crouchMoveZ = 0.0f;

    [HideInInspector] public bool runSpeed;
    [HideInInspector] public bool forwardPress;


    [SerializeField] private float acceleration = 4f;
    [SerializeField] private float deceleration = 2f;
    [SerializeField] float maxWalkVelocity = 1f;
    [SerializeField] float maxRunVelocity = 2f;

    CharacterMovement characterControl;
    Animator anim;

    int velocityZHash;
    int velocityXHash;
    int crouchVelocityX;
    int crouchVelocityZ;
    void Start()
    {
        anim = GetComponent<Animator>();
        characterControl = gameObject.GetComponent<CharacterMovement>();

        velocityZHash = Animator.StringToHash("MoveZ");
        velocityXHash = Animator.StringToHash("MoveX");
        crouchVelocityX = Animator.StringToHash("CrouchMoveX");
        crouchVelocityZ = Animator.StringToHash("CrouchMoveZ");
    }


    void Update()
    {
        if (!hasAuthority) return;

         Movement();
    }

    void ChangeVelocity(bool forwardPress, bool leftPress, bool rightPress, bool backPress, bool runSpeed, float currentMaxVelocity)
    {
        if (forwardPress && moveZ < currentMaxVelocity)
        {
            moveZ += Time.deltaTime * acceleration;
        }
        if (backPress && moveZ > -currentMaxVelocity)
        {
            moveZ -= Time.deltaTime * acceleration;
        }
        if (leftPress && moveX > -currentMaxVelocity)
        {
            moveX -= Time.deltaTime * acceleration;
        }
        if (rightPress && moveX < currentMaxVelocity)
        {
            moveX += Time.deltaTime * acceleration;
        }
        if (!forwardPress && moveZ > 0.0f)
        {
            moveZ -= Time.deltaTime * deceleration;
        }
        if (!backPress && moveZ < 0.0f)
        {
            moveZ += Time.deltaTime * deceleration;
        }
        if (!leftPress && moveX < 0.0f)
        {
            moveX += Time.deltaTime * deceleration;
        }
        if (!rightPress && moveX > 0.0f)
        {
            moveX -= Time.deltaTime * deceleration;
        }
    }

    void LockOrResetVelocity(bool forwardPress, bool leftPress, bool rightPress, bool backPress, bool runSpeed, float currentMaxVelocity)
    {
        if (!leftPress && !rightPress && moveX != 0.0f && (moveX > -0.05f && moveX < 0.05f))
        {
            moveX = 0.0f;
        }
        if (forwardPress && runSpeed && moveZ > currentMaxVelocity)
        {
            moveZ = currentMaxVelocity;
        }
        else if (forwardPress && moveZ > currentMaxVelocity)
        {
            moveZ -= Time.deltaTime * deceleration;

            if (moveZ > currentMaxVelocity && moveZ < (currentMaxVelocity + 0.05f))
            {
                moveZ = currentMaxVelocity;
            }
        }
        else if (forwardPress && moveZ < currentMaxVelocity && moveZ > (currentMaxVelocity - 0.05f))
        {
            moveZ = currentMaxVelocity;
        }
        if (leftPress && runSpeed && moveX < -currentMaxVelocity)
        {
            moveX = -currentMaxVelocity;
        }
        else if (leftPress && moveX < -currentMaxVelocity)
        {
            moveX += Time.deltaTime * deceleration;

            if (moveX < -currentMaxVelocity && moveX > (-currentMaxVelocity - 0.05f))
            {
                moveX = -currentMaxVelocity;
            }
        }
        else if (leftPress && moveX > -currentMaxVelocity && moveX < (-currentMaxVelocity + 0.05f))
        {
            moveX = -currentMaxVelocity;
        }
        if (rightPress && runSpeed && moveX > currentMaxVelocity)
        {
            moveX = currentMaxVelocity;
        }
        else if (rightPress && moveX > currentMaxVelocity)
        {
            moveX -= Time.deltaTime * deceleration;

            if (moveX > currentMaxVelocity && moveX < (currentMaxVelocity + 0.05f))
            {
                moveX = currentMaxVelocity;
            }
        }
        else if (rightPress && moveX < currentMaxVelocity && moveX > (currentMaxVelocity - 0.05f))
        {
            moveX = currentMaxVelocity;
        }
    }

    void CrouchVelocity(bool CforwardPress, bool CbackPress, bool CrightPress, bool CleftPress)
    {

        if (CforwardPress && crouchMoveX < maxWalkVelocity)
        {
            crouchMoveX += Time.deltaTime * acceleration;
        }
        if (!CforwardPress && crouchMoveX > -maxWalkVelocity)
        {
            crouchMoveX -= Time.deltaTime * deceleration;
        }
        if (CbackPress && crouchMoveX > -maxWalkVelocity)
        {
            crouchMoveX -= Time.deltaTime * acceleration;
        }
        if (!CbackPress && crouchMoveX < maxWalkVelocity)
        {
            crouchMoveX += Time.deltaTime * deceleration;
        }
        if (CrightPress && crouchMoveZ < maxWalkVelocity)
        {
            crouchMoveZ += Time.deltaTime * acceleration;
        }
        if (!CrightPress && crouchMoveZ > -maxWalkVelocity)
        {
            crouchMoveZ -= Time.deltaTime * deceleration;
        }
        if (CleftPress && crouchMoveZ > -maxWalkVelocity)
        {
            crouchMoveZ -= Time.deltaTime * acceleration;
        }
        if (!CleftPress && crouchMoveZ < maxWalkVelocity)
        {
            crouchMoveZ += Time.deltaTime * deceleration;
        }

    }
    void CrouchLockOrResetVelocity(bool CforwardPress, bool CbackPress, bool CrightPress, bool CleftPress)
    {

        if (!CforwardPress && !CbackPress && (crouchMoveX < 0.05f && crouchMoveX > 0.05f))
        {
            crouchMoveX = 0.0f;
        }
        if (!CleftPress && !CrightPress && (crouchMoveZ < 0.05f && crouchMoveZ > 0.05f))
        {
            crouchMoveZ = 0.0f;
        }

        if (!CforwardPress && !CbackPress && crouchMoveX != 0.0f)
        {

            if (crouchMoveX < 0f) crouchMoveX += Time.deltaTime * deceleration;
            if (crouchMoveX > 0f) crouchMoveX -= Time.deltaTime * deceleration;

        }

        if (!CleftPress && !CrightPress && crouchMoveZ != 0.0f)
        {
            if (crouchMoveZ < 0f) crouchMoveZ += Time.deltaTime * deceleration;
            if (crouchMoveZ > 0f) crouchMoveZ -= Time.deltaTime * deceleration;
        }
        if (CleftPress && crouchMoveZ < -maxWalkVelocity) crouchMoveZ = -maxWalkVelocity;

        if (CrightPress && crouchMoveZ > maxWalkVelocity) crouchMoveZ = maxWalkVelocity;

        if (CforwardPress && crouchMoveX > maxWalkVelocity) crouchMoveX = maxWalkVelocity;

        if (CbackPress && crouchMoveX < -maxWalkVelocity) crouchMoveX = -maxWalkVelocity;
    }

    private void Movement()
    {
        forwardPress = Input.GetKey(KeyCode.W);
        bool backPress = Input.GetKey(KeyCode.S);
        bool leftPress = Input.GetKey(KeyCode.A);
        bool rightPress = Input.GetKey(KeyCode.D);
        runSpeed = Input.GetKey(KeyCode.LeftShift);

        float currentMaxVelocity = runSpeed ? maxRunVelocity : maxWalkVelocity;

        ChangeVelocity(forwardPress, leftPress, rightPress, backPress, runSpeed, currentMaxVelocity);
        LockOrResetVelocity(forwardPress, leftPress, rightPress, backPress, runSpeed, currentMaxVelocity);


        anim.SetFloat(velocityXHash, moveX);
        anim.SetFloat(velocityZHash, moveZ);

    }
    private void CrouchState()
    {
        
        bool CforwardPress = Input.GetKey(KeyCode.W);
        bool CbackPress = Input.GetKey(KeyCode.S);
        bool CrightPress = Input.GetKey(KeyCode.D);
        bool CleftPress = Input.GetKey(KeyCode.A);


        CrouchVelocity(CforwardPress, CbackPress, CrightPress, CleftPress);
        CrouchLockOrResetVelocity(CforwardPress, CbackPress, CrightPress, CleftPress);

        anim.SetFloat(crouchVelocityX, crouchMoveX);
        anim.SetFloat(crouchVelocityZ, crouchMoveZ);
    }
}