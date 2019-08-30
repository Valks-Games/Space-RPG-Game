using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProceduralCubeGrid : MonoBehaviour
{
    public Vector3 gridOffset;
    public float cellSize = 1;
    public int gridSize = 10;

    private Vector3[] vertices;
    private Mesh mesh;
    private int[] triangles;

    private void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
    }

    private void Start()
    {
        Test();
        //MakeDiscreteProceduralCubeGrid();
    }

    private void Test() {
        ProceduralCube cube = gameObject.AddComponent<ProceduralCube>();
    }

    public void MakeDiscreteProceduralCubeGrid()
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
                //cube.CreateCube(cube.scale * 0.5f);
                cubeObject.transform.position = cellOffset;
            }
        }
    }
}
