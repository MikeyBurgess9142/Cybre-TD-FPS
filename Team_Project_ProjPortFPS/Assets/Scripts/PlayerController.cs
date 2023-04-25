using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.Rendering;
using static UnityEngine.EventSystems.StandaloneInputModule;
using static UnityEngine.LightAnchor;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [Header("--- Components ---")]
    [SerializeField] CharacterController characterController;
    [SerializeField] LineRenderer lineRendered;
    [SerializeField] AudioSource aud;
    public Transform playerHitBox;
    DefaultInput input;
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
    private Vector3 initialJumpDirection;
    Vector3 jumpSpeed;
    public bool isGrounded;
    public bool isFalling;

    public bool isSprinting;
    Vector3 movementSpeed;
    Vector3 velocitySpeed;
    int hpOrigin;
    bool isPlayingFootSteps;
    float playerSpdOrig;
    Vector3 forwardDirection;

    bool debugTimer;

    [Header("Weapon")]
    [SerializeField] MeshFilter weapon;
    [SerializeField] MeshRenderer weaponMaterial;
    public List<gunStats> gunList = new List<gunStats>();
    public WeaponController currentWeapon;
    public float weaponAnimSpeed;
    int selectedGun;
    bool isShooting;

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

    [Header("--- Auido Settings---")]
    [SerializeField] AudioClip[] audFootSteps;
    [Range(0, 1)][SerializeField] float audFootStepsVol;
    [SerializeField] AudioClip[] audJump;
    [Range(0, 1)][SerializeField] float audJumpVol;
    [SerializeField] AudioClip[] audDamage;
    [Range(0, 1)][SerializeField] float audDamageVol;
    [SerializeField] AudioClip audShoot;
    [Range(0, 1)][SerializeField] float audShootVol;

    private void SetupInputActions()
    {
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
    }

    void Awake()
    {
        instance = this;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        input = new();

        SetupInputActions();

        cameraRotation = mainCamera.localRotation.eulerAngles;
        playerRotation = transform.localRotation.eulerAngles;

        cameraHeight = mainCamera.localPosition.y;

        if (currentWeapon)
        {
            currentWeapon.Initialize(this);
        }
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    void FixedUpdate()
    {
        if (Time.timeScale != 0)
        {
            SetIsGrounded();
            SetIsFalling();
            Movement();
            Jumping();
        }
    }

    void Update()
    {
        if (Time.timeScale != 0)
        {
            PlayerStance();
            Leaning();
            Aiming();
            SelectGun();
        }
    }

    void LateUpdate()
    {
        if (Time.timeScale != 0)
        {
            Camera();
        }
    }

    void Camera()
    {
        //Camera Rotation
        cameraRotation.x += (isAiming ? playerSettings.cameraSensVer * playerSettings.aimSpeedEffector : playerSettings.cameraSensVer) * (playerSettings.invertY ? inputCamera.y : -inputCamera.y) * Time.smoothDeltaTime;
        cameraRotation.x = Mathf.Clamp(cameraRotation.x, lockVerMin, lockVerMax);
        mainCamera.localRotation = Quaternion.Euler(cameraRotation);
    }

    void Movement()
    {
        // Player Rotation
        playerRotation.y += (isAiming ? playerSettings.cameraSensHor * playerSettings.aimSpeedEffector : playerSettings.cameraSensHor) * (playerSettings.invertX ? -inputCamera.x : inputCamera.x) * Time.fixedDeltaTime;
        transform.localRotation = Quaternion.Euler(playerRotation);

        if (!isGrounded)
        {
            playerGravity -= gravity * Time.fixedDeltaTime;
        }
        else if (Mathf.Abs(playerGravity) > 0.3f)
        {
            playerGravity = 0;
        }

        if (inputMovement.y <= 0.2f || isAiming)
        {
            isSprinting = false;
        }

        float verticalSpeed;
        float horizontalSpeed;

        if (!isSprinting)
        {
            verticalSpeed = playerSettings.forwardWalkSpeed;
            horizontalSpeed = playerSettings.strafeWalkSpeed;
        }
        else
        {
            verticalSpeed = playerSettings.forwardSprintSpeed;
            horizontalSpeed = playerSettings.strafeSprintSpeed;
        }

        float speedEffector = GetSpeedEffector();
        verticalSpeed *= speedEffector;
        horizontalSpeed *= speedEffector;

        weaponAnimSpeed = characterController.velocity.magnitude / (playerSettings.forwardWalkSpeed * speedEffector);
        weaponAnimSpeed = Mathf.Clamp(weaponAnimSpeed, 0, 1);

        Vector3 moveDirection = CalculateMoveDirection();
        Vector3 newMovementSpeed = CalculateNewMovement(moveDirection, verticalSpeed, horizontalSpeed);

        characterController.Move(newMovementSpeed);
    }

    float GetSpeedEffector()
    {
        if (!isGrounded)
        {
            return playerSettings.fallingSpeedEffector;
        }
        else if (playerPose == Models.PlayerPose.Crouch)
        {
            return playerSettings.crouchSpeedEffector;
        }
        else if (playerPose == Models.PlayerPose.Prone)
        {
            return playerSettings.proneSpeedEffector;
        }
        else if (isAiming)
        {
            return playerSettings.aimSpeedEffector;
        }

        return 1;
    }

    Vector3 CalculateMoveDirection()
    {
        Vector3 transformForward = transform.forward.normalized;
        Vector3 transformRight = transform.right.normalized;
        transformForward.y = 0;
        transformForward.y = 0;

        return (inputMovement.y * transformForward).normalized + (inputMovement.x * transformRight).normalized;
    }

    Vector3 CalculateNewMovement(Vector3 moveDirection, float verticalSpeed, float horizontalSpeed)
    {
        if (isGrounded)
        {
            playerSettings.isJumping = false;

            float angle = Vector3.Angle(moveDirection, characterController.velocity);
            float jumpDirectionMultiplier = 1f;

            if (isSprinting)
            {
                if (angle > 20f && angle < 30f)
                {
                    jumpDirectionMultiplier = 0.25f; //<-
                }                                    // |
            }                                        // |
            else                                     // - You can adjust this value to find the best balance between responsiveness and limiting fast strafe jumps
            {                                        // |
                if (angle >= 90f)                    // |
                {                                    // |
                    jumpDirectionMultiplier = 0.5f;  //<-
                }
            }

            if (Vector3.Angle(moveDirection, characterController.velocity) != 0)
            {
                Debug.Log(Vector3.Angle(moveDirection, characterController.velocity));
            }

            float forwardSpeed = verticalSpeed * inputMovement.y * Time.fixedDeltaTime;
            float strafeSpeed = horizontalSpeed * inputMovement.x * Time.fixedDeltaTime;

            movementSpeed = Vector3.SmoothDamp(movementSpeed, new Vector3(strafeSpeed, 0, forwardSpeed), ref velocitySpeed, isGrounded ? playerSettings.movementSmoothing : playerSettings.fallingSmoothing);

            float backwardJumpingFactor = inputMovement.y < 0 ? 0.75f : 1f;
            playerSettings.jumpDirection = backwardJumpingFactor * jumpDirectionMultiplier * movementSpeed.magnitude * moveDirection;
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

        newMovementSpeed.y += playerGravity;
        newMovementSpeed += jumpForce * Time.fixedDeltaTime;

        return newMovementSpeed;
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
        //float groundCheckDistance = 0.1f; // Adjust this value as needed
        //bool raycastGrounded = Physics.Raycast(transform.localPosition, Vector3.down, out RaycastHit hit, characterController.height * 0.5f + groundCheckDistance);
        //isGrounded = characterController.isGrounded || raycastGrounded;
        isGrounded = characterController.isGrounded || Physics.CheckSphere(playerTransform.position, playerSettings.isGroundedRadius, groundMask);

        //FDebug.DrawLine(transform.localPosition, hit.point, Color.red);
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

        playerGravity = 0;
        jumpForce = new Vector3(0, playerSettings.jumpingHeight, 0); // Set the initial upward force for jumping
        currentWeapon.TriggerJump();
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

    IEnumerator ResetJump()
    {
        yield return new WaitForSeconds(0.1f);
        if (isGrounded)
        {
            playerSettings.isJumping = false;
        }
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
    void SelectGun()
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
