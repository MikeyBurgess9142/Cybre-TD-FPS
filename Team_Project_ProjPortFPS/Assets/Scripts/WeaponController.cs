using UnityEngine;

public class WeaponController : MonoBehaviour
{
    PlayerController playerController;

    [Header("---References---")]
    public Animator weaponAnimator;

    [Header("---Settings---")]
    public WeaponSettingsModel settings;

    bool isInitialized;

    Vector3 weaponRotation;
    Vector3 weaponRotationVelocity;

    Vector3 targetWeaponRotation;
    Vector3 targetWeaponRotationVelocity;

    Vector3 weaponMovementRotation;
    Vector3 weaponMovementRotationVelocity;

    Vector3 targetWeaponMovementRotation;
    Vector3 targetWeaponMovementRotationVelocity;

    bool isGroundedTrigger;

    public float fallingDelay;

    [Header("Weapon Idle Sway")]
    public Transform weaponSway;
    public Transform weaponBreathing;
    public Transform cameraSwayADS;
    public float swayAmountA;
    public float swayAmountB;
    public float swayScale;
    public float swayScaleADS;
    public float swayLerpSpeed;
    public float swayTime;
    Vector3 swayPosition;
    Vector3 cameraOriginalPosition = new(0, 0, 0);
    public float lerpSpeed;

    [Header("Sights")]
    public bool isAiming;
    public Transform weaponSight;
    public float sightOffset;
    public float ADSSpeed;
    public float fovOrig;
    public float fovZoomMax;
    public int fovZoomInSpd;
    public int fovZoomOutSpd;
    Vector3 weaponSwayPosition;
    Vector3 weaponSwayPositionVelocity;

    public void Start()
    {
        weaponRotation = transform.localRotation.eulerAngles;
        fovOrig = Camera.main.fieldOfView;
    }

    public void Initialize(PlayerController PlayerController)
    {
        playerController = PlayerController;
        isInitialized = true;
    }

    void Update()
    {
        if (!isInitialized)
        {
            return;
        }

        WeaponRotation();
        SetWeaponAnimation();
        WeaponIdleSway();
        Aiming();
    }

    void Aiming()
    {
        Vector3 targetPosition = transform.position;

        if (isAiming)
        {
            targetPosition = playerController.cam.position + (weaponSway.position - weaponSight.position) + (playerController.cam.forward * sightOffset);
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, fovZoomMax, Time.deltaTime * fovZoomInSpd);
            //cameraSwayADS.localEulerAngles = swayPosition; Needs work. Trying to rotate camera with gun ADS sway.
        }
        else
        {
            cameraSwayADS.localPosition = cameraOriginalPosition;
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, fovOrig, Time.deltaTime * fovZoomOutSpd);
        }

        weaponSwayPosition = weaponSway.position;
        weaponSwayPosition = Vector3.SmoothDamp(weaponSwayPosition, targetPosition, ref weaponSwayPositionVelocity, ADSSpeed);
        weaponSway.position = weaponSwayPosition;
    }

    public void TriggerJump()
    {
        isGroundedTrigger = false;
        weaponAnimator.SetTrigger("Jump");
    }

    void WeaponRotation()
    {
        targetWeaponRotation.y += (isAiming ? settings.swayAmountADS : settings.swayAmount) * (settings.swayXInverted ? -playerController.inputCamera.x : playerController.inputCamera.x); // Horizontal Rotation
        targetWeaponRotation.x += (isAiming ? settings.swayAmountADS : settings.swayAmount) * (settings.swayYInverted ? playerController.inputCamera.y : -playerController.inputCamera.y); // Vertical Rotation

        targetWeaponRotation.x = Mathf.Clamp(targetWeaponRotation.x, -settings.swayClampX, settings.swayClampX);
        targetWeaponRotation.y = Mathf.Clamp(targetWeaponRotation.y, -settings.swayClampY, settings.swayClampY);
        targetWeaponRotation.z = isAiming ? 0 : targetWeaponRotation.y * settings.swayLeanAmount;

        targetWeaponRotation = Vector3.SmoothDamp(targetWeaponRotation, Vector3.zero, ref targetWeaponRotationVelocity, settings.swayResetSmoothing);
        weaponRotation = Vector3.SmoothDamp(weaponRotation, targetWeaponRotation, ref weaponRotationVelocity, settings.swaySmoothing);

        targetWeaponMovementRotation.z = (isAiming ? settings.swayMovementXADS : settings.swayMovementX) * (settings.swayMovementXInverted ? -playerController.inputMovement.x : playerController.inputMovement.x);
        targetWeaponMovementRotation.x = (isAiming ? settings.swayMovementYADS : settings.swayMovementY) * (settings.swayMovementYInverted ? -playerController.inputMovement.y : playerController.inputMovement.y);

        targetWeaponMovementRotation = Vector3.SmoothDamp(targetWeaponMovementRotation, Vector3.zero, ref targetWeaponMovementRotationVelocity, settings.swayMovementSmoothing);
        weaponMovementRotation = Vector3.SmoothDamp(weaponMovementRotation, targetWeaponMovementRotation, ref weaponMovementRotationVelocity, settings.swayMovementSmoothing);

        transform.localRotation = Quaternion.Euler(weaponRotation + weaponMovementRotation);
    }

    void SetWeaponAnimation()
    {
        if (isGroundedTrigger)
        {
            fallingDelay = 0;
        }
        else
        {
            fallingDelay += Time.deltaTime;
        }

        if (playerController.isGrounded && !isGroundedTrigger && fallingDelay > 0.1f)
        {
            weaponAnimator.SetTrigger("Land");
            isGroundedTrigger = true;
        }
        else if (!playerController.isGrounded && isGroundedTrigger)
        {
            weaponAnimator.SetTrigger("Falling");
            isGroundedTrigger = false;
        }

        weaponAnimator.SetBool("IsWalking", playerController.isWalking);
        weaponAnimator.SetBool("IsSprinting", playerController.isSprinting);
        weaponAnimator.SetFloat("WeaponAnimSpeed", playerController.weaponAnimSpeed);
    }

    void WeaponIdleSway()
    {
        Vector3 targetPosition = Curve(swayTime, swayAmountA, swayAmountB) / (isAiming ? swayScaleADS : swayScale);

        swayPosition = Vector3.Lerp(swayPosition, targetPosition, Time.smoothDeltaTime * swayLerpSpeed);
        swayTime += Time.deltaTime;

        if (swayTime > 6.29475f)
        {
            swayTime = 0;
        }

        weaponBreathing.localPosition = swayPosition;
    }

    //void ResetAnimations()
    //{
    //    StartCoroutine(ResetAnimationCoroutine("Walking", lerpSpeed));
    //}

    //IEnumerator ResetAnimationCoroutine(string animationName, float lerpDuration)
    //{
    //    AnimatorStateInfo stateInfo = weaponAnimator.GetCurrentAnimatorStateInfo(0);
    //    float currentNormalizedTime = stateInfo.normalizedTime % 1;

    //    float elapsedTime = 0;
    //    while (elapsedTime < lerpDuration)
    //    {
    //        float newNormalizedTime = Mathf.Lerp(currentNormalizedTime, 0, elapsedTime / lerpDuration);
    //        weaponAnimator.Play(animationName, 0, newNormalizedTime);

    //        elapsedTime += Time.deltaTime;
    //        yield return null;
    //    }

    //    weaponAnimator.Play(animationName, 0, 0);
    //}

    Vector3 Curve(float Time, float A, float B)
    {
        return new Vector3(Mathf.Sin(Time), A * Mathf.Sin(B * Time + Mathf.PI));
    }
}
