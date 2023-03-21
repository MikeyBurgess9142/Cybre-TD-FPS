using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    DefaultInput input;
    [SerializeField] CharacterController characterController;
    CharacterController characterControllerOrig;
    public Vector2 inputMovement;
    public Vector2 inputCamera;
    [SerializeField] float lockVerMin;
    [SerializeField] float lockVerMax;

    Vector3 cameraRotation;
    Vector3 playerRotation;

    [Header("---Preferences---")]
    public Transform mainCamera;
    [SerializeField] Models.PlayerPose playerPose;
    [SerializeField] float playerPoseSmooth;
    [SerializeField] float cameraHeight;
    [SerializeField] float cameraHeightSpeed;
    [SerializeField] Models.PlayerStance playerStandStance;
    [SerializeField] Models.PlayerStance playerCrouchStance;
    [SerializeField] Models.PlayerStance playerProneStance;
    float playerStanceHeightVelocity;
    Vector3 playerStanceCenterVelocity;

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
        input.Player.Crouch.performed += e => Crouch();
        input.Player.Prone.performed += e => Prone();

        input.Enable();

        cameraRotation = mainCamera.localRotation.eulerAngles;
        playerRotation = transform.localRotation.eulerAngles;

        characterControllerOrig = characterController;

        cameraHeight = mainCamera.localPosition.y;
    }

    private void Update()
    {
        Camera();
        Movement();
        Jumping();
        PlayerStance();
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

        characterController.height = Mathf.SmoothDamp(characterController.height, currentStance.playerHeight, ref playerStanceHeightVelocity, playerPoseSmooth);
        characterController.center = Vector3.SmoothDamp(characterController.center, currentStance.playerController.center, ref playerStanceCenterVelocity, playerPoseSmooth);
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


    void Crouch()
    {
        if (playerPose == Models.PlayerPose.Crouch)
        {
            playerPose = Models.PlayerPose.Stand;
            return;
        }

        playerPose = Models.PlayerPose.Crouch;
    }

    void Prone()
    {
        if (playerPose == Models.PlayerPose.Prone)
        {
            playerPose = Models.PlayerPose.Stand;
            return;
        }

        playerPose = Models.PlayerPose.Prone;
    }
}
