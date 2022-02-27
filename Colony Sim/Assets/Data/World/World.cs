using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ColonySim.Helpers;
using ILoggerSlave = ColonySim.LoggingUtility.ILoggerSlave;
using ColonySim.LoggingUtility;
using ColonySim.World.Tiles;

namespace ColonySim.World
{
    /// <summary>
    /// Game World
    /// </summary>
    public class GameWorld : ILoggerSlave
    {
        public LoggingUtility.ILogger Master => WorldSystem.Get;
        public string LoggingPrefix => "<color=green>[WORLD]</color>";

        public float[] groundNoiseMap;
        public RectI worldRect;
        public Vector2Int Size;
        public Vector2Int ChunkSize;

        private readonly IWorldChunk[,] WorldChunks;

        public GameWorld(int width, int height)
        {
            WorldChunks = new IWorldChunk[width, height];
            worldRect = new RectI(new Vector2Int(0, 0), width, height);
            Size = new Vector2Int(width*WorldSystem.CHUNK_SIZE, height*WorldSystem.CHUNK_SIZE);
            ChunkSize = new Vector2Int(width, height);
            this.Notice($"<color=blue>[Generating world of Size {WorldChunks.Length}]</color>");       
        }

        public void GenerateWorldChunks()
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

        public void WorldGeneration(float seed)
        {
            groundNoiseMap = NoiseMap.GenerateNoiseMap(Size, 4, NoiseMap.GroundWave(seed));
            foreach (var tile in GetTiles())
            {
                //WorldPoint Point = tile.Coordinates;
                //float noise = groundNoiseMap[Point.X + Point.Y * Size.x];
                //Debug.Log($"{tile.Coordinates}::{noise}");
                //if (noise < 0.22f)
                //{
                    
                //}
                tile.Container.AddEntity(new DirtFloor());
            }
        }

        private IWorldChunk GenerateNewChunk(int X, int Y)
        {
            RectI rect = new RectI(new Vector2Int(X* WorldSystem.CHUNK_SIZE, Y* WorldSystem.CHUNK_SIZE), WorldSystem.CHUNK_SIZE, WorldSystem.CHUNK_SIZE);
            //rect.Clip(worldRect);
            IWorldChunk Chunk = new WorldChunk((X,Y), rect, WorldSystem.CHUNK_SIZE);
            return Chunk;
        }

        public IWorldChunk Chunk(ChunkLocation Coordinates)
        {
            return WorldChunks[Coordinates.X, Coordinates.Y];       
        }

        public ITileData Tile(WorldPoint Coordinates)
        {
            ChunkLocation chunkLoc = Coordinates;
            this.Debug($"Getting Tile {Coordinates} from {chunkLoc}", LoggingPriority.Low);
            return WorldChunks[chunkLoc.X, chunkLoc.Y].Tile(Coordinates);         
        }

        #region Enumerations

        public IEnumerator<ITileData> GetEnumerator()
        {
            foreach (var tile in GetTiles())
            {
                yield return tile;
            }
        }

        public IEnumerable<WorldPoint> WorldCoordinates()
        {
            for (int x = 0; x < WorldChunks.GetLength(0); x++)
            {
                for (int y = 0; y < WorldChunks.GetLength(1); y++)
                {
                    yield return new WorldPoint(x, y);
                }
            }
        }

        public IEnumerable<IWorldChunk> Chunks()
        {
            for (int x = 0; x < WorldChunks.GetLength(0); x++)
            {
                for (int y = 0; y < WorldChunks.GetLength(1); y++)
                {
                    yield return WorldChunks[x, y];
                }
            }
        }

        public IEnumerable<ITileData> GetTiles()
        {
            for (int x = 0; x < WorldChunks.GetLength(0); x++)
            {
                for (int y = 0; y < WorldChunks.GetLength(1); y++)
                {
                    foreach (var tile in WorldChunks[x, y].GetTiles())
                    {
                        yield return tile;
                    }
                }
            }
        }

        public IWorldChunk this[WorldPoint Coordinate]
        { get { return Chunk(Coordinate); } }

        public ITileData this[LocalPoint Coordinate]
        { get { return Tile(Coordinate); } }

        #endregion
    }
}
