using System.Collections;
using System.Collections.Generic;

namespace ColonySim.World
{
    public interface IWorldChunk : IWorldTick
    {
        (int X, int Y) Coordinates { get; }
        TileData[][] TileData { get; }

        TileData GetTileData(LocalPoint Coordinates);
    }
    /// <summary>
    /// Collection of Tiles
    /// </summary>
    public class WorldChunk : IWorldChunk
    {
        public TileData[][] TileData { get; private set; }

        public (int X, int Y) Coordinates { get { return coordinates; } }
        private readonly (int X, int Y) coordinates;

        public WorldChunk((int X, int Y) Coordinates, int CHUNK_SIZE)
        {
            coordinates = Coordinates;
            GenerateChunk(CHUNK_SIZE);
        }

        private void GenerateChunk(int CHUNK_SIZE)
        {
            TileData = new TileData[CHUNK_SIZE][];
            for (int x = 0; x < CHUNK_SIZE; x++)
            {
                TileData[x] = new TileData[CHUNK_SIZE];
                for (int y = 0; y < CHUNK_SIZE; y++)
                {
                    ITileData Data = TileData[x][y] = new TileData(Coordinates, x, y);
                    Data.Container.AddEntity(new ConcreteFloor());
                }
            }
        }

        public void WorldTick(float delta)
        {
            
        }

        public TileData GetTileData(LocalPoint Point)
        {
            if (Point.X < TileData.Length && Point.Y < TileData.Length)
            {
                return TileData[Point.X][Point.Y];
            }
            return null;          
        }
    }
}
