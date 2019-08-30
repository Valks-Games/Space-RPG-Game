using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderChunk : MonoBehaviour
{
    private float scale = 1.0f;
    private int chunk_size = 10;

    private MeshRenderer meshRenderer;
    private Mesh mesh;

    private List<Vector3> vertices;
    private List<int> triangles;

    private float adjScale;

    private GameObject player;

    private World world;

    private void Awake()
    {
        mesh = gameObject.AddComponent<MeshFilter>().mesh;

        adjScale = scale * 0.5f;

        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = (Material)Resources.Load("Materials/Terrain");

        world = GameObject.Find("World").GetComponent<World>();
    }

    private void Start()
    {
        
    }

    public void Render(int chunk_x, int chunk_z)
    {
        GenerateChunkMesh(new VoxelData(chunk_x, chunk_z, chunk_size, world.world_size, world.frequency, world.seed.GetHashCode()));
        ReloadMeshCollider();
    }

    private void ReloadMeshCollider() {
        if (!GetComponent<MeshCollider>())
        {
            InitMeshCollider();
        }
        else
        {
            Destroy(GetComponent<MeshCollider>());

            InitMeshCollider();
        }
    }

    private void InitMeshCollider() {
        MeshCollider collider = gameObject.AddComponent<MeshCollider>();
        collider.convex = false;
    }

    public void GenerateChunkMesh(VoxelData data) {
        vertices = new List<Vector3>();
        triangles = new List<int>();

        for (int x = 0; x < data.Depth; x++) {
            for (int y = 0; y < data.Height; y++) {
                for (int z = 0; z < data.Width; z++)
                {
                    if (data.GetCell(x, y, z) == 0)
                        continue;

                    CreateCube(adjScale, data.ChunkX, data.ChunkZ, new Vector3(x * scale, y * scale, z * scale), x, y, z, data);
                }
            }
        }

        UpdateMesh(data.ChunkX, data.ChunkZ);
    }

    public void CreateCube(float cubeScale, int chunk_x, int chunk_z, Vector3 cubePos, int x, int y, int z, VoxelData data)
    {
        for (int i = 0; i < 6; i++)
        {
            if (data.GetNeighbor(x, y, z, (Direction)i) == 0) {
                CreateFace(chunk_x, chunk_z, (Direction)i, cubeScale, cubePos);
            }
        }

        
    }

    private void CreateFace(int chunk_x, int chunk_z, Direction dir, float faceScale, Vector3 facePos)
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

    private void UpdateMesh(int chunk_x, int chunk_z)
    {
        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
    }
}
