using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.XR;
using UnityEngine.Rendering;
using static UnityEngine.LightAnchor;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [Header("--- Components ---")]
    [SerializeField] CharacterController characterController;
    public Transform mainCamera;
    public Transform cam;
    public Transform playerTransform;
    [SerializeField] LineRenderer lineRendered;
    [SerializeField] AudioSource aud;
    public Transform playerHitBox;
    DefaultInput input;
    public Vector2 inputMovement;
    public Vector2 inputCamera;
    public float mouseScroll;
    [SerializeField] float lockVerMin;
    [SerializeField] float lockVerMax;

    Vector3 cameraRotation;
    Vector3 playerRotation;

    [Header("--- Player Stats ---")]
    [Range(10, 1000)][SerializeField] int HP;

    [Header("---Preferences---")]
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

    public float timeSinceLastJump;
    Vector3 previousJumpDirection;

    public bool isSprinting;
    public bool isWalking;
    Vector3 movementSpeed;
    Vector3 velocitySpeed;
    int hpOrigin;
    bool isPlayingFootSteps;

    [Header("Weapon Stats")]
    public List<gunStats> gunList = new List<gunStats>();
    [Range(0, 10)][SerializeField] float shootRate;
    [Range(0, 500)][SerializeField] int shootDist;
    [Range(0, 250)][SerializeField] int shootDmg;
    [SerializeField] MeshFilter weaponMesh;
    [SerializeField] MeshRenderer weaponMaterial;
    [SerializeField] GameObject shootEffect;
    public WeaponController currentWeapon;
    public float weaponAnimSpeed;
    int selectedGun;
    bool isShooting;
    bool canShoot;

    WaitForSeconds shootRateWait;

    [Header("--- Weapon Transformations---")]
    public Transform weaponHolderPos;
    public Transform weapon;
    public Transform weaponSights;
    public Transform shootEffectPos;

    [Header("----- Weapon ADS Stats -----")]
    public float zoomMax;
    public int zoomInSpeed;
    public int zoomOutSpeed;

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

        input.Player.MouseScrollWheel.performed += e => mouseScroll = e.ReadValue<float>();

        input.Weapon.AimPressed.performed += e => AimPressed();
        input.Weapon.AimReleased.performed += e => AimReleased();


        input.Weapon.Shoot.performed += e => Shoot();
        //input.Weapon.Shoot.performed += e => isShooting = true;
        //input.Weapon.Shoot.canceled += e => isShooting = false;

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
        canShoot = true;

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

    private void Start()
    {
        hpOrigin = HP;

        RespawnPlayer();
        UpdateHP();
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
            JumpTimer();
            ShootMouseClick();

            Cam();
        }
    }

    void Cam()
    {
        //Camera Rotation
        cameraRotation.x += (isAiming ? playerSettings.cameraSensVer * playerSettings.aimSpeedEffector : playerSettings.cameraSensVer) * (playerSettings.invertY ? inputCamera.y : -inputCamera.y) * Time.deltaTime;
        cameraRotation.x = Mathf.Clamp(cameraRotation.x, lockVerMin, lockVerMax);
        mainCamera.localRotation = Quaternion.Euler(cameraRotation);

    }

    void Movement()
    {
        // Player Rotation
        playerRotation.y += (isAiming ? playerSettings.cameraSensHor * playerSettings.aimSpeedEffector : playerSettings.cameraSensHor) * (playerSettings.invertX ? -inputCamera.x : inputCamera.x) * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(playerRotation);

        if (!isGrounded)
        {
            playerGravity -= gravity * Time.fixedDeltaTime;
        }
        else
        {
            if (playerGravity < -0.1f)
            {
                playerGravity = -0.1f;
            }
        }

        if (isGrounded && playerGravity <= 0)
        {
            if (!isPlayingFootSteps && movementSpeed.normalized.magnitude > 0.5f)
            {
                StartCoroutine(PlayFootSteps());
            }
        }

        if (inputMovement.y <= 0.1f || isAiming)
        {
            isSprinting = false;
        }

        if (Mathf.Abs(inputMovement.y) <= 0.1f && Mathf.Abs(inputMovement.x) <= 0.1f || isSprinting)
        {
            isWalking = false;
        }
        else
        {
            isWalking = true;
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

        transformForward.y = 0f;
        transformRight.y = 0f;

        Vector3 moveDirection = (inputMovement.y * transformForward) + (inputMovement.x * transformRight);

        if (moveDirection.magnitude > 1f)
        {
            moveDirection.Normalize();
        }
        else if (Mathf.Abs(inputMovement.x) < 0.1f && Mathf.Abs(inputMovement.y) < 0.1f)
        {
            moveDirection = Vector3.zero;
        }

        return moveDirection;
    }

    Vector3 CalculateNewMovement(Vector3 moveDirection, float verticalSpeed, float horizontalSpeed)
    {
        if (isGrounded)
        {
            playerSettings.isJumping = false;

            float forwardSpeed = verticalSpeed * inputMovement.y * Time.fixedDeltaTime;
            float strafeSpeed = horizontalSpeed * inputMovement.x * Time.fixedDeltaTime;

            movementSpeed = Vector3.SmoothDamp(movementSpeed, new Vector3(strafeSpeed, 0, forwardSpeed), ref velocitySpeed, isGrounded ? playerSettings.movementSmoothing : playerSettings.fallingSmoothing);

            float backwardJumpingFactor = inputMovement.y < 0 ? 0.75f : 1f;
            playerSettings.jumpDirection = backwardJumpingFactor * movementSpeed.magnitude * moveDirection;
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
            float angleBetweenJumps = Vector3.Angle(previousJumpDirection, playerSettings.jumpDirection); // Calculate the angle between previous jump direction and current jump direction

            if (angleBetweenJumps >= playerSettings.angleThreshold && timeSinceLastJump <= playerSettings.jumpTimeWindow)
            {
                float speedReductionFactor = Mathf.Clamp(angleBetweenJumps / 180f, 0.25f, 0.5f);
                playerSettings.jumpDirection *= speedReductionFactor;
            }

            timeSinceLastJump = 0f;
            previousJumpDirection = playerSettings.jumpDirection; // Update previousJumpDirection

            newMovementSpeed = playerSettings.jumpDirection;
        }

        newMovementSpeed.y += playerGravity;
        newMovementSpeed += jumpForce * Time.fixedDeltaTime;

        return newMovementSpeed;
    }

    IEnumerator PlayFootSteps()
    {
        isPlayingFootSteps = true;
        aud.PlayOneShot(audFootSteps[UnityEngine.Random.Range(0, audFootSteps.Length)], audFootStepsVol);
        if (!isSprinting)
        {
            yield return new WaitForSeconds(0.5f);
        }
        else
        {
            yield return new WaitForSeconds(0.3f);
        }

        isPlayingFootSteps = false;
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

        if (isGrounded)
        {
            playerGravity = 0;
            jumpForce = new Vector3(0, playerSettings.jumpingHeight, 0); // Set the initial upward force for jumping
            currentWeapon.TriggerJump();
        }
    }

    void Jumping()
    {
        if (!isGrounded)
        {
            isSprinting = false;
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

    void JumpTimer()
    {
        if (!playerSettings.isJumping)
        {
            timeSinceLastJump += Time.deltaTime;

            if (timeSinceLastJump > playerSettings.jumpTimeWindow)
            {
                timeSinceLastJump = playerSettings.jumpTimeWindow + 1f;
            }
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

    public void ResetGun()
    {
        Camera.main.fieldOfView = currentWeapon.fovOrig;
        weaponHolderPos.transform.localPosition = new Vector3(0, 0, 0);
        weapon.transform.localPosition = new Vector3(0, 0, 0);
        weapon.transform.eulerAngles = new Vector3(0, 0, 0);
        weapon.transform.localScale = new Vector3(0, 0, 0);
        weaponSights.transform.localPosition = new Vector3(0, 0, 0);
        shootEffectPos.localPosition = new Vector3(0, 0, 0);
    }

    void SelectGun()
    {
        if (gunList.Count > 1)
        {
            if (mouseScroll > 0 && selectedGun < gunList.Count - 1)
            {
                selectedGun++;
                ChangeGun();
            }
            else if (mouseScroll < 0 && selectedGun > 0)
            {
                selectedGun--;
                ChangeGun();
            }
        }
    }

    void ChangeGun()
    {
        ResetGun();

        weaponHolderPos.transform.localPosition = gunList[selectedGun].gunPosition;
        weapon.transform.localRotation = Quaternion.Euler(gunList[selectedGun].gunRotation);
        weapon.transform.localScale = gunList[selectedGun].gunScale;
        weaponSights.transform.localPosition = gunList[selectedGun].gunModeSightsPos;

        shootRate = gunList[selectedGun].shtRate;
        shootRateWait = new WaitForSeconds(shootRate);
        shootDist = gunList[selectedGun].shtDist;
        shootDmg = gunList[selectedGun].shtDmg;
        currentWeapon.fovZoomMax = gunList[selectedGun].zoomMaxFov;
        currentWeapon.fovZoomInSpd = gunList[selectedGun].zoomInSpd;
        currentWeapon.fovZoomOutSpd = gunList[selectedGun].zoomOutSpd;
        shootEffectPos.transform.localPosition = gunList[selectedGun].shootEffectPos;
        audShoot = gunList[selectedGun].gunShotAud;
        shootEffect = gunList[selectedGun].shootEffect;

        weaponMesh.sharedMesh = gunList[selectedGun].gunModel.GetComponent<MeshFilter>().sharedMesh;
        weaponMaterial.sharedMaterial = gunList[selectedGun].gunModel.GetComponent<MeshRenderer>().sharedMaterial;
    }

    public void GunPickup(gunStats gunStat)
    {
        ResetGun();
        gunList.Add(gunStat);

        weaponHolderPos.transform.localPosition = gunStat.gunPosition;
        weapon.transform.localRotation = Quaternion.Euler(gunStat.gunRotation);
        weapon.transform.localScale = gunStat.gunScale;
        weaponSights.transform.localPosition = gunStat.gunModeSightsPos;

        shootRate = gunStat.shtRate;
        shootRateWait = new WaitForSeconds(shootRate);
        shootDist = gunStat.shtDist;
        shootDmg = gunStat.shtDmg;
        currentWeapon.fovZoomMax = gunStat.zoomMaxFov;
        currentWeapon.fovZoomInSpd = gunStat.zoomInSpd;
        currentWeapon.fovZoomOutSpd = gunStat.zoomOutSpd;
        shootEffectPos.transform.localPosition = gunStat.shootEffectPos;
        audShoot = gunStat.gunShotAud;
        shootEffect = gunStat.shootEffect;

        weaponMesh.sharedMesh = gunStat.gunModel.GetComponent<MeshFilter>().sharedMesh;
        weaponMaterial.sharedMaterial = gunStat.gunModel.GetComponent<MeshRenderer>().sharedMaterial;

        selectedGun = gunList.Count - 1;
    }
   
    public void UpdateHP()
    {
        gameManager.instance.playerHPBar.fillAmount = (float)HP / (float)hpOrigin;
    }

    public void HealthPickup(healthStats health)
    {
        if (HP + health.HP > hpOrigin)
        {
            HP = hpOrigin;
        }
        else
        {
            HP += health.HP;
        }
        UpdateHP();
    }

    public void RespawnPlayer()
    {
        if (PlayerPrefs.HasKey("PlayerHealth"))
        {

            float playerPosX = PlayerPrefs.GetFloat("PlayerPosX");
            float playerPosY = PlayerPrefs.GetFloat("PlayerPosY");
            float playerPosZ = PlayerPrefs.GetFloat("PlayerPosZ");


            float playerRotX = PlayerPrefs.GetFloat("PlayerRotX");
            float playerRotY = PlayerPrefs.GetFloat("PlayerRotY");
            float playerRotZ = PlayerPrefs.GetFloat("PlayerRotZ");
            float playerRotW = PlayerPrefs.GetFloat("PlayerRotW");


            HP = PlayerPrefs.GetInt("PlayerHealth");


            // If the gun string is not empty, split it into an array of gun names and add each gun to the gun list


            gameManager.instance.civilliansRescued = PlayerPrefs.GetInt("civilliansRescued");
            gameManager.instance.civilliansRescuedText.text = gameManager.instance.civilliansRescued.ToString("F0");
            //PlayerPrefs.SetString("CurrentGun", currentGun);


            gameManager.instance.bossesKilled = PlayerPrefs.GetInt("BossesKilled");
            gameManager.instance.bossesKilledText.text = gameManager.instance.bossesKilled.ToString("F0");
            gameManager.instance.pointsTotal = PlayerPrefs.GetInt("pointsTotal");

            gameManager.instance.pointsTotalText.text = gameManager.instance.pointsTotal.ToString("F0");



            transform.position = new Vector3(playerPosX, playerPosY, playerPosZ);

        }
        else
        {
            HP = hpOrigin;
            UpdateHP();
            characterController.enabled = false;
            transform.position = gameManager.instance.playerSpawnPos.transform.position;
            characterController.enabled = true;
        }
    }

    public void TakeDmg(int dmg)
    {
        HP -= dmg;
        aud.PlayOneShot(audDamage[UnityEngine.Random.Range(0, audDamage.Length)], audDamageVol);
        UpdateHP();
        StartCoroutine(gameManager.instance.playerHit());

        if (HP <= 0)
        {
            isAiming = false;
            gameManager.instance.playerDead();
            gameManager.instance.updateGameGoal(0, 0, 0, 0, 0, -(gameManager.instance.pointsTotal / 10), true);
        }
    }

    void Shoot()
    {
        if (!isShooting && gunList.Count > 0 && canShoot)
        {
            StartCoroutine(Shooting());
        }
        //else
        //{
        //    StopCoroutine(Shooting());
        //}
    }

    void ShootMouseClick()
    {
        if (!isShooting && Input.GetButton("Shoot") && gunList.Count > 0 && canShoot)
        {
            StartCoroutine(Shooting());
        }
        else
        {
            StopCoroutine(Shooting());
        }
    }

    //void StopShooting()
    //{
    //    if (isShooting == false)
    //    {
    //        StopCoroutine(Shooting());
    //    }
    //}

    IEnumerator Shooting()
    {
        isShooting = true;
        canShoot = false;
        aud.PlayOneShot(audShoot, audShootVol);
        Instantiate(shootEffect, shootEffectPos.position, shootEffect.transform.rotation);

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDist))
        {
            lineRendered.enabled = true;
            lineRendered.SetPosition(0, shootEffectPos.position);
            lineRendered.SetPosition(1, hit.point);
            Debug.Log(hit.collider.gameObject.name);
            if (hit.collider.GetComponent<IDamage>() != null)
            {
                hit.collider.GetComponent<IDamage>().takeDmg(shootDmg);
            }
            if (hit.collider.GetComponent<ZombieAI>() != null)
            {
                hit.collider.GetComponent<ZombieAI>().TakeDamage(shootDmg);
            }
            if (hit.collider.GetComponent<Barrier>() != null)
            {
                hit.collider.GetComponent<Barrier>().TakeDmg(shootDmg);
            }
        }

        yield return shootRateWait;
        lineRendered.enabled = false;
        isShooting = false;
        canShoot = true;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(playerTransform.position, playerSettings.isGroundedRadius);
    }
}
