using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public int chunk_x = 0;
    public int chunk_z = 0;
    public int world_size = 20;
    public float frequency = 4f;
    public float amplitude = 2f;
    public float lacunarity = 0.6f;
    public float persistance = 0.6f;
    public int octaves = 1;
    public string seed = "cat";

    public GameObject player;

    private float[,] world_noise;

    public void Start()
    {
        GenerateWorldNoise();
    }

    public float[,] GetWorldNoise() {
        return world_noise;
    }

    private void GenerateWorldNoise()
    {
        NoiseData noiseData = new NoiseData(seed.GetHashCode(), frequency, amplitude, lacunarity, persistance, octaves);
        world_noise = noiseData.GetNoiseValues(world_size, world_size);
    }
}
