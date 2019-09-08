using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoiseSettings
{
    public enum FilterType
    {
        Simple, Rigid
    }
    public FilterType TheFilterType;

    [ConditionalHide("TheFilterType", 0)]
    public SimpleNoiseSettings TheSimpleNoiseSettings;
    [ConditionalHide("TheFilterType", 1)]
    public RigidNoiseSettings TheRigidNoiseSettings;

    [System.Serializable]
    public class SimpleNoiseSettings
    {
        public float Strength = 1;
        [Range(1, 8)]
        public int NumLayers = 1;
        public float BaseRoughness = 1;
        public float Roughness = 2;
        public float Persistance = 0.5f;
        public Vector3 Centre;
        public float MinValue;
    }

    [System.Serializable]
    public class RigidNoiseSettings : SimpleNoiseSettings
    {
        public float WeightMultiplier = 0.8f;
    }
}
