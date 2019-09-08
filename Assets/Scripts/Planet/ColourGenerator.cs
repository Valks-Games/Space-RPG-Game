using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourGenerator
{
    public ColourSettings ColourSettings;
    public Texture2D Texture;
    public const int TextureResolution = 50;

    public void UpdateSettings(ColourSettings colourSettings)
    {
        ColourSettings = colourSettings;
        if (Texture == null)
        {
            Texture = new Texture2D(TextureResolution, 1);
        }
    }

    public void UpdateElevation(MinMax elevationMinMax)
    {
        ColourSettings.PlanetMaterial.SetVector("_elevationMinMax", new Vector4(elevationMinMax.Min, elevationMinMax.Max));
    }

    public void UpdateColours()
    {
        Color[] colours = new Color[TextureResolution];
        for (int i = 0; i < TextureResolution; i++)
        {
            colours[i] = ColourSettings.Gradient.Evaluate(i / (TextureResolution - 1f));
        }
        Texture.SetPixels(colours);
        Texture.Apply();
        ColourSettings.PlanetMaterial.SetTexture("_texture", Texture);
    }
}
