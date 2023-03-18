using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    DefaultInput input;
    CharacterController characterController;
    public Vector2 inputMovement;
    public Vector2 inputCamera;
    [SerializeField] float lockVerMin;
    [SerializeField] float lockVerMax;

    Vector3 cameraRotation;
    Vector3 playerRotation;

    [Header("---Preferences---")]
    public Transform mainCamera;
    [SerializeField] public Models.PlayerPose playerPose;
    [SerializeField] public float playerPoseSmooth;
    [SerializeField] public float cameraStandHeight;
    [SerializeField] public float cameraCrouchHeight;
    [SerializeField] public float cameraProneHeight;
    [SerializeField] public float cameraHeight;
    [SerializeField] public float cameraHeightSpeed;

    [Header("---Settings---")]
    public Models.PlayerSettingsModel playerSettings;
    [SerializeField] float gravity;
    [SerializeField] float gravitySpeed;
    [SerializeField] float playerGravity;
    [SerializeField] Vector3 jumpForce;
    Vector3 jumpSpeed;

    void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        input = new();

        input.Player.Movement.performed += e => inputMovement = e.ReadValue<Vector2>();
        input.Player.Camera.performed += e => inputCamera = e.ReadValue<Vector2>();
        input.Player.Jump.performed += e => Jump();

        input.Enable();

        cameraRotation = mainCamera.localRotation.eulerAngles;
        playerRotation = transform.localRotation.eulerAngles;

        characterController = GetComponent<CharacterController>();

        cameraHeight = mainCamera.localPosition.y;
    }

    private void Update()
    {
        Camera();
        Movement();
        Jumping();
        CameraHeight();
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

    void CameraHeight()
    {
        float poseHeight = cameraStandHeight;

        if (playerPose == Models.PlayerPose.Crouch)
        {
            poseHeight = cameraCrouchHeight;
        }
        else if (playerPose == Models.PlayerPose.Prone)
        {
            poseHeight = cameraProneHeight;
        }

        cameraHeight = Mathf.SmoothDamp(mainCamera.localPosition.y, poseHeight, ref cameraHeightSpeed, playerPoseSmooth);
        mainCamera.localPosition = new Vector3(mainCamera.localPosition.x, cameraHeight, mainCamera.localPosition.z);
    }

    void Movement()
    {
        float verticalSpeed = playerSettings.forwardPlayerSpeed * inputMovement.y * Time.deltaTime;
        float horizontalSpeed = playerSettings.strafePlayerSpeed * inputMovement.x * Time.deltaTime;

        Vector3 movementSpeed = new(horizontalSpeed, 0, verticalSpeed);
        movementSpeed = transform.transform.TransformDirection(movementSpeed);

        if (playerGravity > gravitySpeed)
        {
            playerGravity -= gravity * Time.deltaTime;
        }

        if (playerGravity < -0.1f && characterController.isGrounded)
        {
            playerGravity = -0.1f;
        }

        movementSpeed.y += playerGravity;
        movementSpeed += jumpForce * Time.deltaTime;
        characterController.Move(movementSpeed);
    }

    void Jumping()
    {
        jumpForce = Vector3.SmoothDamp(jumpForce, Vector3.zero, ref jumpSpeed, playerSettings.jumpingFalloff);
    }

    void Jump()
    {
        if (!characterController.isGrounded)
        {
            return;
        }

        jumpForce = Vector3.up * playerSettings.jumpingHeight;
        playerGravity = 0;
    }

}
