using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{
    public float sunSpeed;
    public GameObject terrain;

    // assume terrain is always built around origin
    void Update()
    {
        transform.RotateAround(new Vector3(0, 0, 0), Vector3.forward, sunSpeed * Time.deltaTime);
    }
    public Vector3 GetWorldPosition()
    {
        return this.transform.position;
    }
}
