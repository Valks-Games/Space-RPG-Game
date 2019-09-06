using System.Collections.Generic;
using UnityEngine;

public class RenderChunk : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    private Mesh _mesh;

    private List<Vector3> _vertices;
    private List<int> _triangles;

    private float _adjScale;

    private GameObject _player;

    private World _world;

    private int _chunkSize;
    private float _scale;

    private void Awake()
    {
        _world = GameObject.Find("World").GetComponent<World>();
        _mesh = gameObject.AddComponent<MeshFilter>().mesh;
        _scale = _world.GetScale();
        _adjScale = _scale * 0.5f;
        _chunkSize = _world.GetChunkSize();

        _meshRenderer = gameObject.AddComponent<MeshRenderer>();
        _meshRenderer.material = (Material)Resources.Load("Materials/Terrain");
    }

    private void Start()
    {
        
    }

    public void Render(int chunk_x, int chunk_z)
    {
        GenerateChunkMesh(new VoxelData(chunk_x, chunk_z, _chunkSize, _world.WorldSize));
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
        collider.material = (PhysicMaterial) Resources.Load("Physics Materials/Terrain");
    }

    public void GenerateChunkMesh(VoxelData data) {
        _vertices = new List<Vector3>();
        _triangles = new List<int>();

        int chunkX = data.ChunkX;
        int chunkZ = data.ChunkZ;

        for (int x = _chunkSize * chunkX; x < _chunkSize * (chunkX + 1); x++) {
            for (int y = 0; y < data.Height; y++) {
                for (int z = _chunkSize * chunkZ; z < _chunkSize * (chunkZ + 1); z++)
                {
                    if (data.GetCell(x, y, z) == 0)
                        continue;

                    int renderOffsetX = chunkX * _chunkSize;
                    int renderOffsetZ = chunkZ * _chunkSize;

                    CreateCube(_adjScale, new Vector3(x * _scale - renderOffsetX, y * _scale, z * _scale - renderOffsetZ), x, y, z, data);
                }
            }
        }

        UpdateMesh();
    }

    public void CreateCube(float cubeScale, Vector3 cubePos, int x, int y, int z, VoxelData data)
    {
        for (int i = 0; i < 6; i++)
        {
            if (data.GetNeighbor(x, y, z, (Direction)i) == 0) {
                CreateFace((Direction)i, cubeScale, cubePos);
            }
        }

        
    }

    private void CreateFace(Direction dir, float faceScale, Vector3 facePos)
    {
        _vertices.AddRange(CubeMeshData.FaceVertices(dir, faceScale, facePos));

        int vCount = _vertices.Count;

        _triangles.Add(vCount - 4);
        _triangles.Add(vCount - 4 + 1);
        _triangles.Add(vCount - 4 + 2);
        _triangles.Add(vCount - 4);
        _triangles.Add(vCount - 4 + 2);
        _triangles.Add(vCount - 4 + 3);
    }

    private void UpdateMesh()
    {
        _mesh.Clear();
        _mesh.vertices = _vertices.ToArray();
        _mesh.triangles = _triangles.ToArray();
        _mesh.RecalculateNormals();
    }
}
