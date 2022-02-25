using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ColonySim.World;
using ColonySim.LoggingUtility;
using ILogger = ColonySim.LoggingUtility.ILogger;
using ColonySim.Rendering;

namespace ColonySim.Systems
{
    public class WorldSystem : System, ILogger
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

        public Transform TileMapTransform;
        public Sprite ConcreteTileSprite;

        public const int CHUNK_SIZE = 8;
        public GameWorld World;

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
            World = new GameWorld(10, 10);
            World.GenerateWorldChunks();
            World.WorldGeneration(Time.realtimeSinceStartup);
            Renderer = new WorldRenderer();
            Renderer.TileMapTransform = this.TileMapTransform;
            Simulation = new WorldSimulation();

            foreach (var Chunk in World.GetChunks())
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

        public ITileData GetTileData(WorldPoint worldPos)
        {
            ITileData Data = World.GetTileData(worldPos);
            return Data;
        }

        public IWorldChunk GetChunk(WorldPoint worldPos)
        {
            return World.GetChunk(worldPos);
        }

        public void OnDrawGizmos()
        {
            if (Initialized)
            {
                if (DrawGizmoTiles)
                {
                    foreach (var tile in World)
                    {
                        WorldPoint Coordinates = tile.Coordinates;
                        Gizmos.DrawWireCube(
                            new Vector3(Coordinates.X+.5f, Coordinates.Y+.5f),
                            Vector3.one);
                    }
                }
                if (DrawGizmoChunks)
                {
                    foreach (var chunk in World.GetChunks())
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
                    foreach (var tile in World)
                    {
                        WorldPoint Coordinates = tile.Coordinates;
                        float h = World.groundNoiseMap[Coordinates.X + Coordinates.Y * World.Size.x];
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
