#if UNITY_EDITOR

using UnityEngine;

public class DebugManager : MonoBehaviour
{
    public bool UseSceneView = false;

    // Start is called before the first frame update
    private void Awake()
    {
        if (UseSceneView)
            UnityEditor.SceneView.FocusWindowIfItsOpen(typeof(UnityEditor.SceneView));
    }
}
#endif