using UnityEngine;

public class Planet : MonoBehaviour
{
    public int x, y, z;
    public int radius;

    [Range(0, 1)]
    public int Subdivisions = 0;
    public float TestOffset = 0;

    [Range(2, 256)]
    public int Resolution = 10;
    public bool AutoUpdate = true;

    public ShapeSettings ShapeSettings;
    public ColourSettings ColourSettings;

    [HideInInspector]
    public bool ShapeSettingsFoldout;
    [HideInInspector]
    public bool ColourSettingsFoldout;

    private ShapeGenerator _shapeGenerator = new ShapeGenerator();
    private ColourGenerator _colourGenerator = new ColourGenerator();

    [SerializeField, HideInInspector]
    private MeshFilter[,] _meshFilters;
    private TerrainFace[,] _terrainFaces;
    private MeshCollider[,] _meshColliders;

    private int _subdivisions;
    private int _divisions;

    public void Awake()
    {
        GeneratePlanet(new Vector3(x, y, z), radius);
    }

    public void GeneratePlanet()
    {
        Initialize();
        GenerateMesh();
        GenerateColours();
    }

    public void GeneratePlanet(Vector3 loc, int radius)
    {
        //ColourSettings.Gradient = 
        ShapeSettings.PlanetRadius = radius;
        GeneratePlanet();
        transform.Translate(loc);
    }

    private void Initialize()
    {
        _divisions = (int) Mathf.Pow(4, Subdivisions);

        _shapeGenerator.UpdateSettings(ShapeSettings);
        _colourGenerator.UpdateSettings(ColourSettings);
        if (_meshFilters == null || _meshFilters.Length == 0 || _subdivisions != Subdivisions) {
            _meshFilters = new MeshFilter[6, _divisions];
        }

        _meshColliders = new MeshCollider[6, _divisions];

        _subdivisions = Subdivisions;

        _terrainFaces = new TerrainFace[6, _divisions];

        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };
        Vector2[] offsets = {
            new Vector2(1, 1),
            new Vector2(0, 1),
            new Vector2(1, 0),
            new Vector2(0, 0)
        };

        for (int d = 0; d < 6; d++)
        {
            // Remember arrays count 0 as a index. That is why we subtract 1 from divisions.
            GenerateFaces(directions, offsets, d, _divisions - 1);
        }
    }

    private void GenerateFaces(Vector3[] directions, Vector2[] offsets, int d, int divisions) {
        if (divisions < 0) {
            return;
        }

        if (_meshFilters[d, divisions] == null)
        {
            GameObject meshObj = new GameObject("Chunk");
            meshObj.transform.parent = transform;

            meshObj.AddComponent<MeshRenderer>();
            _meshFilters[d, divisions] = meshObj.AddComponent<MeshFilter>();
            _meshFilters[d, divisions].sharedMesh = new Mesh();

            MeshCollider collider = meshObj.AddComponent<MeshCollider>();
            _meshColliders[d, divisions] = collider;
            collider.convex = false;
        }
        _meshFilters[d, divisions].GetComponent<MeshRenderer>().sharedMaterial = ColourSettings.PlanetMaterial;

        // Size of 0.125 has offset of 6.5f, 6.5f
        // Size of 0.25 has offset of 2.5f, 2.5f
        // Size of 0.5 has offset of 0.5f, 0.5f
        // Size of 1 has offset of -0.5f, -0.5f
        // Size of 2 has offset of -1f, -1f
        Vector2 offset = offsets[divisions % 4] + new Vector2(-1.5f + Mathf.Sqrt(_divisions) / 2, -1.5f + Mathf.Sqrt(_divisions) / 2);

        Mesh mesh = _meshFilters[d, divisions].sharedMesh;
        
        //float size = 0.125f;
        float size = 2.0f / Mathf.Sqrt(_divisions);

        TerrainFace terrainFace = new TerrainFace(_shapeGenerator, mesh, Resolution, directions[d], offset, size);
        _terrainFaces[d, divisions] = terrainFace;

        GenerateFaces(directions, offsets, d, divisions - 1);
    }

    public void OnShapeSettingsUpdated()
    {
        if (AutoUpdate)
        {
            Initialize();
            GenerateMesh();
        }
    }

    public void OnColourSettingsUpdated()
    {
        if (AutoUpdate)
        {
            Initialize();
            GenerateColours();
        }
    }

    private void GenerateMesh()
    {
        foreach (TerrainFace face in _terrainFaces)
        {
            face.ConstructMesh(transform);
        }

        _colourGenerator.UpdateElevation(_shapeGenerator.ElevationMinMax);
    }

    private void GenerateColours()
    {
        _colourGenerator.UpdateColours();
    }
}
