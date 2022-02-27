using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ColonySim.Systems;

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
            : this(Coordinates.X * Coordinates.ChunkCoordinate.X * WorldSystem.CHUNK_SIZE,
                  Coordinates.X * Coordinates.ChunkCoordinate.Y * WorldSystem.CHUNK_SIZE)
        { }

        public WorldPoint(int X, int Y)
        { this.X = X; this.Y = Y; }

        public static implicit operator LocalPoint(WorldPoint Coordinate) =>
            new LocalPoint(Coordinate.X % WorldSystem.CHUNK_SIZE,
            Coordinate.Y % WorldSystem.CHUNK_SIZE);

        public override string ToString()
        {
            return string.Format("({0},{1})", X, Y);
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

        public static bool operator == (WorldPoint a, WorldPoint b) =>
            a.Equals(b);

        public static bool operator !=(WorldPoint a, WorldPoint b) =>
            !a.Equals(b);
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
            : this(Chunk, Coordinates.X, Coordinates.Y) { }
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

        public static implicit operator ChunkLocation(LocalPoint Coordinate) =>
            Coordinate.ChunkCoordinate;

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

        public static implicit operator (int, int)(ChunkLocation Chunk) =>
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
}
