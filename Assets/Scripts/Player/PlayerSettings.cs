using UnityEngine;

public class PlayerSettings : MonoBehaviour
{
    public int frameRate = 60;

    private void Start()
    {
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = frameRate;
    }

    private void Update()
    {
        
    }
}
