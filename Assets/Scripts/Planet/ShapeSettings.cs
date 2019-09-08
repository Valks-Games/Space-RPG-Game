using UnityEngine;

[CreateAssetMenu()]
public class ShapeSettings : ScriptableObject
{
    public float PlanetRadius = 1;
    public NoiseLayer[] NoiseLayers;

    [System.Serializable]
    public class NoiseLayer
    {
        public bool Enabled = true;
        public bool UseFirstLayerAsMask;
        public NoiseSettings NoiseSettings;
    }
}
