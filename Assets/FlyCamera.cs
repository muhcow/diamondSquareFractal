using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyCamera : MonoBehaviour
{
    public float speed;
    public float angularSpeed;
    // Start is called before the first frame update
    void Start()
    {
        // Lock the cursor to allow mouse look, press esc to get out of scene and use cursor normally
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // Movement
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.rotation * new Vector3(0, 0, speed) * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += transform.rotation * new Vector3(0, 0, -speed) * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += transform.rotation * new Vector3(-speed, 0, 0) * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += transform.rotation * new Vector3(speed, 0, 0) * Time.deltaTime;
        }

        // make a quaternion that has no rotation assigned, perform transforms on this to get rotation
        Quaternion delta = Quaternion.identity;

        // Controlling the pitch
        float pitch = Input.GetAxis("Mouse Y");
        delta *= Quaternion.AngleAxis(angularSpeed * Time.deltaTime * pitch, -Vector3.right);
        // Controlling the yaw
        float yaw = Input.GetAxis("Mouse X");
        delta *= Quaternion.AngleAxis(angularSpeed * Time.deltaTime * yaw,
                                                   Vector3.up);

        // Rotate camera around axis, easier to realign to be flat on xy plane (look perpindicular to z)
        if (Input.GetKey(KeyCode.Q))
        {
            delta *= Quaternion.AngleAxis(angularSpeed * Time.deltaTime, Vector3.forward);
        }
        if (Input.GetKey(KeyCode.E))
        {
            delta *= Quaternion.AngleAxis(-angularSpeed * Time.deltaTime, Vector3.forward);
        }

        transform.rotation *= delta;
    }
}
