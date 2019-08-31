using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public void Update()
    {
        transform.Rotate(new Vector3(0.01f, 0, 0));       
    }
}
