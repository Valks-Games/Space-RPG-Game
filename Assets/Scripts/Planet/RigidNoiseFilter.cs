using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidNoiseFilter : INoiseFilter
{
    private NoiseSettings.RigidNoiseSettings _settings;
    private Noise _noise = new Noise();

    public RigidNoiseFilter(NoiseSettings.RigidNoiseSettings settings)
    {
        _settings = settings;
    }

    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0;
        float frequency = _settings.BaseRoughness;
        float amplitude = 1;
        float weight = 1;

        for (int i = 0; i < _settings.NumLayers; i++)
        {
            float v = 1 - Mathf.Abs(_noise.Evaluate(point * frequency + _settings.Centre));
            v *= v;
            v *= weight;
            weight = Mathf.Clamp01(v * _settings.WeightMultiplier);

            noiseValue += v * amplitude;
            frequency *= _settings.Roughness;
            amplitude *= _settings.Persistance;
        }

        noiseValue = Mathf.Max(0, noiseValue - _settings.MinValue);
        return noiseValue * _settings.Strength;
    }
}
