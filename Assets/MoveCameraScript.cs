using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//code for updating camera orientation adapted from https://www.youtube.com/watch?v=lYIRm4QEqro

public class MoveCameraScript : MonoBehaviour
{
    public float cameraSpeed;

    private float rotateHorizontalSpeed;
    private float rotateVerticalSpeed;
    private float yaw = 0.0f;
    private float pitch = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        rotateHorizontalSpeed = 3.0f;
        rotateVerticalSpeed = 3.0f;
    }

    // Update is called once per frame
    void Update()
    {   
        //w means go forward
        if (Input.GetKey(KeyCode.W)) {
            this.transform.Translate(new Vector3(0, 0, cameraSpeed * Time.deltaTime));
        }

        //s means go backward
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.Translate(new Vector3(0, 0, -cameraSpeed * Time.deltaTime));
        }

        //d means go right
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.Translate(new Vector3(cameraSpeed * Time.deltaTime, 0, 0));
        }

        //a means go left
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.Translate(new Vector3(-cameraSpeed * Time.deltaTime, 0, 0));
        }

        //update yaw and pitch based on mouse location
        yaw += rotateHorizontalSpeed * Input.GetAxis("Mouse X");
        pitch -= rotateVerticalSpeed * Input.GetAxis("Mouse Y");

        //update camera orientation
        this.transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
    }
}
