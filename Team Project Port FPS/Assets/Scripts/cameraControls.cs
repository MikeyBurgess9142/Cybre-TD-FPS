using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraControls : MonoBehaviour
{
    [SerializeField] int sensHor;
    [SerializeField] int sensVer;

    [SerializeField] int lockVerMin;
    [SerializeField] int lockVerMax;

    [SerializeField] bool invertY;

    float xRotation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Get Input
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensVer;
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensHor;

        if (invertY)
        {
            xRotation += mouseY;
        }
        else
        {
            xRotation -= mouseY;
        }
        //Clamp Camera Rotation
        xRotation = Mathf.Clamp(xRotation, lockVerMin, lockVerMax);
        //Rotate the Camera on the X-Axis
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        //Rotate the Player on its Y-Axis
        transform.parent.Rotate(Vector3.up * mouseX);
    }
}
