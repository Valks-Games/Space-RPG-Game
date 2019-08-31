using UnityEngine;

public class VoxelData
{
    private int[,,] _data;
    private int _world_height = 64;

    public VoxelData(int chunkX, int chunkZ, int chunkSize, int worldSize) {
        ChunkX = chunkX;
        ChunkZ = chunkZ;

        _data = new int[worldSize, _world_height, worldSize];

        GameObject world_game_object = GameObject.Find("World");
        World world_script = world_game_object.GetComponent<World>();

        float[,] noise = world_script.GetWorldNoise();

        for (int x = chunkSize * chunkX; x < chunkSize * (chunkX + 1); x++) {
            for (int z = chunkSize * chunkZ; z < chunkSize * (chunkZ + 1); z++)
            {
                if (x < 0 || z < 0 || x > noise.GetLength(0) || z > noise.GetLength(1))
                    continue;

                int elevation = (int) (noise[x,z] * chunkSize);
                _data[x, elevation, z] = 1;

                for (int y = 0; y < elevation; y++) {
                    _data[x, y, z] = 1;
                }
            }
        }
    }

    public int ChunkX { get; }

    public int ChunkZ { get; }

    public int Width
    {
        get { return _data.GetLength(0); }
    }

    public int Height
    {
        get { return _data.GetLength(1); }
    }

    public int Depth
    {
        get { return _data.GetLength(2); }
    }

    public int GetCell(int x, int y, int z)
    {
        if (x < 0 || z < 0 || x > Width || z > Depth)
            return 0;

        return _data[x, y, z];
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
