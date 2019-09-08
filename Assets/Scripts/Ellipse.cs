using UnityEngine;

[System.Serializable]
public class Ellipse
{
    public float XAxis;
    public float YAxis;

    public Ellipse(float xAxis, float yAxis)
    {
        XAxis = xAxis;
        YAxis = yAxis;
    }

    public Vector2 Evaluate(float t)
    {
        float angle = Mathf.Deg2Rad * 360f * t;
        float x = Mathf.Sin(angle) * XAxis;
        float y = Mathf.Cos(angle) * YAxis;
        return new Vector2(x, y);
    }
}
