using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ColonySim.LoggingUtility;
using ILogger = ColonySim.LoggingUtility.ILogger;
using ColonySim.Rendering;
using ISystem = ColonySim.Systems.System;
using UnityEditor;
using ColonySim.Systems;

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
        private bool DrawGizmoChunks;
        [SerializeField]
        private bool DrawGizmoNoiseMap;

        public override void Init()
        {
            this.Notice("<color=blue>[World System Init]</color>");
            instance = this;
            _world = new GameWorld(5, 5);
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

        public override void Tick()
        {
            base.Tick();
            Simulation.Tick();
            Renderer.Tick();
        }

        public WorldPoint VectorToWorldPoint(Vector3 worldPos)
        {
            int X = Mathf.FloorToInt(worldPos.x);
            int Y = Mathf.FloorToInt(worldPos.y);
            return new WorldPoint(X, Y);
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

                        Vector3 v = new Vector3(Coordinates.X, Coordinates.Y + .5f);
                        Vector3 dif = new Vector3(CursorSystem.Get.currentMousePosition.X - Coordinates.X, CursorSystem.Get.currentMousePosition.Y - Coordinates.Y);
                        if (Mathf.Abs(dif.x) < 3 && Mathf.Abs(dif.y) < 3)
                        {
                            Handles.Label(v, Coordinates.ToString());
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
            }        
        }
    }
}
