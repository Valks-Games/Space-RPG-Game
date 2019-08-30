using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProceduralCube : MonoBehaviour
{
    public float scale = 1.0f;
    public int posX, posY, posZ;

    private Mesh mesh;

    private List<Vector3> vertices;
    private List<int> triangles;

    private void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        //adjScale = scale * 0.5f;
    }

    private void Start()
    {
        // CreateCube(scale * 0.5f, new Vector3(posX * scale, posY * scale, posZ * scale));
    }

    public void CreateCube(float scale) {
        CreateCube(scale * 0.5f, new Vector3(posX * scale, posY * scale, posZ * scale));
    }

    public void CreateCube(float cubeScale, Vector3 cubePos) {
        vertices = new List<Vector3>();
        triangles = new List<int>();

        for (int i = 0; i < 6; i++) {
            CreateFace(i, cubeScale, cubePos);
        }

        UpdateMesh();
    }

    private void CreateFace(int dir, float faceScale, Vector3 facePos) {
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
