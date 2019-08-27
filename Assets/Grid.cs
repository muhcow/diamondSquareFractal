using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//code for defining vertices adapted from https://catlikecoding.com/unity/tutorials/procedural-grid/
//code for implementing diamond square algorithm inspired from https://www.youtube.com/watch?v=1HV8GbFnCik

//automatically add mesh filter and mesh renderer
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]

public class Grid : MonoBehaviour
{   
    //size of grid and detail of grid
    public int numDivisions;
    public float terrainSize;

    private Vector3[] vertices;
    private int[] triangles;
    private Vector2[] uvs;

    // Start is called before the first frame update
    void Start()
    {
        //make new mesh
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        int totalVertices = (numDivisions + 1) * (numDivisions + 1);

        //calculate number of vertices and define vertices array size
        vertices = new Vector3[totalVertices];
        uvs = new Vector2[totalVertices];

        //number of triangles = number of squares * 2 *3
        triangles = new int[numDivisions*numDivisions*2*3];

        float divSize = terrainSize / numDivisions;
        int vertexCount = 0;
        int triangleCount = 0;

        //define positions of vertices and uvs
        for (int x=0;x<(numDivisions+1);x++) {
            for(int z = 0; z<(numDivisions + 1); z++) {
                vertices[vertexCount] = new Vector3(x*divSize,0,z*divSize);
                //uvs[vertexCount] = new Vector2((float)x/numDivisions,(float)z/numDivisions);

                //input the triangles
                if (vertexCount % (numDivisions + 1) != numDivisions && vertexCount < totalVertices - (numDivisions + 1))
                {
                    triangles[triangleCount] = vertexCount;
                    triangles[triangleCount + 1] = vertexCount + 1;
                    triangles[triangleCount + 2] = vertexCount + (numDivisions + 2);
                    triangles[triangleCount + 3] = vertexCount ;
                    triangles[triangleCount + 4] = vertexCount + (numDivisions + 2);
                    triangles[triangleCount + 5] = vertexCount + (numDivisions + 1);
                    triangleCount = triangleCount + 6;
                }
                vertexCount++;
            }
        }

        mesh.vertices = vertices;
        //mesh.uv = uvs;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        if (vertices == null)
        {
            return;
        }
        Gizmos.color = Color.black;
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }
}
