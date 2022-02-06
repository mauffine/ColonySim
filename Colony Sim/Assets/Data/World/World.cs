using System;
using System.Collections;
using System.Collections.Generic;
using ColonySim.Systems;
using UnityEngine;

namespace ColonySim.World
{
    public interface ICoordinate
    {
        int X { get; }
        int Y { get; }
    }

    /// <summary>
    /// A point relative to the world position.
    /// </summary>
    public struct WorldPoint : ICoordinate
    {
        public int X { get; }
        public int Y { get; }

        public WorldPoint((int X, int Y) Coordinates)
            : this(Coordinates.X, Coordinates.Y) { }
        public WorldPoint(Vector2Int Coordinates)
            : this(Coordinates.x, Coordinates.y) { }
        public WorldPoint(LocalPoint Coordinates)
            : this(Coordinates.X*Coordinates.Chunk.X*WorldSystem.CHUNK_SIZE, 
                  Coordinates.X*Coordinates.Chunk.Y*WorldSystem.CHUNK_SIZE) { }

        public WorldPoint(int X, int Y)
            { this.X = X; this.Y = Y; }

        public static implicit operator LocalPoint(WorldPoint Coordinate) =>
            new LocalPoint(Coordinate.X % WorldSystem.CHUNK_SIZE,
            Coordinate.Y % WorldSystem.CHUNK_SIZE);

        public override String ToString()
        {
            return string.Format("({0}-{1})", X, Y);
        }
    }

    /// <summary>
    /// A point relative to a Chunk.
    /// </summary>
    public struct LocalPoint : ICoordinate
    {
        public int X { get; }
        public int Y { get; }
        public (int X, int Y) Chunk { get; }
        public LocalPoint((int X, int Y) Chunk, (int X, int Y) Coordinates)
            : this(Chunk,  Coordinates.X, Coordinates.Y) { }
        public LocalPoint(Vector2Int Coordinates, (int X, int Y) Chunk)
            : this(Chunk, Coordinates.x, Coordinates.y) { }
        public LocalPoint(WorldPoint Coordinates, (int X, int Y) Chunk)
            : this(Chunk, Coordinates.X % WorldSystem.CHUNK_SIZE, Coordinates.Y % WorldSystem.CHUNK_SIZE) { }

        public LocalPoint((int X, int Y) Chunk, int X, int Y)
            { this.Chunk = Chunk; this.X = X; this.Y = Y; }
        public LocalPoint((int X, int Y) Chunk, LocalPoint Coordinate)
            { this.Chunk = Chunk; this.X = Coordinate.X; this.Y = Coordinate.Y; }
        // LocalPoint, No Chunk Data
        public LocalPoint(int X, int Y)
        { this.Chunk = (1, 1); this.X = X; this.Y = Y; }

        public static implicit operator WorldPoint(LocalPoint Coordinate) => 
            new WorldPoint(Coordinate.X * Coordinate.Chunk.X * WorldSystem.CHUNK_SIZE,
            Coordinate.Y * Coordinate.Chunk.Y * WorldSystem.CHUNK_SIZE);

        public static explicit operator LocalPoint(WorldPoint Coordinate) =>
            new LocalPoint(Coordinate.X % WorldSystem.CHUNK_SIZE,
            Coordinate.Y % WorldSystem.CHUNK_SIZE);

        public LocalPoint RelativeTo((int X, int Y) Chunk)
        {
            return new LocalPoint(Chunk, X, Y);
        }

        public LocalPoint RelativeTo(int _X, int _Y)
        {
            return new LocalPoint((_X, _Y), X, Y);
        }


        public override String ToString()
        {
            return string.Format("({0}-{1})({2}-{3})", Chunk.X, Chunk.Y, X, Y);
        }
    }


    /// <summary>
    /// Game World
    /// </summary>
    public class GameWorld
    {
        #region Static
        private static GameWorld instance;
        public static GameWorld Get()
        {
            return instance;
        }
        #endregion

        private IWorldChunk[][] WorldChunks;
        public IEnumerable<IWorldChunk> GetChunks()
        {
            for (int x = 0; x < WorldChunks.Length; x++)
            {
                for (int y = 0; y < WorldChunks.Length; y++)
                {
                    yield return WorldChunks[x][y];
                }
            }
        }

        private readonly WorldSystem SYSTEM;

        public (int X, int Y) WorldBounds { get { return worldBounds; } }
        private readonly (int X, int Y) worldBounds;

        public GameWorld(int X, int Y)
        {
            SYSTEM = WorldSystem.Get;
            WorldChunks = new IWorldChunk[X][];
            for (int _x = 0; _x < X; _x++)
            {
                WorldChunks[_x] = new IWorldChunk[Y];
                for (int _y = 0; _y < Y; _y++)
                {
                    WorldChunks[_x][_y] = GenerateNewChunk(_x, _y);
                }
            }
            worldBounds = (X, Y);
        }

        private IWorldChunk GenerateNewChunk(int X, int Y)
        {
            IWorldChunk Chunk = new WorldChunk((X,Y), WorldSystem.CHUNK_SIZE);
            return Chunk;
        }

        public IWorldChunk GetChunk(WorldPoint Coordinates)
        {
            int X = Mathf.FloorToInt(Coordinates.X / WorldSystem.CHUNK_SIZE);
            int Y = Mathf.FloorToInt(Coordinates.Y / WorldSystem.CHUNK_SIZE);
            if (X < WorldChunks.Length && X >= 0)
            {
                if (Y < WorldChunks[X].Length && Y >= 0)
                {
                    return WorldChunks[X][Y];
                }
            }
            return null;            
        }

        public ITileData GetTileData(WorldPoint Coordinates)
        {
            if (Coordinates.X < 0 || Coordinates.Y < 0)
            {
                return null;
            }
            IWorldChunk _Chunk = GetChunk(Coordinates);
            if (_Chunk != null){
                return _Chunk.GetTileData(Coordinates);
            }
            else
            {
                Debug.Log("No Chunk Located");
            }
            return null;            
        }
    }
}
