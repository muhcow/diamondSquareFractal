using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlaneTransform : MonoBehaviour
{


    public DSTerrain terrain;
    public Renderer renderer;
    public PointLight pointLight;

    private float scaleToTerrainWidthRatio = 6.2f;

    // Start is called before the first frame update
    void Start()
    {
        MeshFilter _filter = (MeshFilter)gameObject.GetComponent("MeshFilter");
    }

    // Update is called once per frame
    void Update()
    {
        float terrainWidth = terrain.findWidth(terrain.nverts);
        //set water plane height relative to terrain height
        float waterHeight = 0.24f * terrain.totalHeight + terrain.findMinHeight(terrain.nverts);
        this.transform.position = new Vector3(0,waterHeight,0);
        transform.localScale = new Vector3(terrainWidth/scaleToTerrainWidthRatio, 1, terrainWidth / scaleToTerrainWidthRatio);
        //print(terrainWidth / scaleToTerrainWidthRatio);
        SetColour();

        //pass information for phong shader
        renderer = GetComponent<Renderer>();
        renderer.material.SetColor("_PointLightColor", pointLight.color);
        renderer.material.SetVector("_PointLightPosition", pointLight.GetWorldPosition());
    }

    //set color of terrain
    void SetColour() {

        MeshFilter _filter = (MeshFilter)gameObject.GetComponent("MeshFilter");
        Mesh mesh = _filter.sharedMesh;
        GetComponent<MeshFilter>().mesh = mesh;

        int numVerts = mesh.vertices.Length;

        Color[] colourArray = new Color[numVerts];

        for (int i = 0; i < numVerts; i++) {
            colourArray[i] = Color.blue; 
        }

        mesh.colors = colourArray;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

    }
}
