using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProceduralGrid : MonoBehaviour
{
    public float CellSize = 1;
    public Vector3 GridOffset;
    public int GridSize = 10;

    private Mesh _mesh;

    private Vector3[] _vertices;
    private int[] _triangles;

    private void Awake()
    {
        _mesh = GetComponent<MeshFilter>().mesh;
    }

    private void Start()
    {
        MakeDiscreteProceduralGrid(); // Discrete means no 2 vertices are sharing, this allows for sharp edges.
        UpdateMesh();
    }

    public void MakeDiscreteProceduralGrid() {
        _vertices = new Vector3[GridSize * GridSize * 4];
        _triangles = new int[GridSize * GridSize * 6];

        int v = 0;
        int t = 0;

        float vertexOffset = CellSize * 0.5f; // Multiplication is not as intensive as division.

        for (int x = 0; x < GridSize; x++) {
            for (int z = 0; z < GridSize; z++) {
                Vector3 cellOffset = new Vector3(x * CellSize, 0, z * CellSize);

                _vertices[v] = new Vector3(-vertexOffset, 0, -vertexOffset) + cellOffset + GridOffset;
                _vertices[v+1] = new Vector3(-vertexOffset, 0, vertexOffset) + cellOffset + GridOffset;
                _vertices[v+2] = new Vector3(vertexOffset, 0, -vertexOffset) + cellOffset + GridOffset;
                _vertices[v+3] = new Vector3(vertexOffset, 0, vertexOffset) + cellOffset + GridOffset;

                _triangles[t] = v;
                _triangles[t+1] = _triangles[t+4] = v+1;
                _triangles[t+2] = _triangles[t+3] = v+2;
                _triangles[t+5] = v+3;

                v += 4;
                t += 6;
            }
        }
    }

    private void UpdateMesh() {
        _mesh.Clear();
        _mesh.vertices = _vertices;
        _mesh.triangles = _triangles;
        _mesh.RecalculateNormals();
    }
}
