using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class VoxelData
{
    private int[,,] data;
    private int world_height = 64;

    public VoxelData(int chunk_x, int chunk_z, int chunk_size, int world_size, float frequency, int seed) {
        ChunkX = chunk_x;
        ChunkZ = chunk_z;

        data = new int[world_size, world_height, world_size];

        GameObject world_game_object = GameObject.Find("World");
        World world_script = world_game_object.GetComponent<World>();

        float[,] noise = world_script.GetWorldNoise();

        for (int x = chunk_size * chunk_x; x < chunk_size * (chunk_x + 1); x++) {
            for (int z = chunk_size * chunk_z; z < chunk_size * (chunk_z + 1); z++)
            {
                int elevation = (int) (noise[x,z] * chunk_size);
                data[x, elevation, z] = 1;

                for (int y = 0; y < elevation; y++) {
                    data[x, y, z] = 1;
                }
            }
        }
    }

    public int ChunkX { get; }

    public int ChunkZ { get; }

    public int Width
    {
        get { return data.GetLength(0); }
    }

    public int Height
    {
        get { return data.GetLength(1); }
    }

    public int Depth
    {
        get { return data.GetLength(2); }
    }

    public int GetCell(int x, int y, int z)
    {
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
