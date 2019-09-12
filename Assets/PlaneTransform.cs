using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlaneTransform : MonoBehaviour
{

    public DSTerrain terrain;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //set water plane height relative to terrain height
        float waterHeight = 0.23f * terrain.totalHeight + terrain.findMinHeight(terrain.nverts);
        this.transform.position = new Vector3(0,waterHeight,0);
    }
}
