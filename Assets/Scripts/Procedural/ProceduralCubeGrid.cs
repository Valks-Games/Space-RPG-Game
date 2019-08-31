using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProceduralCubeGrid : MonoBehaviour
{
    public Vector3 GridOffset;
    public float CellSize = 1;
    public int GridSize = 10;

    private Vector3[] _vertices;
    private Mesh _mesh;
    private int[] _triangles;

    private void Awake()
    {
        _mesh = GetComponent<MeshFilter>().mesh;
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
        _vertices = new Vector3[GridSize * GridSize * 4];
        _triangles = new int[GridSize * GridSize * 6];

        float vertexOffset = CellSize * 0.5f; // Multiplication is not as intensive as division.

        for (int x = 0; x < GridSize; x++)
        {
            for (int z = 0; z < GridSize; z++)
            {
                Vector3 cellOffset = new Vector3(x * CellSize, x + z, z * CellSize);

                GameObject cubeObject = new GameObject();
                ProceduralCube cube = cubeObject.AddComponent<ProceduralCube>();
                //cube.CreateCube(cube.scale * 0.5f);
                cubeObject.transform.position = cellOffset;
            }
        }
    }
}
