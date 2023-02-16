using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ColonySim.Systems;
using System;

namespace ColonySim
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

        public WorldPoint(int X, int Y)
        { this.X = X; this.Y = Y; }

        public static implicit operator Vector2(WorldPoint Coordinate) =>
            new Vector2(Coordinate.X, Coordinate.Y);

        public static implicit operator Vector2Int(WorldPoint Coordinate) =>
            new Vector2Int(Coordinate.X, Coordinate.Y);

        public static implicit operator WorldPoint(Vector2 Coordinate) =>
            new WorldPoint(Mathf.FloorToInt(Coordinate.x), Mathf.FloorToInt(Coordinate.y));

        public override string ToString()
        {
            return $"({X},{Y})";
        }

        public override int GetHashCode() =>
         this.X * 666 + this.Y * 1339;

        public override bool Equals(object Other)
        {
            if (Other != null && Other is WorldPoint point)
            {
                return this.X == point.X && this.Y == point.Y;
            }
            return false;          
        }

        public static WorldPoint operator +(WorldPoint a, WorldPoint b) =>
            new WorldPoint(a.X + b.X, a.Y + b.Y);

        public static bool operator == (WorldPoint a, WorldPoint b) =>
            a.Equals(b);

        public static bool operator !=(WorldPoint a, WorldPoint b) =>
            !a.Equals(b);

        public static bool operator ==(WorldPoint a, Vector2 b) =>
            a.X == b.x && a.Y == b.y;

        public static bool operator !=(WorldPoint a, Vector2 b) =>
            a.X != b.x || a.Y != b.y;

        public static implicit operator Vector3(WorldPoint v) =>
            new Vector3(v.X, v.Y);

        public static implicit operator WorldPoint(Vector2Int v) =>
            new WorldPoint(v.x, v.y);
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

        public WorldPoint Origin =>
            new WorldPoint(X * WorldSystem.CHUNK_SIZE, Y * WorldSystem.CHUNK_SIZE);

        public WorldPoint Boundary =>
             new WorldPoint(X * WorldSystem.CHUNK_SIZE + WorldSystem.CHUNK_SIZE, Y * WorldSystem.CHUNK_SIZE + WorldSystem.CHUNK_SIZE);

        public static implicit operator WorldPoint(ChunkLocation Chunk) =>
            new WorldPoint(Chunk.X * WorldSystem.CHUNK_SIZE, Chunk.Y * WorldSystem.CHUNK_SIZE);

        public static implicit operator ChunkLocation(WorldPoint v)
        {
            int vX = Mathf.FloorToInt((float)v.X / WorldSystem.CHUNK_SIZE);
            int vY = Mathf.FloorToInt((float)v.Y / WorldSystem.CHUNK_SIZE);
            return new ChunkLocation(vX, vY);
        }

        public override string ToString()
        {
            return $"({X},{Y})";
        }

        public override int GetHashCode() =>
         this.X*WorldSystem.CHUNK_SIZE * 666 + this.Y * WorldSystem.CHUNK_SIZE * 1339;

        public override bool Equals(object obj)
        {
            if (obj is ChunkLocation chunk)
            {
                if (this.X == chunk.X && this.Y == chunk.Y) ;
            }
            return base.Equals(obj);
        }

        public static bool operator ==(ChunkLocation a, ChunkLocation b) =>
            a.X == b.X && a.Y == b.Y;
        public static bool operator !=(ChunkLocation a, ChunkLocation b) =>
            a.X != b.X || a.Y != b.Y;
    }
}
