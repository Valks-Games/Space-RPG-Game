using UnityEngine;

public static class CubeMeshData
{
    public static Vector3[] vertices(float vertexOffset) {
        Vector3[] vertices = {
            new Vector3( vertexOffset,   vertexOffset,   vertexOffset),
            new Vector3(-vertexOffset,   vertexOffset,   vertexOffset),
            new Vector3(-vertexOffset,  -vertexOffset,   vertexOffset),
            new Vector3( vertexOffset,  -vertexOffset,   vertexOffset),
            new Vector3(-vertexOffset,   vertexOffset,  -vertexOffset),
            new Vector3( vertexOffset,   vertexOffset,  -vertexOffset),
            new Vector3( vertexOffset,  -vertexOffset,  -vertexOffset),
            new Vector3(-vertexOffset,  -vertexOffset,  -vertexOffset)
        };

        return vertices;
    }

    public static int[][] faceTriangles = {
        new int[]{ 0, 1, 2, 3 },
        new int[]{ 5, 0, 3, 6 },
        new int[]{ 4, 5, 6, 7 },
        new int[]{ 1, 4, 7, 2 },
        new int[]{ 5, 4, 1, 0 },
        new int[]{ 3, 2, 7, 6 }
    };

    public static Vector3[] faceVertices(int dir, float vertexOffset) {
        Vector3[] faceVector = new Vector3[4];
        for (int i = 0; i < faceVector.Length; i++) {
            Vector3[] verts = vertices(vertexOffset);
            faceVector[i] = verts[faceTriangles[dir][i]];
        }
        return faceVector;
    }
}
