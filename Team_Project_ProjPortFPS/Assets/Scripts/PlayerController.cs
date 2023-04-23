using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using static UnityEngine.EventSystems.StandaloneInputModule;
using static UnityEngine.LightAnchor;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

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
    public Transform cam;
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
    public float playerGravity;
    Vector3 jumpSpeed;
    public bool isGrounded;
    public bool isFalling;
    Vector3 moveDirection;

    public bool isSprinting;
    Vector3 movementSpeed;
    Vector3 velocitySpeed;

    [Header("Weapon")]
    [SerializeField] MeshFilter weapon;
    [SerializeField] MeshRenderer weaponMaterial;
    public List<gunStats> gunList = new List<gunStats>();
    public WeaponController currentWeapon;
    public float weaponAnimSpeed;
    int selectedGun;

    [Header("Leaning")]
    public Transform cameraLeanPivot;
    public float leanAngle;
    public float leanSmoothing;
    float currentLean;
    float targetLean;
    float leanVelocity;

    bool isLeaningLeft;
    bool isLeaningRight;

    [Header("Aiming")]
    public bool isAiming;

    void Awake()
    {
        instance = this;
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

        input.Weapon.AimPressed.performed += e => AimPressed();
        input.Weapon.AimReleased.performed += e => AimReleased();

        input.Player.LeanLeftPressed.performed += e => isLeaningLeft = true;
        input.Player.LeanLeftReleased.performed += e => isLeaningLeft = false;

        input.Player.LeanRightPressed.performed += e => isLeaningRight = true;
        input.Player.LeanRightReleased.performed += e => isLeaningRight = false;
        ;
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
        if (Time.timeScale != 0)
        {
            SetIsGrounded();
            SetIsFalling();

            Camera();
            Movement();
            selectGun();
        }
    }

    void RotatePlayer()
    {
        playerRotation.y += (isAiming ? playerSettings.cameraSensHor * playerSettings.aimSpeedEffector : playerSettings.cameraSensHor) * (playerSettings.invertX ? -inputCamera.x : inputCamera.x) * Time.smoothDeltaTime;
        transform.localRotation = Quaternion.Euler(playerRotation);

        cameraRotation.x += (isAiming ? playerSettings.cameraSensVer * playerSettings.aimSpeedEffector : playerSettings.cameraSensVer) * (playerSettings.invertY ? inputCamera.y : -inputCamera.y) * Time.smoothDeltaTime;
        cameraRotation.x = Mathf.Clamp(cameraRotation.x, lockVerMin, lockVerMax);
        mainCamera.localRotation = Quaternion.Euler(cameraRotation);
    }

    void RotateCamera()
    {
        cameraRotation.y += (isAiming ? playerSettings.cameraSensHor * playerSettings.aimSpeedEffector : playerSettings.cameraSensHor) * (playerSettings.invertX ? -inputCamera.x : inputCamera.x) * Time.smoothDeltaTime;
        mainCamera.localRotation = Quaternion.Euler(cameraRotation);

        cameraRotation.x += (isAiming ? playerSettings.cameraSensVer * playerSettings.aimSpeedEffector : playerSettings.cameraSensVer) * (playerSettings.invertY ? inputCamera.y : -inputCamera.y) * Time.smoothDeltaTime;
        cameraRotation.x = Mathf.Clamp(cameraRotation.x, lockVerMin, lockVerMax);
        mainCamera.localRotation = Quaternion.Euler(cameraRotation);
    }

    void Camera()
    {
        if (isGrounded)
        {
            RotatePlayer();
        }
        else
        {
            RotateCamera();
        }
    }

    void Movement()
    {
        Jumping();
        PlayerStance();
        Leaning();
        Aiming();

        if (inputMovement.y <= 0.2f || isAiming)
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
            isSprinting = false;
        }
        else if (playerPose == Models.PlayerPose.Prone)
        {
            playerSettings.speedEffector = playerSettings.proneSpeedEffector;
            isSprinting = false;
        }
        else if (isAiming)
        {
            playerSettings.speedEffector = playerSettings.aimSpeedEffector;
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

        if (isGrounded)
        {
            playerSettings.isJumping = false;

            Vector3 cameraForward = mainCamera.transform.forward;
            Vector3 cameraRight = mainCamera.transform.right;

            // Remove the Y-axis components to avoid any vertical movement
            cameraForward.y = 0;
            cameraRight.y = 0;

            // Normalize the vectors
            cameraForward.Normalize();
            cameraRight.Normalize();

            moveDirection = (inputMovement.y * cameraForward) + (inputMovement.x * cameraRight);
            moveDirection.Normalize();

            float forwardSpeed = verticalSpeed * inputMovement.y * Time.deltaTime;
            float strafeSpeed = horizontalSpeed * inputMovement.x * Time.deltaTime;

            movementSpeed = Vector3.SmoothDamp(movementSpeed, new Vector3(strafeSpeed, 0, forwardSpeed), ref velocitySpeed, isGrounded ? playerSettings.movementSmoothing : playerSettings.fallingSmoothing);
            playerSettings.jumpDirection = moveDirection * movementSpeed.magnitude;
        }
        else
        {
            if (playerSettings.isJumping)
            {
                moveDirection.x = 0;
                moveDirection.z = 0;
            }
        }

        Vector3 newMovementSpeed;

        if (isGrounded || !playerSettings.isJumping)
        {
            newMovementSpeed = moveDirection * movementSpeed.magnitude;
        }
        else
        {
            newMovementSpeed = playerSettings.jumpDirection;
        }

        if (playerGravity > gravitySpeed)
        {
            if (!isGrounded)
            {
                playerGravity -= gravity * Time.deltaTime;
            }

            if (isGrounded && playerGravity < -0.1)
            {
                playerGravity = 0;
            }
        }

        newMovementSpeed.y += playerGravity;
        newMovementSpeed += jumpForce * Time.deltaTime;

        characterController.Move(newMovementSpeed);
    }

    void AimPressed()
    {
        isAiming = true;
    }

    void AimReleased()
    {
        isAiming = false;
    }

    void Aiming()
    {
        if (!currentWeapon)
        {
            return;
        }

        currentWeapon.isAiming = isAiming;
    }

    void SetIsGrounded()
    {
        //isGrounded = Physics.Raycast(transform.position, -Vector3.up, playerSettings.groundDistance + 0.1f);
        isGrounded = Physics.CheckSphere(playerTransform.position, playerSettings.isGroundedRadius, groundMask);

        //Debug.Log("isGrounded = " + isGrounded);
    }

    void SetIsFalling()
    {
        isFalling = !isGrounded && characterController.velocity.magnitude >= playerSettings.isFallingSpeed;
    }

    void Leaning()
    {
        if (isLeaningLeft)
        {
            targetLean = leanAngle;
        }
        else if (isLeaningRight)
        {
            targetLean = -leanAngle;
        }
        else
        {
            targetLean = 0;
        }

        currentLean = Mathf.SmoothDamp(currentLean, targetLean, ref leanVelocity, leanSmoothing);
        cameraLeanPivot.localRotation = Quaternion.Euler(new Vector3(0, 0, currentLean));
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
        if (!isGrounded)
        {
            playerSettings.isJumping = true;
            jumpForce = Vector3.SmoothDamp(jumpForce, Vector3.zero, ref jumpSpeed, playerSettings.jumpingFalloff);
            StartCoroutine(ResetJump());
        }
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
            //return; --------------------------- This can be used to make the player stand up only and not jump at the same time.
        }

        jumpForce = Vector3.up * playerSettings.jumpingHeight;
        playerGravity = 0;
        currentWeapon.TriggerJump();
    }

    IEnumerator ResetJump()
    {
        yield return new WaitForSeconds(0.1f);
        playerSettings.isJumping = false;
    }

    void Crouch()
    {
        if (playerPose == Models.PlayerPose.Crouch) // || playerPose == Models.PlayerPose.Prone) : This can be added for the player -
        {                                                                                       // to be able to stand straight up from prone.
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
        Vector3 start = new(playerTransform.position.x, playerTransform.position.y + characterController.radius + stanceCheck, playerTransform.position.z);
        Vector3 end = new(playerTransform.position.x, playerTransform.position.y - characterController.radius - stanceCheck + stanceCheckHeight, playerTransform.position.z);

        return Physics.CheckCapsule(start, end, characterController.radius, playerMask);
    }

    void ToggleSprint()
    {
        if (inputMovement.y <= 0.2f || isAiming || playerPose == Models.PlayerPose.Prone)
        {
            isSprinting = false;
            return;
        }

        if (playerPose == Models.PlayerPose.Crouch)
        {
            if (StanceCheck(playerStandStance.stanceCollider.height))
            {
                return;
            }

            playerPose = Models.PlayerPose.Stand;
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
    void selectGun()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedGun < gunList.Count - 1)
        {
            selectedGun++;
            ChangeGun();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectedGun > 0)
        {
            selectedGun--;
            ChangeGun();
        }
    }

    void ChangeGun()
    {


        weapon.sharedMesh = gunList[selectedGun].gunModel.GetComponent<MeshFilter>().sharedMesh;
        weaponMaterial.sharedMaterial = gunList[selectedGun].gunModel.GetComponent<MeshRenderer>().sharedMaterial;
    }

    public void GunPickup(gunStats gunStat)
    {
        gunList.Add(gunStat);



        weapon.sharedMesh = gunStat.gunModel.GetComponent<MeshFilter>().sharedMesh;
        weaponMaterial.sharedMaterial = gunStat.gunModel.GetComponent<MeshRenderer>().sharedMaterial;

        selectedGun = gunList.Count - 1;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(playerTransform.position, playerSettings.isGroundedRadius);
    }
}
