using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelData
{
    // int[,] data = new int[,] { {0, 1, 1}, {1, 1, 1}, {1, 1, 0} };
    const int Size = 20;
    int[,,] data = new int[Size, 1, Size];

    public VoxelData() {
        for (int x = 0; x < data.GetLength(0); x++) {
            for (int y = 0; y < data.GetLength(1); y++) {
                for (int z = 0; z < data.GetLength(2); z++)
                {

                    float frequency = 4f;

                    float noise = Mathf.PerlinNoise(x / (float) Size * frequency, z / (float) Size * frequency);

                    if (noise < 0.5)
                    {
                        data[x, y, z] = 1;
                    }
                    else {
                        data[x, y, z] = 0;
                    }
                }
            }
        }
    }

    public int Width {
        get { return data.GetLength(0); }
    }

    public int Height {
        get { return data.GetLength(1); }
    }

    public int Depth
    {
        get { return data.GetLength(2); }
    }

    public int GetCell(int x, int y, int z) {
        return data[x, y, z];
    }

    public int GetNeighbor(int x, int y, int z, Direction dir) {
        DataCoordinate offset = offsets[(int) dir]; // The offset to check.
        DataCoordinate neighborCoord = new DataCoordinate(x + offset.x, y + offset.y, z + offset.z);

        if (neighborCoord.x < 0 || neighborCoord.x >= Width || neighborCoord.y < 0 || neighborCoord.y >= Height || neighborCoord.z < 0 || neighborCoord.z >= Depth)
        {
            return 0;
        }
        else {
            return GetCell(neighborCoord.x, neighborCoord.y, neighborCoord.z);
        }
    }

    struct DataCoordinate {
        public int x;
        public int y;
        public int z;

        public DataCoordinate(int x, int y, int z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    DataCoordinate[] offsets = {
        new DataCoordinate(0, 0, 1),
        new DataCoordinate(1, 0, 0),
        new DataCoordinate(0, 0, -1),
        new DataCoordinate(-1, 0, 0),
        new DataCoordinate(0, 1, 0),
        new DataCoordinate(0, -1, 0)
    };
}

public enum Direction {
    North,
    East,
    South,
    West,
    Up,
    Down
}
