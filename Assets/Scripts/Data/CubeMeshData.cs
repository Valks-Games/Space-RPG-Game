﻿using UnityEngine;

public static class CubeMeshData
{
    public static Vector3[] vertices = {
        new Vector3( 1,   1,   1),
        new Vector3(-1,   1,   1),
        new Vector3(-1,  -1,   1),
        new Vector3( 1,  -1,   1),
        new Vector3(-1,   1,  -1),
        new Vector3( 1,   1,  -1),
        new Vector3( 1,  -1,  -1),
        new Vector3(-1,  -1,  -1)
    };

    public static int[][] faceTriangles = {
        new int[]{ 0, 1, 2, 3 },
        new int[]{ 5, 0, 3, 6 },
        new int[]{ 4, 5, 6, 7 },
        new int[]{ 1, 4, 7, 2 },
        new int[]{ 5, 4, 1, 0 },
        new int[]{ 3, 2, 7, 6 }
    };

    public static Vector3[] faceVertices(int dir, float scale, Vector3 pos) {
        Vector3[] faceVector = new Vector3[4];
        for (int i = 0; i < faceVector.Length; i++) {
            faceVector[i] = (vertices[faceTriangles[dir][i]] * scale) + pos;
        }
        return faceVector;
    }

    public static Vector3[] faceVertices(Direction dir, float scale, Vector3 pos)
    {
        return faceVertices((int) dir, scale, pos);
    }
}
