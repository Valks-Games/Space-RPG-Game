using UnityEngine;

public class SimpleNoiseFilter : INoiseFilter
{
    private NoiseSettings.SimpleNoiseSettings _settings;
    private Noise _noise = new Noise();

    public SimpleNoiseFilter(NoiseSettings.SimpleNoiseSettings settings)
    {
        _settings = settings;
    }

    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0;
        float frequency = _settings.BaseRoughness;
        float amplitude = 1;

        for (int i = 0; i < _settings.NumLayers; i++)
        {
            float v = _noise.Evaluate(point * frequency + _settings.Centre);
            noiseValue += (v + 1) * 0.5f * amplitude;
            frequency *= _settings.Roughness;
            amplitude *= _settings.Persistance;
        }

        noiseValue = Mathf.Max(0, noiseValue - _settings.MinValue);
        return noiseValue * _settings.Strength;
    }
}
