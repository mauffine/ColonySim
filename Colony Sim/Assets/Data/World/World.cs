using System;
using System.Collections;
using System.Collections.Generic;
using ColonySim.Systems;
using UnityEngine;
using ColonySim.Helpers;
using ILogger = ColonySim.LoggingUtility.ILoggerSlave;
using ColonySim.LoggingUtility;

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
            : this(Coordinates.X*Coordinates.ChunkCoordinate.X*WorldSystem.CHUNK_SIZE, 
                  Coordinates.X*Coordinates.ChunkCoordinate.Y*WorldSystem.CHUNK_SIZE) { }

        public WorldPoint(int X, int Y)
            { this.X = X; this.Y = Y; }

        public static implicit operator LocalPoint(WorldPoint Coordinate) =>
            new LocalPoint(Coordinate.X % WorldSystem.CHUNK_SIZE,
            Coordinate.Y % WorldSystem.CHUNK_SIZE);

        public override String ToString()
        {
            return string.Format("({0},{1})", X, Y);
        }
    }

    /// <summary>
    /// A point relative to a Chunk.
    /// </summary>
    public struct LocalPoint : ICoordinate
    {
        public int X { get; }
        public int Y { get; }
        public ChunkLocation ChunkCoordinate { get; }

        public LocalPoint((int X, int Y) Chunk, (int X, int Y) Coordinates)
            : this(Chunk,  Coordinates.X, Coordinates.Y) { }
        public LocalPoint(Vector2Int Coordinates, (int X, int Y) Chunk)
            : this(Chunk, Coordinates.x, Coordinates.y) { }
        public LocalPoint(WorldPoint Coordinates, (int X, int Y) Chunk)
            : this(Chunk, Coordinates.X % WorldSystem.CHUNK_SIZE, Coordinates.Y % WorldSystem.CHUNK_SIZE) { }

        public LocalPoint((int X, int Y) Chunk, int X, int Y)
            { this.ChunkCoordinate = Chunk; this.X = X; this.Y = Y; }
        public LocalPoint((int X, int Y) Chunk, LocalPoint Coordinate)
            { this.ChunkCoordinate = Chunk; this.X = Coordinate.X; this.Y = Coordinate.Y; }
        public LocalPoint(int X, int Y)
        { this.ChunkCoordinate = default; this.X = X; this.Y = Y; }

        public static implicit operator WorldPoint(LocalPoint Coordinate) => 
            new WorldPoint(Coordinate.X + Coordinate.ChunkCoordinate.X * WorldSystem.CHUNK_SIZE,
            Coordinate.Y + Coordinate.ChunkCoordinate.Y * WorldSystem.CHUNK_SIZE);

        public static explicit operator LocalPoint(WorldPoint Coordinate) =>
            new LocalPoint(Coordinate.X % WorldSystem.CHUNK_SIZE,
            Coordinate.Y % WorldSystem.CHUNK_SIZE);

        /// <summary>
        /// Return a Local Point copied relative to Chunk.
        /// </summary>
        /// <param name="Chunk"></param>
        /// <returns></returns>
        public LocalPoint RelativeTo((int X, int Y) Chunk)
        {
            return new LocalPoint(Chunk, X, Y);
        }

        /// <summary>
        /// Return a Local Point copied relative to Chunk.
        /// </summary>
        /// <param name="Chunk"></param>
        /// <returns></returns>
        public LocalPoint RelativeTo(int _X, int _Y)
        {
            return new LocalPoint((_X, _Y), X, Y);
        }

        public override string ToString()
        {
            return string.Format("({0},{1})", X, Y);
        }
    }

    /// <summary>
    /// A Chunk's relative position to another.
    /// </summary>
    public struct ChunkLocation : ICoordinate
    {
        public int X { get; }
        public int Y { get; }

        public ChunkLocation(int X, int Y)
        { this.X = X; this.Y = Y; }

        public ChunkLocation((int X, int Y) Coordinates)
        { this.X = Coordinates.X; this.Y = Coordinates.Y; }

        public static implicit operator (int,int)(ChunkLocation Chunk) =>
            (Chunk.X, Chunk.Y);

        public static implicit operator ChunkLocation((int X, int Y) Coordinates) =>
            new ChunkLocation(Coordinates);

        public static implicit operator WorldPoint(ChunkLocation Chunk) =>
            new WorldPoint(Chunk.X * WorldSystem.CHUNK_SIZE, Chunk.Y * WorldSystem.CHUNK_SIZE);

        public static implicit operator ChunkLocation(WorldPoint v) =>
            new ChunkLocation(v.X / WorldSystem.CHUNK_SIZE, v.Y / WorldSystem.CHUNK_SIZE);

        public override string ToString()
        {
            return $"({X},{Y})";
        }
    }


    /// <summary>
    /// Game World
    /// </summary>
    public class GameWorld : ILoggerSlave
    {
        public LoggingUtility.ILogger Master => WorldSystem.Get;
        public string LoggingPrefix => "<color=green>[WORLD]</color>";



        private IWorldChunk[,] WorldChunks;
        public IEnumerable<IWorldChunk> GetChunks()
        {
            for (int x = 0; x < WorldChunks.GetLength(0); x++)
            {
                for (int y = 0; y < WorldChunks.GetLength(1); y++)
                {
                    yield return WorldChunks[x, y];
                }
            }
        }

        public RectI worldRect;

        public float[] groundNoiseMap;
        public Vector2Int Size;

        public GameWorld(int width, int height)
        {
            WorldChunks = new IWorldChunk[width, height];
            worldRect = new RectI(new Vector2Int(0, 0), width, height);
            Size = new Vector2Int(width*WorldSystem.CHUNK_SIZE, height*WorldSystem.CHUNK_SIZE);
            groundNoiseMap = NoiseMap.GenerateNoiseMap(new Vector2Int(width * WorldSystem.CHUNK_SIZE, height * WorldSystem.CHUNK_SIZE), 10, NoiseMap.GroundWave(987));
            this.Notice($"<color=blue>[Generating world of Size {WorldChunks.Length}]</color>");

            GenerateWorldChunks();           
        }

        private void GenerateWorldChunks()
        {
            for (int x = 0; x < WorldChunks.GetLength(0); x++)
            {
                for (int y = 0; y < WorldChunks.GetLength(1); y++)
                {
                    this.Verbose($"Creating Chunk At::{x}-{y}");
                    WorldChunks[x, y] = GenerateNewChunk(x, y);
                }
            }
        }

        private IWorldChunk GenerateNewChunk(int X, int Y)
        {
            RectI rect = new RectI(new Vector2Int(X* WorldSystem.CHUNK_SIZE, Y* WorldSystem.CHUNK_SIZE), WorldSystem.CHUNK_SIZE, WorldSystem.CHUNK_SIZE);
            //rect.Clip(worldRect);
            IWorldChunk Chunk = new WorldChunk((X,Y), rect, WorldSystem.CHUNK_SIZE);
            return Chunk;
        }

        public IWorldChunk GetChunk(WorldPoint Coordinates)
        {
            int X = Mathf.FloorToInt(Coordinates.X / WorldSystem.CHUNK_SIZE);
            int Y = Mathf.FloorToInt(Coordinates.Y / WorldSystem.CHUNK_SIZE);
            if (X < WorldChunks.GetLength(0) && Y < WorldChunks.GetLength(1))
            {
                return WorldChunks[X, Y];
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
            return null;            
        }

        public IEnumerator<ITileData> GetEnumerator()
        {
            foreach (var WorldChunk in GetChunks())
            {
                foreach (var tile in WorldChunk.TileData)
                {
                    yield return tile;
                }
            }
        }

        public IWorldChunk this[WorldPoint Coordinate]
        { get { return GetChunk(Coordinate); } }

        public ITileData this[LocalPoint Coordinate]
        { get { return GetTileData(Coordinate); } }
    }
}
