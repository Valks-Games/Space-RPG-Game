using UnityEngine;

public class ShapeGenerator {
    private ShapeSettings _settings;

    public ShapeGenerator(ShapeSettings settings)
    {
        _settings = settings;
    }

    public Vector3 CalculatePointOnPlanet(Vector3 pointOnUnitSphere, Vector3 renderOffset)
    {
        return pointOnUnitSphere * _settings.PlanetRadius + renderOffset;
    }
}
