using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyCamera : MonoBehaviour
{
    public float speed;
    public float angularSpeed;
    private float dy = 10f;
    public Rigidbody rigidbody;
    public GameObject terrainObj;
    // Start is called before the first frame update
    void Start()
    {
        // Lock the cursor to allow mouse look, press esc to get out of scene and use cursor normally
        Cursor.lockState = CursorLockMode.Locked;
        transform.position = new Vector3(0, 20, 0);
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
        Bound();
        rigidbody.velocity = new Vector3(0, 0, 0);
    }

    private void Bound()
    {
        DSTerrain terrain = terrainObj.GetComponent<DSTerrain>();
        Vector3 newPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        float terrainWidth = terrain.findWidth(terrain.nverts)*0.7f;
        float terrainHighPoint = terrain.findMaxHeight(terrain.nverts);

        // Keep the camera inside the terrain's bounds
        if (newPos.x > terrainWidth - dy)
        {
            newPos.x = terrainWidth - dy;
        }
        if (newPos.x < -terrainWidth + dy)
        {
            newPos.x = -terrainWidth + dy;
        }
        if (newPos.z > terrainWidth - dy)
        {
            newPos.z = terrainWidth - dy;
        }
        if (newPos.z < -terrainWidth + dy)
        {
            newPos.z = -terrainWidth + dy;
        }
        if (newPos.y > 3*terrainHighPoint - dy)
               {
                   newPos.y = 3*terrainHighPoint- dy;
               }

        transform.position = newPos;


    }
}
