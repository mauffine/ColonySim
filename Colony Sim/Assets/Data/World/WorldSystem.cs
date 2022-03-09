using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ColonySim.LoggingUtility;
using ILogger = ColonySim.LoggingUtility.ILogger;
using ColonySim.Rendering;
using ISystem = ColonySim.Systems.System;
using UnityEditor;
using ColonySim.Systems;
using ColonySim.World.Tiles;
using ColonySim.Systems.Navigation;

namespace ColonySim.World
{
    public class WorldSystem : ISystem, ILogger
    {
        #region Static
        private static WorldSystem instance;
        public static WorldSystem Get => instance;
        public List<string> Logs { get; set; } = new List<string>();
        public LoggingLevel LoggingLevel { get => _loggingLevel; set => _loggingLevel = value; }
        [SerializeField]
        private LoggingLevel _loggingLevel = LoggingLevel.Warning;
        public LoggingPriority LoggingPriority { get => _loggingPriority; set => _loggingPriority = value; }
        [SerializeField]
        private LoggingPriority _loggingPriority = LoggingPriority.AlwaysShow;
        public bool Stamp { get => _stamp; set => _stamp = value; }
        public string LoggingPrefix => "<color=green>[WORLDSYS]</color>";
        [SerializeField]
        private bool _stamp = false;

        public LoggingLevel _rendererLogging = LoggingLevel.Warning;
        public LoggingPriority _rendererPriority = LoggingPriority.AlwaysShow;
        public LoggingLevel _worldLogging = LoggingLevel.Warning;
        public LoggingPriority _worldPriority = LoggingPriority.AlwaysShow;

        #endregion
        public static WorldRenderer Renderer;
        public static WorldSimulation Simulation;
        public static GameWorld World => _world;
        private static GameWorld _world;

        public Transform TileMapTransform;

        public const int CHUNK_SIZE = 12;
        
        [SerializeField]
        private bool DrawGizmoTiles;
        [SerializeField]
        private bool DrawGizmoTileCoordinates;
        [SerializeField]
        private bool DrawGizmoChunks;
        [SerializeField]
        private bool DrawGizmoNoiseMap;
        [SerializeField]
        private bool drawNavRegionsGizmo = false;

        public override void Init()
        {
            this.Notice("> World System Init.. <");
            instance = this;
            _world = new GameWorld(1, 1);
            _world.GenerateWorldChunks();
            _world.WorldGeneration(Time.realtimeSinceStartup);
            Renderer = new WorldRenderer();
            Renderer.TileMapTransform = this.TileMapTransform;
            Simulation = new WorldSimulation();

            foreach (var Chunk in _world.Chunks())
            {
                Renderer.RenderNewChunk(Chunk);
                Simulation.Simulate(Chunk);
            }
            base.Init();
        }

        public override void OnInitialized()
        {
            base.OnInitialized();
            foreach (var Chunk in _world.Chunks())
            {
                foreach (var region in Chunk.Regions)
                {
                    FloodFillRegion(region, Chunk.Coordinates.Origin, 
                        Chunk.Coordinates.Origin, Chunk.Coordinates.Boundary, null);
                }
            }
        }

        public override void Tick()
        {
            base.Tick();
            Simulation.Tick();
            Renderer.Tick();
        }

        public static WorldPoint ToWorldPoint(Vector3 worldPos)
        {
            int X = Mathf.FloorToInt(worldPos.x);
            int Y = Mathf.FloorToInt(worldPos.y);
            return new WorldPoint(X, Y);
        }

        public static WorldPoint ToWorldPoint(Vector2 worldPos)
        {
            int X = Mathf.FloorToInt(worldPos.x);
            int Y = Mathf.FloorToInt(worldPos.y);
            return new WorldPoint(X, Y);
        }

        public static void InvalidateChunk(ChunkLocation ChunkLoc)
        {
            IWorldChunk chunkData = Chunk(ChunkLoc);
            chunkData.ClearRegions();
            foreach (var region in chunkData.Regions)
            {
                instance.FloodFillRegion(region, chunkData.Coordinates.Origin,
                        chunkData.Coordinates.Origin, chunkData.Coordinates.Boundary, null);
            }
        }

        private void FloodFillRegion(WorldRegion Region, WorldPoint Origin, WorldPoint MinBoundary, WorldPoint MaxBoundary,List<ITileData> closedSet)
        {
            if(closedSet == null) closedSet = new List<ITileData>();
            this.Verbose($"Flood Filling Region @{Region.Location}::{Region.Origin}::{Region.Location.Boundary}");
            ITileData OriginTile = Tile(Origin);
            if (OriginTile == null) return;

            List<ITileData> traversibleTiles = new List<ITileData>();
            List<WorldPoint> boundaries = new List<WorldPoint>();
            Stack<ITileData> stack = new Stack<ITileData>();
            stack.Push(OriginTile);

            while (stack.Count > 0)
            {
                ITileData next = stack.Pop();
                foreach (var neighbour in TileManager.AdjacentTiles(next))
                {
                    if (neighbour == null || closedSet.Contains(neighbour)) continue;
                    closedSet.Add(neighbour);
                    WorldPoint neighbourLoc = neighbour.Coordinates;
                    // if we're outside of the regions bounds, skip
                    if (neighbourLoc.X >= MaxBoundary.X || neighbourLoc.Y >= MaxBoundary.Y
                        || neighbourLoc.X < MinBoundary.X || neighbourLoc.Y < MinBoundary.Y)
                    {
                        boundaries.Add(neighbourLoc);
                        continue;
                    }
                    if (neighbour.NavData != null)
                    {
                        ITileNavData navData = neighbour.NavData[NavigationMode.Walking];
                        if (navData != null && navData.Traversible)
                        {
                            stack.Push(neighbour);
                            traversibleTiles.Add(neighbour);
                            continue;
                        }
                    }
                    boundaries.Add(neighbourLoc);
                }
            }

            Region.SetRegionTiles(traversibleTiles.ToArray(), boundaries.ToArray());

            foreach (var boundary in boundaries)
            {
                foreach (var neighbour in TileManager.AdjacentTiles(boundary))
                {
                    if (neighbour == null || closedSet.Contains(neighbour)) continue;
                    if (neighbour.X >= MaxBoundary.X || neighbour.Y >= MaxBoundary.Y
                        || neighbour.X < MinBoundary.X || neighbour.Y < MinBoundary.Y) continue;
                    // If the boundary is adjacent to a tile that is NOT outside of the chunk
                    // then it's probably a new region.
                    IWorldChunk chunkData = Chunk(Region.Location);
                    WorldRegion newRegion = new WorldRegion(neighbour.Coordinates, Region.Location);
                    chunkData.AddRegion(newRegion);
                    FloodFillRegion(newRegion, neighbour.Coordinates, chunkData.Coordinates.Origin, chunkData.Coordinates.Boundary, closedSet);
                    return;
                }
            }
        }

        #region Static Helpers

        public IWorldChunk this[ChunkLocation Coordinate] =>
            Chunk(Coordinate);

        public ITileData this[WorldPoint Coordinate] =>
            Tile(Coordinate);

        public static ITileData Tile(int X, int Y) => Tile(new WorldPoint(X, Y));

        public static ITileData Tile(WorldPoint Coordinates)
        {
            instance.Debug($"Getting Tile At::{Coordinates}", LoggingPriority.Low);
            if (Coordinates.X >= 0 && Coordinates.Y >= 0 &&
                Coordinates.X < _world.Size.x && Coordinates.Y < _world.Size.y)
                return TileUnsf(Coordinates);
            return null;
        }

        public static IWorldChunk Chunk(ChunkLocation Coordinates)
        {
            if (Coordinates.X >= 0 && Coordinates.Y >= 0 &&
                Coordinates.X < _world.ChunkSize.x && Coordinates.Y < _world.ChunkSize.y)
                return ChunkUnsf(Coordinates);
            return null;
        }

        public static ITileData TileUnsf(WorldPoint Coordinate) =>
            _world.Tile(Coordinate);

        public static IWorldChunk ChunkUnsf(WorldPoint worldPos) =>
            _world.Chunk(worldPos);

        #endregion

        #region Gizmos

        public void OnDrawGizmos()
        {
            if (Initialized)
            {
                if (DrawGizmoTiles)
                {
                    foreach (WorldPoint Coordinates in _world.WorldCoordinates())
                    {
                        Gizmos.DrawWireCube(
                            new Vector3(Coordinates.X+.5f, Coordinates.Y+.5f),
                            Vector3.one);

                        if (DrawGizmoTileCoordinates)
                        {
                            Vector3 v = new Vector3(Coordinates.X, Coordinates.Y + .5f);
                            Vector3 dif = new Vector3(CursorSystem.Get.currentMousePosition.X - Coordinates.X, CursorSystem.Get.currentMousePosition.Y - Coordinates.Y);
                            if (Mathf.Abs(dif.x) < 3 && Mathf.Abs(dif.y) < 3)
                            {
                                Handles.Label(v, Coordinates.ToString());
                            }
                        }             
                    }
                }
                if (DrawGizmoChunks)
                {
                    foreach (var chunk in _world.Chunks())
                    {
                        Gizmos.color = new Color(0, 0, 1, .1f);
                        Gizmos.DrawCube(
                            new Vector3(chunk.ChunkRect.max.x - (chunk.ChunkRect.width) / 2f,
                                        chunk.ChunkRect.max.y - (chunk.ChunkRect.height) / 2f,
                                        1f),
                            new Vector3(chunk.ChunkRect.width - .5f, chunk.ChunkRect.height - .5f, 1f)
                            );
                    }
                }
                if (DrawGizmoNoiseMap)
                {
                    foreach (WorldPoint Coordinates in _world.WorldCoordinates())
                    {
                        float h = _world.groundNoiseMap[Coordinates.X + Coordinates.Y * World.Size.x];
                        Gizmos.color = new Color(h, h, h, .9f);
                        Gizmos.DrawCube(
                            new Vector3(Coordinates.X + .5f, Coordinates.Y + .5f),
                            Vector3.one);
                    }
                }

                if (drawNavRegionsGizmo)
                {
                    int index = 0;
                    foreach (var chunk in World.Chunks())
                    {
                        WorldRegion[] regions = chunk.Regions;
                        if (regions != null)
                        {
                            foreach (var region in regions)
                            {
                                if (region.Boundaries != null)
                                {
                                    foreach (var boundary in region.Boundaries)
                                    {
                                        Gizmos.color = new Color(0.75f, 0.25f, 0.75f, 0.2f);
                                        Vector3 boundaryPosition = new Vector3(boundary.X+0.5F, boundary.Y+0.5F);
                                        Gizmos.DrawCube(boundaryPosition, Vector3.one);
                                    }
                                }
                            }
                        }
                        index++;
                    }
                }
            }        
        }
        #endregion
    }
}
