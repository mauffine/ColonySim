using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ColonySim.World;
using ColonySim.LoggingUtility;
using ILogger = ColonySim.LoggingUtility.ILogger;

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
        #endregion

        public Transform TileMapTransform;
        public Sprite ConcreteTileSprite;

        public const int CHUNK_SIZE = 5;
        public GameWorld World;

        public static WorldRenderer Renderer;
        public static WorldSimulation Simulation;
        public override void Init()
        {
            this.Verbose("<color=blue>[World System Init]</color>");
            instance = this;
            World = new GameWorld(3, 3);
            Renderer = new WorldRenderer();
            Renderer.TileMapTransform = this.TileMapTransform;
            Simulation = new WorldSimulation();

            foreach (var Chunk in World.GetChunks())
            {
                Renderer.RenderChunk(Chunk);
                Simulation.Simulate(Chunk);
            }
            base.Init();
        }

        public override void Tick()
        {
            base.Tick();
            Simulation.Tick();
        }

        public WorldPoint VectorToWorldPoint(Vector3 worldPos)
        {
            int X = Mathf.FloorToInt(worldPos.x);
            int Y = Mathf.FloorToInt(worldPos.y);
            return new WorldPoint(X, Y);
        }

        public ITileData GetTileData(WorldPoint worldPos)
        {
            return World.GetTileData(worldPos);
        }

        public IWorldChunk GetChunk(WorldPoint worldPos)
        {
            return World.GetChunk(worldPos);
        }
    }
}
