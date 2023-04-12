using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    DefaultInput input;
    [SerializeField] CharacterController characterController;
    public Vector2 inputMovement;
    public Vector2 inputCamera;
    [SerializeField] float lockVerMin;
    [SerializeField] float lockVerMax;

    Vector3 cameraRotation;
    Vector3 playerRotation;

    [Header("---Preferences---")]
    public Transform mainCamera;
    public Transform playerTransform;
    [SerializeField] Models.PlayerPose playerPose;
    [SerializeField] float playerPoseSmooth;
    [SerializeField] float cameraHeight;
    [SerializeField] float cameraHeightSpeed;
    [SerializeField] float stanceCheck;
    [SerializeField] Models.PlayerStance playerStandStance;
    [SerializeField] Models.PlayerStance playerCrouchStance;
    [SerializeField] Models.PlayerStance playerProneStance;
    float playerStanceHeightVelocity;
    Vector3 playerStanceCenterVelocity;

    [Header("---Settings---")]
    public LayerMask playerMask;
    public LayerMask groundMask;
    public Models.PlayerSettingsModel playerSettings;
    [SerializeField] float gravity;
    [SerializeField] float gravitySpeed;
    [SerializeField] Vector3 jumpForce;
    float playerGravity;
    Vector3 jumpSpeed;

    public bool isSprinting;
    Vector3 movementSpeed;
    Vector3 velocitySpeed;

    [Header("Weapon")]
    public WeaponController currentWeapon;
    public float weaponAnimSpeed;

    public bool isGrounded;
    public bool isFalling;

    void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        input = new();

        input.Player.Movement.performed += e => inputMovement = e.ReadValue<Vector2>();
        input.Player.Camera.performed += e => inputCamera = e.ReadValue<Vector2>();
        input.Player.Jump.performed += e => Jump();
        input.Player.Crouch.performed += e => Crouch();
        input.Player.Prone.performed += e => Prone();

        input.Player.Sprint.performed += e => ToggleSprint();
        input.Player.SprintReleased.performed += e => StopSprint();

        input.Enable();

        cameraRotation = mainCamera.localRotation.eulerAngles;
        playerRotation = transform.localRotation.eulerAngles;

        cameraHeight = mainCamera.localPosition.y;

        if (currentWeapon)
        {
            currentWeapon.Initialize(this);
        }
    }

    void Update()
    {
        SetIsGrounded();
        SetIsFalling();

        Camera();
        Movement();
        Jumping();
        PlayerStance();
    }

    void SetIsGrounded()
    {
        isGrounded = Physics.CheckSphere(playerTransform.position, playerSettings.isGroundedRadius, groundMask);
    }

    void SetIsFalling()
    {
        isFalling = !isGrounded && characterController.velocity.magnitude >= playerSettings.isFallingSpeed;
    }

    void Camera()
    {
        // Horizontal Rotation
        playerRotation.y += playerSettings.cameraSensHor * (playerSettings.invertX ? -inputCamera.x : inputCamera.x);
        transform.localRotation = Quaternion.Euler(playerRotation);

        // Vertical Rotation
        cameraRotation.x += playerSettings.cameraSensVer * (playerSettings.invertY ? inputCamera.y : -inputCamera.y);
        cameraRotation.x = Mathf.Clamp(cameraRotation.x, lockVerMin, lockVerMax);
        mainCamera.localRotation = Quaternion.Euler(cameraRotation);
    }

    void Movement()
    {
        if (inputMovement.y <= 0.2f)
        {
            isSprinting = false;
        }

        float verticalSpeed = playerSettings.forwardWalkSpeed;
        float horizontalSpeed = playerSettings.strafeWalkSpeed;

        if (isSprinting)
        {
            verticalSpeed = playerSettings.forwardSprintSpeed;
            horizontalSpeed = playerSettings.strafeSprintSpeed;
        }

        if (!isGrounded)
        {
            playerSettings.speedEffector = playerSettings.fallingSpeedEffector;
        }
        else if (playerPose == Models.PlayerPose.Crouch)
        {
            playerSettings.speedEffector = playerSettings.crouchSpeedEffector;
        }
        else if (playerPose == Models.PlayerPose.Prone)
        {
            playerSettings.speedEffector = playerSettings.proneSpeedEffector;
        }
        else
        {
            playerSettings.speedEffector = 1;
        }

        weaponAnimSpeed = characterController.velocity.magnitude / (playerSettings.forwardWalkSpeed * playerSettings.speedEffector);

        if (weaponAnimSpeed > 1)
        {
            weaponAnimSpeed = 1;
        }

        verticalSpeed *= playerSettings.speedEffector;
        horizontalSpeed *= playerSettings.speedEffector;

        movementSpeed = Vector3.SmoothDamp(movementSpeed, new(horizontalSpeed * inputMovement.x * Time.deltaTime, 0, verticalSpeed * inputMovement.y * Time.deltaTime), ref velocitySpeed, isGrounded ? playerSettings.movementSmoothing : playerSettings.fallingSmoothing);
        Vector3 newMovementSpeed = transform.TransformDirection(movementSpeed);

        if (playerGravity > gravitySpeed)
        {
            playerGravity -= gravity * Time.deltaTime;
        }

        if (playerGravity < -0.1f && isGrounded)
        {
            playerGravity = -0.1f;
        }

        newMovementSpeed.y += playerGravity;
        newMovementSpeed += jumpForce * Time.deltaTime;

        characterController.Move(newMovementSpeed);
    }

    void PlayerStance()
    {
        Models.PlayerStance currentStance = playerStandStance;

        if (playerPose == Models.PlayerPose.Crouch)
        {
            currentStance = playerCrouchStance;
        }
        else if (playerPose == Models.PlayerPose.Prone)
        {
            currentStance = playerProneStance;
        }


        cameraHeight = Mathf.SmoothDamp(mainCamera.localPosition.y, currentStance.cameraHeight, ref cameraHeightSpeed, playerPoseSmooth);
        mainCamera.localPosition = new Vector3(mainCamera.localPosition.x, cameraHeight, mainCamera.localPosition.z);

        characterController.height = Mathf.SmoothDamp(characterController.height, currentStance.stanceCollider.height, ref playerStanceHeightVelocity, playerPoseSmooth);
        characterController.center = Vector3.SmoothDamp(characterController.center, currentStance.stanceCollider.center, ref playerStanceCenterVelocity, playerPoseSmooth);

    }

    void Jumping()
    {
        jumpForce = Vector3.SmoothDamp(jumpForce, Vector3.zero, ref jumpSpeed, playerSettings.jumpingFalloff);
    }

    void Jump()
    {
        if (!isGrounded || playerPose == Models.PlayerPose.Prone)
        {
            return;
        }

        if (playerPose == Models.PlayerPose.Crouch)
        {
            if (StanceCheck(playerStandStance.stanceCollider.height))
            {
                return;
            }

            playerPose = Models.PlayerPose.Stand;
            //return; --------------------------- This can be used to make the player stand up and not jump at the same time.
        }

        jumpForce = Vector3.up * playerSettings.jumpingHeight;
        playerGravity = 0;
        currentWeapon.TriggerJump();
    }

    void Crouch()
    {
        if (playerPose == Models.PlayerPose.Crouch) // || playerPose == Models.PlayerPose.Prone) : This can be added for the player -
        {                                                                                      // to be able to stand straight up from prone.
            if (StanceCheck(playerStandStance.stanceCollider.height))
            {
                return;
            }

            playerPose = Models.PlayerPose.Stand;
            return;
        }

        if (StanceCheck(playerCrouchStance.stanceCollider.height))
        {
            return;
        }

        playerPose = Models.PlayerPose.Crouch;
    }

    void Prone()
    {
        if (playerPose == Models.PlayerPose.Prone)
        {
            if (StanceCheck(playerStandStance.stanceCollider.height))
            {
                if (StanceCheck(playerCrouchStance.stanceCollider.height))
                {
                    return;
                }

                playerPose = Models.PlayerPose.Crouch;
                return;
            }

            playerPose = Models.PlayerPose.Stand;
            return;
        }

        playerPose = Models.PlayerPose.Prone;
    }

    bool StanceCheck(float stanceCheckHeight)
    {
        Vector3 start = new Vector3(playerTransform.position.x, playerTransform.position.y + characterController.radius + stanceCheck, playerTransform.position.z);
        Vector3 end = new Vector3(playerTransform.position.x, playerTransform.position.y - characterController.radius - stanceCheck + stanceCheckHeight, playerTransform.position.z);

        return Physics.CheckCapsule(start, end, characterController.radius, playerMask);
    }

    void ToggleSprint()
    {
        if (inputMovement.y <= 0.2f)
        {
            isSprinting = false;
            return;
        }

        isSprinting = !isSprinting;
    }

    void StopSprint()
    {
        if (playerSettings.holdSprint)
        {
            isSprinting = false;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(playerTransform.position, playerSettings.isGroundedRadius);
    }
}
