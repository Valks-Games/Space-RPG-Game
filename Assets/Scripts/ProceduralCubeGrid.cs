using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProceduralCubeGrid : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;

    public float cellSize = 1;
    public Vector3 gridOffset;
    public int gridSize = 10;

    void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
    }

    void Start()
    {
        MakeDiscreteProceduralCubeGrid();
    }

    void MakeDiscreteProceduralCubeGrid()
    {
        vertices = new Vector3[gridSize * gridSize * 4];
        triangles = new int[gridSize * gridSize * 6];

        float vertexOffset = cellSize * 0.5f; // Multiplication is not as intensive as division.

        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                Vector3 cellOffset = new Vector3(x * cellSize, x + z, z * cellSize);

                GameObject cubeObject = new GameObject();
                ProceduralCube cube = cubeObject.AddComponent<ProceduralCube>();
                cube.CreateCube(vertexOffset);
                cubeObject.transform.position = cellOffset;
            }
        }
    }
}
