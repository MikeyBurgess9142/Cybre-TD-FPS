using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public static class Models
{
    public enum PlayerPose
    {
        Stand,
        Crouch,
        Prone
    }

    [System.Serializable]

    public class PlayerSettingsModel
    {
        [Header("---Camera Settings---")]
        public float cameraSensHor;
        public float cameraSensVer;

        public float ADSSensEffector;

        public bool invertX;
        public bool invertY;

        [Header("---Movement---")]
        public bool holdSprint; //----- Used for Toggle to Sprint or Hold to Sprint - True = Hold, False = Toggle.
        public float movementSmoothing;

        [Header("---Walking---")]
        public float forwardWalkSpeed;
        public float backwardWalkSpeed;
        public float strafeWalkSpeed;

        [Header("---Sprinting---")]
        public float forwardSprintSpeed;
        public float strafeSprintSpeed;

        [Header("---Jumping---")]
        public float jumpingHeight;
        public float jumpingFalloff;
        public float fallingSmoothing;
        public Vector3 jumpDirection;
        public bool isJumping;
        public float walkJumpFactor;
        public float backwardJumpFactor;

        [Header("---Speed Effectors---")]
        public float speedEffector;
        public float crouchSpeedEffector;
        public float proneSpeedEffector;
        public float fallingSpeedEffector;
        public float aimSpeedEffector;

        [Header("---Is Grounded / Falling---")]
        public float isGroundedRadius;
        public float isFallingSpeed;
        public float groundDistance;
    }

    [System.Serializable]
    public class PlayerStance
    {
        public float cameraHeight;
        public CapsuleCollider stanceCollider;
    }
}

[Serializable]
public class WeaponSettingsModel
{
    [Header("---Weapon Sway---")]
    public float swayAmount;
    public float swayAmountADS;
    public float swaySmoothing;
    public float swayResetSmoothing;
    public float swayClampX;
    public float swayClampY;
    public float swayLeanAmount;
    public bool swayYInverted;
    public bool swayXInverted;

    [Header("---Weapon Movement---")]
    public float swayMovementX;
    public float swayMovementY;
    public float swayMovementXADS;
    public float swayMovementYADS;
    public bool swayMovementYInverted;
    public bool swayMovementXInverted;
    public float swayMovementSmoothing;
}