using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DSTerrain : MonoBehaviour
{
    public int ndivisions;
    public float maxsize;
    public float maxheight;

    public Shader shader;

    Vector3[] nverts;
    int nvertcount;

    public GameObject sun;
    public GameObject camera;
    public Renderer renderer;
    public PointLight pointLight;

    // Start is called before the first frame update
    void Start()
    {
        CreateTerrain();
    }

    private void Update()
    {
        // Pass updated light positions to shader
        renderer = GetComponent<Renderer>();
        renderer.material.SetColor("_PointLightColor", pointLight.color);
        renderer.material.SetVector("_PointLightPosition", pointLight.GetWorldPosition());

    }

    void CreateTerrain()
    {
        // need to get the number of vertices, use a 1D array to represent whole terrain
        nvertcount = (ndivisions + 1) * (ndivisions + 1);
        nverts = new Vector3[nvertcount];
        Vector2[] uvs = new Vector2[nvertcount];
        int[] triangles = new int[ndivisions * ndivisions * 6];

        // get size of divisions and half the size
        float halfsize = maxsize * .5f;
        float divisionsize = maxsize / ndivisions;

        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        int trioffset = 0;

        // setup the triangles, use offset to get to next pair of triangles
        for(int i=0; i <= ndivisions; i++)
        {
            for(int j = 0; j <= ndivisions; j++)
            {
                nverts[i * (ndivisions + 1) + j] = new Vector3(-halfsize + j * divisionsize, 0.0f, halfsize - i * divisionsize);
                uvs[i * (ndivisions + 1) + j] = new Vector2((float)i / ndivisions, (float)j / ndivisions);

                if(i < ndivisions && j < ndivisions)
                {
                    int topleft = i * (ndivisions + 1) + j;
                    int botleft = (i + 1) * (ndivisions + 1) + j;

                    triangles[trioffset] = topleft;
                    triangles[trioffset + 1] = topleft + 1;
                    triangles[trioffset + 2] = botleft + 1;

                    triangles[trioffset + 3] = topleft;
                    triangles[trioffset + 4] = botleft + 1;
                    triangles[trioffset + 5] = botleft;

                    trioffset += 6;
                }
            }
        }

        // Set the corner values before starting Diamond square
        nverts[0].y = Random.Range(-maxheight, maxheight);
        nverts[ndivisions].y = Random.Range(-maxheight, maxheight);
        nverts[nverts.Length-1].y = Random.Range(-maxheight, maxheight);
        nverts[nverts.Length-1 -ndivisions].y = Random.Range(-maxheight, maxheight);

        // calculate number of times we need to iterate over diamond square
        // Unity has a limit if we have ndivisions > 128 program seems to crash
        int iterations = (int)Mathf.Log(ndivisions, 2);
        int nsquares = 1;
        int squaresize = ndivisions;

        for(int i=0; i < iterations; i++)
        {
            int row = 0;
            for(int j = 0; j < nsquares; j++)
            {
                int col = 0;
                for (int k = 0; k < nsquares; k++)
                {
                    DiamondSquare(row, col, squaresize, maxheight);
                    col += squaresize;
                }
                row += squaresize;
            }
            nsquares *= 2;
            squaresize /= 2;
            maxheight *= 0.5f;
        }

        mesh.vertices = nverts;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        //make new color array and add colours to it based on hieghts of vertices
        Color[] colourArray = new Color[nverts.Length];

        //calculate relative color heights based off max and min hieght of map
        float totalHeight = findMaxHeight(nverts) - findMinHeight(nverts);
        float snowHeight = 0.9f * totalHeight + findMinHeight(nverts);
        float dirtHeight = 0.6f * totalHeight + findMinHeight(nverts);
        float grassHeight = 0.5f * totalHeight + findMinHeight(nverts);

        print(snowHeight);
        print(dirtHeight);
        print(grassHeight);
        print(totalHeight);
        print(findMaxHeight(nverts));
        print(findMinHeight(nverts));



        for (int h=0;h< nverts.Length; h++)
        {
            //assign colours to each vertex based on height
            if (nverts[h].y >= snowHeight)
            {
                colourArray[h] = Color.white;
            }
            else if (nverts[h].y >= dirtHeight)
            {
                colourArray[h] = new Vector4(0.57254f, 0.38431f, 0.22353f, 1); ;
            }
            else if (nverts[h].y >= grassHeight)
            {
                colourArray[h] = new Vector4(0.09804f, 0.54902f, 0.09804f, 1); ;
            }
            else if (nverts[h].y < grassHeight)
            {
                colourArray[h] = new Vector4(0.96863f, 0.9451f, 0.7451f, 1); ;
            }
        }

        mesh.colors = colourArray;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }

    void DiamondSquare (int row, int col, int size, float offset)
    {
        // need to get the corners, and middle element of each iteraction
        int halfsize = (int)(size * 0.5f);
        int topleft = row * (ndivisions + 1) + col;
        int botleft = (row + size) * (ndivisions + 1) + col;

        // Square Step: sum corners and take average plus add random value
        int mid = (int)(row + halfsize) * (ndivisions + 1) + (int)(col + halfsize);
        nverts[mid].y = (nverts[topleft].y + nverts[topleft + size].y + nverts[botleft].y + nverts[botleft + size].y) * 0.25f + Random.Range(-offset, offset);

        // Diamond Step: sum middle and corners take average plus add random value
        nverts[topleft + halfsize].y = (nverts[topleft].y + nverts[topleft + size].y + nverts[mid].y) / 3 + Random.Range(-offset, offset);
        nverts[mid - halfsize].y = (nverts[topleft].y + nverts[botleft].y + nverts[mid].y) / 3 + Random.Range(-offset, offset);
        nverts[mid + halfsize].y = (nverts[topleft + size].y + nverts[botleft + size].y + nverts[mid].y) / 3 + Random.Range(-offset, offset);
        nverts[botleft + halfsize].y = (nverts[botleft].y + nverts[botleft + size].y + nverts[mid].y) / 3 + Random.Range(-offset, offset);


    }

    float findMaxHeight (Vector3[] verts) {

        float maxHeight = float.MinValue;

        for (int i = 0; i < verts.Length; i++) {
            if (verts[i].y > maxHeight) {
                maxHeight = verts[i].y; 
            }
        }
        return maxHeight;
    }

    float findMinHeight(Vector3[] verts)
    {

        float minHeight = float.MaxValue;

        for (int i = 0; i < verts.Length; i++)
        {
            if (verts[i].y < minHeight)
            {
                minHeight = verts[i].y;
            }
        }
        return minHeight;
    }

}
