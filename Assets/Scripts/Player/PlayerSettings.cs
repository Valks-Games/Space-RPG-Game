using UnityEngine;

public class PlayerSettings : MonoBehaviour
{
    public int FrameRate = 60;

    private void Start()
    {
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = FrameRate;
    }
}
