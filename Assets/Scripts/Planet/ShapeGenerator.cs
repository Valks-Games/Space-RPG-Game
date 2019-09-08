using UnityEngine;

public class ShapeGenerator {
    private ShapeSettings _settings;
    private INoiseFilter[] _noiseFilters;
    public MinMax ElevationMinMax;

    public void UpdateSettings(ShapeSettings settings)
    {
        _settings = settings;
        _noiseFilters = new INoiseFilter[settings.NoiseLayers.Length];
        for (int i = 0; i < _noiseFilters.Length; i++)
        {
            _noiseFilters[i] = NoiseFilterFactory.CreateNoiseFilter(settings.NoiseLayers[i].NoiseSettings);
        }
        ElevationMinMax = new MinMax();
    }

    public Vector3 CalculatePointOnPlanet(Vector3 pointOnUnitSphere, Vector3 renderOffset)
    {
        float firstLayerValue = 0;
        float elevation = 0;

        if (_noiseFilters.Length > 0)
        {
            firstLayerValue = _noiseFilters[0].Evaluate(pointOnUnitSphere);
            if (_settings.NoiseLayers[0].Enabled)
            {
                elevation = firstLayerValue;
            }
        }

        for (int i = 1; i < _noiseFilters.Length; i++)
        {
            if (_settings.NoiseLayers[i].Enabled)
            {
                float mask = (_settings.NoiseLayers[i].UseFirstLayerAsMask) ? firstLayerValue : 1;
                elevation += _noiseFilters[i].Evaluate(pointOnUnitSphere) * mask;
            }
        }
        elevation = _settings.PlanetRadius * (1 + elevation);
        ElevationMinMax.AddValue(elevation);
        //return pointOnUnitSphere * elevation + renderOffset;
        return pointOnUnitSphere * elevation;
    }
}
