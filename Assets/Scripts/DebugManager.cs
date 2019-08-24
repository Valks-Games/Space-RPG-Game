#if UNITY_EDITOR

using UnityEngine;

public class DebugManager : MonoBehaviour
{
    public bool useSceneView = false;

    // Start is called before the first frame update
    private void Awake()
    {
        if (useSceneView)
        {
            UnityEditor.SceneView.FocusWindowIfItsOpen(typeof(UnityEditor.SceneView));
        }
    }
}
#endif