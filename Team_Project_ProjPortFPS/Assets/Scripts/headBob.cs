using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class headBob : MonoBehaviour
{
    [SerializeField] bool enableHeadBob;

    [SerializeField, Range(0, 0.1f)] float amplitude;
    [SerializeField, Range(0, 30)] float frequency;

    [SerializeField] Transform cam = null;
    [SerializeField] Transform cameraHolder = null;

    [SerializeField] float toggleSpeed;
    Vector3 startPos;
    CharacterController playerController;

    void Awake()
    {
        playerController = GetComponent<CharacterController>();
        startPos = cam.localPosition;
    }

    void FixedUpdate()
    {
        if (enableHeadBob)
        {
            CheckMotion();
            cam.LookAt(FocusTarget());
        }    
    }

    void CheckMotion()
    {
        ResetPosition();

        float speed = new Vector3(playerController.velocity.x, 0, playerController.velocity.z).magnitude;

        if (speed < toggleSpeed)
        {
            return;
        }

        if (!gameManager.instance.playerControllerScript.isGrounded)
        {
            return;
        }

        PlayMotion(FootStepMotion());
    }

    Vector3 FootStepMotion()
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * frequency) * amplitude;
        pos.x += Mathf.Cos(Time.time * frequency / 2) * amplitude * 2;
        return pos;
    }

    void ResetPosition()
    {
        if (cam.localPosition == startPos)
        {
            return;
        }

        cam.localPosition = Vector3.Lerp(cam.localPosition, startPos, 1 * Time.fixedDeltaTime);
    }

    void PlayMotion(Vector3 motion)
    {
        cam.localPosition += motion;
    }

    Vector3 FocusTarget()
    {
        Vector3 pos = new(transform.position.x, transform.position.y, transform.position.z);
        pos += cameraHolder.forward * 15.0f;
        return pos;
    }
}
