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
        public bool Stamp { get => _stamp; set => _stamp = value; }
        [SerializeField]
        private bool _stamp = false;

        public LoggingLevel _rendererLogging = LoggingLevel.Warning;
        public LoggingLevel _worldLogging = LoggingLevel.Warning;

        #endregion
        public static WorldRenderer Renderer;
        public static WorldSimulation Simulation;

        public Transform TileMapTransform;
        public Sprite ConcreteTileSprite;

        public const int CHUNK_SIZE = 5;
        public GameWorld World;

        [SerializeField]
        private bool DrawGizmoTIles;
        [SerializeField]
        private bool DrawGizmoChunks;


        public override void Init()
        {
            this.Verbose("<color=blue>[World System Init]</color>");
            instance = this;
            World = new GameWorld(1, 1);
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
                if (DrawGizmoTIles)
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
            }        
        }
    }
}
