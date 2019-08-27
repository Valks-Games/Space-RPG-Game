using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class VoxelRender : MonoBehaviour
{
    private Mesh mesh;

    private List<Vector3> vertices;
    private List<int> triangles;

    public float scale = 1.0f;

    private float adjScale;

    private void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        adjScale = scale * 0.5f;
    }

    public void Start()
    {
        GenerateVoxelMesh( new VoxelData() );
        MeshCollider collider = gameObject.AddComponent<MeshCollider>();
        collider.convex = false;

        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    public void GenerateVoxelMesh(VoxelData data) {
        vertices = new List<Vector3>();
        triangles = new List<int>();

        for (int x = 0; x < data.Depth; x++) {
            for (int y = 0; y < data.Height; y++) {
                for (int z = 0; z < data.Width; z++)
                {
                    if (data.GetCell(x, y, z) == 0)
                    {
                        continue;
                    }

                    CreateCube(adjScale, new Vector3(x * scale, y * scale, z * scale), x, y, z, data);
                }
            }
        }
    }

    public void CreateCube(float cubeScale, Vector3 cubePos, int x, int y, int z, VoxelData data)
    {
        for (int i = 0; i < 6; i++)
        {
            if (data.GetNeighbor(x, y, z, (Direction)i) == 0) {
                CreateFace((Direction)i, cubeScale, cubePos);
            }
        }

        UpdateMesh();
    }

    private void CreateFace(Direction dir, float faceScale, Vector3 facePos)
    {
        vertices.AddRange(CubeMeshData.faceVertices(dir, faceScale, facePos));

        int vCount = vertices.Count;

        triangles.Add(vCount - 4);
        triangles.Add(vCount - 4 + 1);
        triangles.Add(vCount - 4 + 2);
        triangles.Add(vCount - 4);
        triangles.Add(vCount - 4 + 2);
        triangles.Add(vCount - 4 + 3);
    }

    private void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
    }
}
