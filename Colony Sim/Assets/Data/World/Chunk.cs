using ColonySim.Helpers;
using ColonySim.World.Tiles;
using System.Collections;
using System.Collections.Generic;

namespace ColonySim.World
{
    public interface IWorldChunk : IWorldTick
    {
        ChunkLocation Coordinates { get; }
        ITileData[,] TileData { get; }
        public RectI ChunkRect { get; }

        ITileData GetTileData(LocalPoint Coordinates);
    }
    /// <summary>
    /// Collection of Tiles
    /// </summary>
    public class WorldChunk : IWorldChunk
    {
        public ITileData[,] TileData { get; private set; }

        public ChunkLocation Coordinates { get { return coordinates; } }
        private readonly ChunkLocation coordinates;
        public RectI ChunkRect => chunkRect;
        private readonly RectI chunkRect;

        public WorldChunk(ChunkLocation Coordinates, RectI ChunkRect, int CHUNK_SIZE)
        {
            coordinates = Coordinates;
            chunkRect = ChunkRect;
            GenerateChunk(CHUNK_SIZE);
        }

        private void GenerateChunk(int CHUNK_SIZE)
        {
            TileData = new ITileData[CHUNK_SIZE, CHUNK_SIZE];
            for (int x = 0; x < CHUNK_SIZE; x++)
            {
                for (int y = 0; y < CHUNK_SIZE; y++)
                {
                    ITileData Data = TileData[x,y] = new TileData(Coordinates, x, y);
                    Data.Container.AddEntity(new ConcreteFloor());
                }
            }
        }

        public void WorldTick(float delta)
        {
            
        }

        public ITileData GetTileData(LocalPoint Point)
        {
            if (Point.X < TileData.GetLength(0) && Point.Y < TileData.GetLength(1))
            {
                return TileData[Point.X, Point.Y];
            }
            return null;          
        }
    }
}
