using UnityEngine;

public class TerrainFace {
    private ShapeGenerator _shapeGenerator;
    private Mesh _mesh;
    private int _resolution;
    private Vector3 _localUp;
    private Vector3 _axisA;
    private Vector3 _axisB;
    private Vector2 _offset;
    private float _faceSize;

    public TerrainFace(ShapeGenerator shapeGenerator, Mesh mesh, int resolution, Vector3 localUp, Vector2 offset, float faceSize)
    {
        _shapeGenerator = shapeGenerator;
        _mesh = mesh;
        _resolution = resolution;
        _localUp = localUp;
        _offset = offset;
        _faceSize = faceSize;

        _axisA = new Vector3(localUp.y, localUp.z, localUp.x);
        _axisB = Vector3.Cross(localUp, _axisA);
    }

    public void ConstructMesh(Transform transform)
    {
        Vector3[] vertices = new Vector3[_resolution * _resolution];
        int[] triangles = new int[(_resolution - 1) * (_resolution - 1) * 6];
        int triIndex = 0;

        for (int y = 0; y < _resolution; y++)
        {
            for (int x = 0; x < _resolution; x++)
            {
                int i = x + y * _resolution;
                Vector2 percent = new Vector2(x, y) / (_resolution - 1);

                Vector3 pointOnUnitCube = _localUp + (percent.x - 0.5f - _offset.x) * _faceSize * _axisA + (percent.y - 0.5f - _offset.y) * _faceSize * _axisB;
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;

                vertices[i] = _shapeGenerator.CalculatePointOnPlanet(pointOnUnitSphere, transform.position);

                if (x != _resolution - 1 && y != _resolution - 1)
                {
                    triangles[triIndex] = i;
                    triangles[triIndex + 1] = i + _resolution + 1;
                    triangles[triIndex + 2] = i + _resolution;

                    triangles[triIndex + 3] = i;
                    triangles[triIndex + 4] = i + 1;
                    triangles[triIndex + 5] = i + _resolution + 1;
                    triIndex += 6;
                }
            }
        }

        _mesh.Clear();
        _mesh.name = "Chunk";
        _mesh.vertices = vertices;
        _mesh.triangles = triangles;
        _mesh.normals = vertices;
    }
}
