using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDirection : MonoBehaviour
{
    // direct light to origin
    void Update()
    {
        transform.LookAt(Vector3.zero);
    }
}
