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
        move = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
        controller.Move(move * Time.deltaTime * playerSpd);
    }
}
