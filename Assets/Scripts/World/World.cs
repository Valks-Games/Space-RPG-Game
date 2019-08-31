using UnityEngine;

public class World : MonoBehaviour
{
    public int ChunkX = 0;
    public int ChunkZ = 0;
    public int WorldSize = 20;
    public int NoiseSize = 20;
    public float Frequency = 4f;
    public float Amplitude = 2f;
    public float Lacunarity = 0.6f;
    public float Persistance = 0.6f;
    public int Octaves = 1;
    public string Seed = "cat";

    private int _chunkSize = 16;
    private float _scale = 1.0f;

    public GameObject Player;

    private float[,] _worldNoise;

    public void Awake()
    {
        GenerateWorldNoise();
        // GenerateSpawn(2, 2);
        SpawnPlayer(true);
    }

    private void GenerateSpawn(int width, int length) {
        for (int x = 0; x < width; x++) {
            for (int z = 0; z < length; z++) {
                GameObject chunk = new GameObject("Chunk " + x + " " + z);
                chunk.transform.parent = gameObject.transform;
                chunk.transform.position = new Vector3(x * _chunkSize, 0, z * _chunkSize);
                RenderChunk renderChunk = chunk.AddComponent<RenderChunk>();
                renderChunk.Render(x, z);
            }
        }
    }

    public float[,] GetWorldNoise() {
        return _worldNoise;
    }

    public int GetChunkSize() {
        return _chunkSize;
    }

    public float GetScale() {
        return _scale;
    }

    private void GenerateWorldNoise()
    {
        NoiseData noiseData = new NoiseData(Seed.GetHashCode(), Frequency, Amplitude, Lacunarity, Persistance, Octaves);
        _worldNoise = noiseData.GetNoiseValues(WorldSize, NoiseSize);
    }

    private void SpawnPlayer(bool checkDupe)
    {
        if (Player)
        {
            Destroy(Player);
            SpawnPlayer();
        }
        else
        {
            SpawnPlayer();
        }
    }

    private void SpawnPlayer()
    {
        // Create and spawn the player.
        GameObject playerPrefab = (GameObject)Resources.Load("Prefabs/Player");
        Player = Instantiate(playerPrefab, new Vector3(10, 21, 10), Quaternion.identity);

        InitPlayerCamera();
    }

    private void InitPlayerCamera()
    {
        // Create and attach the camera component.
        GameObject camObject = new GameObject("Camera");
        Camera camera = camObject.AddComponent<Camera>();
        Transform cameraTransform = camera.transform;
        Vector3 offset = Player.transform.position;
        cameraTransform.Translate(offset + new Vector3(0, 25, 0));
        cameraTransform.Rotate(new Vector3(90, 0, 0));
        camObject.transform.parent = Player.transform;
    }
}
