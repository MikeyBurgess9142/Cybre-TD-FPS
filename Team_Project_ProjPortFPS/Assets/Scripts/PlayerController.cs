using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

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

    void Camera()
    {
        // Horizontal Rotation
        playerRotation.y += (isAiming ? playerSettings.cameraSensHor * playerSettings.aimSpeedEffector : playerSettings.cameraSensHor) * (playerSettings.invertX ? -inputCamera.x : inputCamera.x);
        transform.localRotation = Quaternion.Euler(playerRotation);

        // Vertical Rotation
        cameraRotation.x += (isAiming ? playerSettings.cameraSensVer * playerSettings.aimSpeedEffector : playerSettings.cameraSensVer) * (playerSettings.invertY ? inputCamera.y : -inputCamera.y);
        cameraRotation.x = Mathf.Clamp(cameraRotation.x, lockVerMin, lockVerMax);
        mainCamera.localRotation = Quaternion.Euler(cameraRotation);
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
        }
        else if (playerPose == Models.PlayerPose.Prone)
        {
            playerSettings.speedEffector = playerSettings.proneSpeedEffector;
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

        movementSpeed = Vector3.SmoothDamp(movementSpeed, new(horizontalSpeed * inputMovement.x * Time.deltaTime, 0, verticalSpeed * inputMovement.y * Time.deltaTime), ref velocitySpeed, isGrounded ? playerSettings.movementSmoothing : playerSettings.fallingSmoothing);
        Vector3 newMovementSpeed = transform.TransformDirection(movementSpeed);

        if (playerGravity > gravitySpeed)
        {
            if (!isGrounded)
            {
                playerGravity -= gravity * Time.deltaTime;
            }
            else
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
        isGrounded = Physics.CheckSphere(playerTransform.position, playerSettings.isGroundedRadius, groundMask);
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
            //return; --------------------------- This can be used to make the player stand up only and not jump at the same time.
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
        if (inputMovement.y <= 0.2f || isAiming)
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
