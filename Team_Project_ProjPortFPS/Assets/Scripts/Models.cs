using System.Collections;
using System.Collections.Generic;
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

        public bool invertX;
        public bool invertY;

        [Header("---Movement---")]
        public float forwardPlayerSpeed;
        public float backwardPlayerSpeed;
        public float strafePlayerSpeed;

        [Header("---Jumping---")]
        public float jumpingHeight;
        public float jumpingFalloff;

    }

    [System.Serializable]
    public class PlayerStance
    {
        public float cameraHeight;
        public float playerHeight;
        public CharacterController playerController;
    }
}
