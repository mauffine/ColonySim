using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ColonySim.World;

namespace ColonySim.Systems
{
    public class WorldSystem : System
    {
        #region Static
        private static WorldSystem instance;
        public static WorldSystem Get() => instance;
        #endregion

        public Sprite ConcreteTileSprite;

        public const int CHUNK_SIZE = 5;
        public GameWorld World;

        private WorldRenderer Renderer;
        public override void Init()
        {
            instance = this;
            World = new GameWorld(3, 3);
            Renderer = new WorldRenderer();

            foreach (var Chunk in World.GetChunks())
            {
                Renderer.RenderChunk(Chunk);
            }
            base.Init();
        }
    }
}
