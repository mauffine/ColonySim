using System.Collections;
using System.Collections.Generic;

namespace ColonySim.World
{
    public interface IWorldChunk
    {
        (int X, int Y) Coordinates { get; }
        TileData[][] Tiles { get; }

        (int X, int Y) WorldCoordinate(ITileData Tile);
    }
    /// <summary>
    /// Collection of Tiles
    /// </summary>
    public class WorldChunk : IWorldChunk
    {
        public TileData[][] Tiles { get; private set; }

        public (int X, int Y) Coordinates { get { return coordinates; } }

        private readonly (int X, int Y) coordinates;

        public WorldChunk((int X, int Y) Coordinates, int CHUNK_SIZE)
        {
            coordinates = Coordinates;
            GenerateChunk(CHUNK_SIZE);
        }

        private void GenerateChunk(int CHUNK_SIZE)
        {
            Tiles = new TileData[CHUNK_SIZE][];
            for (int x = 0; x < CHUNK_SIZE; x++)
            {
                Tiles[x] = new TileData[CHUNK_SIZE];
                for (int y = 0; y < CHUNK_SIZE; y++)
                {
                    ITileData Data = Tiles[x][y] = new TileData(x, y);
                    (int, int) Coords = WorldCoordinate(Data);
                    UnityEngine.Debug.LogFormat("Tile Created [{0}-{1}]", Coords.Item1, Coords.Item2);
                }
            }
        }

        public (int X, int Y) WorldCoordinate(ITileData Tile)
        {
            int X = Coordinates.X * Tiles.Length + Tile.Coordinates.X;
            int Y = Coordinates.Y * Tiles.Length + Tile.Coordinates.Y;
            return (X, Y);
        }


    }
}
