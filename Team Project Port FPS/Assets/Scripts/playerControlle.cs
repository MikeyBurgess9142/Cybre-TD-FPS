using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerControlle : MonoBehaviour
{
    [SerializeField] CharacterController controller;

    [SerializeField] float playerSpd;
    [SerializeField] int jumpMax;
    [SerializeField] int jumpSpd;
    [SerializeField] int gravity;

    int jumpsCurr;
    Vector3 move;
    Vector3 playerVeloc;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        movement();
    }

    void movement()
    {
        move = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
        move = move.normalized;
        controller.Move(move * Time.deltaTime * playerSpd);

        if (Input.GetButtonDown("Jump"))
        {
            playerVeloc.y = jumpSpd;
        }

        playerVeloc.y -= gravity * Time.deltaTime;

        controller.Move(playerVeloc * Time.deltaTime);
    }
}
