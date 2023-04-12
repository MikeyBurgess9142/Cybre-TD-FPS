using UnityEditor;
using UnityEditor.ShaderGraph.Internal;
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

    public void Start()
    {
        weaponRotation = transform.localRotation.eulerAngles;
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
    }

    public void TriggerJump()
    {
        isGroundedTrigger = false;
        weaponAnimator.SetTrigger("Jump");
    }

    void WeaponRotation()
    {
        targetWeaponRotation.y += settings.swayAmount * (settings.swayXInverted ? -playerController.inputCamera.x : playerController.inputCamera.x); // Horizontal Rotation
        targetWeaponRotation.x += settings.swayAmount * (settings.swayYInverted ? playerController.inputCamera.y : -playerController.inputCamera.y); // Vertical Rotation

        targetWeaponRotation.x = Mathf.Clamp(targetWeaponRotation.x, -settings.swayClampX, settings.swayClampX);
        targetWeaponRotation.y = Mathf.Clamp(targetWeaponRotation.y, -settings.swayClampY, settings.swayClampY);
        targetWeaponRotation.z = targetWeaponRotation.y * settings.swayLeanAmount;

        targetWeaponRotation = Vector3.SmoothDamp(targetWeaponRotation, Vector3.zero, ref targetWeaponRotationVelocity, settings.swayResetSmoothing);
        weaponRotation = Vector3.SmoothDamp(weaponRotation, targetWeaponRotation, ref weaponRotationVelocity, settings.swaySmoothing);

        targetWeaponMovementRotation.z = settings.swayMovementX * (settings.swayMovementXInverted ? -playerController.inputMovement.x : playerController.inputMovement.x);
        targetWeaponMovementRotation.x = settings.swayMovementY * (settings.swayMovementYInverted ? -playerController.inputMovement.y : playerController.inputMovement.y);

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

        weaponAnimator.SetBool("IsSprinting", playerController.isSprinting);
        weaponAnimator.SetFloat("WeaponAnimSpeed", playerController.weaponAnimSpeed);
    }
}
