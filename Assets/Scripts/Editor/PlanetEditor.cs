using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Planet))]
public class PlanetEditor : Editor {

    private Planet _planet;
    private Editor _shapeEditor;
    private Editor _colourEditor;

	public override void OnInspectorGUI()
	{
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            base.OnInspectorGUI();
            if (check.changed)
            {
                _planet.GeneratePlanet();
            }
        }

        if (GUILayout.Button("Generate Planet"))
        {
            _planet.GeneratePlanet();
        }

        DrawSettingsEditor(_planet.ShapeSettings, _planet.OnShapeSettingsUpdated, ref _planet.ShapeSettingsFoldout, ref _shapeEditor);
        DrawSettingsEditor(_planet.ColourSettings, _planet.OnColourSettingsUpdated, ref _planet.ColourSettingsFoldout, ref _colourEditor);
	}

    void DrawSettingsEditor(Object settings, System.Action onSettingsUpdated, ref bool foldout, ref Editor editor)
    {
        if (settings != null)
        {
            foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                if (foldout)
                {
                    CreateCachedEditor(settings, null, ref editor);
                    editor.OnInspectorGUI();

                    if (check.changed)
                    {
                        if (onSettingsUpdated != null)
                        {
                            onSettingsUpdated();
                        }
                    }
                }
            }
        }
    }

	private void OnEnable()
	{
        _planet = (Planet)target;
	}
}
