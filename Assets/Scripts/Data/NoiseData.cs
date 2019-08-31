using UnityEngine;

public class NoiseData
{
    private float _seed;

    private float _frequency;
    private float _amplitude;

    private float _lacunarity;
    private float _persistance;

    private int _octaves;

    public NoiseData(float seed, float frequency, float amplitude, float lacunarity, float persistance, int octaves)
    {
        _seed = seed;
        _frequency = frequency;
        _amplitude = amplitude;
        _lacunarity = lacunarity;
        _persistance = persistance;
        _octaves = octaves;
    }

    public float[,] GetNoiseValues(int worldSize, int noiseSize)
    {
        float[,] noiseValues = new float[worldSize, worldSize];

        float max = 0f;
        float min = float.MaxValue;

        _seed = _seed * 0.00000001f;

        for (int i = 0; i < worldSize; i++)
        {
            for (int j = 0; j < worldSize; j++)
            {

                noiseValues[i, j] = 0f;

                float tempA = _amplitude;
                float tempF = _frequency;

                for (int k = 0; k < _octaves; k++)
                {
                    noiseValues[i, j] += Mathf.PerlinNoise((i + _seed) / noiseSize * _frequency, (j + _seed) / noiseSize * _frequency) * _amplitude;
                    _frequency *= _lacunarity;
                    _amplitude *= _persistance;
                }

                _amplitude = tempA;
                _frequency = tempF;

                if (noiseValues[i, j] > max)
                    max = noiseValues[i, j];

                if (noiseValues[i, j] < min)
                    min = noiseValues[i, j];
            }
        }

        for (int i = 0; i < worldSize; i++)
        {
            for (int j = 0; j < worldSize; j++)
            {
                noiseValues[i, j] = Mathf.InverseLerp(max, min, noiseValues[i, j]);
            }
        }

        return noiseValues;

    }
}
