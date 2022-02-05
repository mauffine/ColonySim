using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ColonySim.World;

namespace ColonySim.Systems
{
    public class WorldRenderer
    {
        #region Static
        private static WorldRenderer instance;
        public static WorldRenderer Get() => instance;
        #endregion

        public const int CHUNK_SIZE = 5;
        private WorldSystem WorldSystem;
        public WorldRenderer()
        {
            instance = this;
            WorldSystem = WorldSystem.Get();
        }

        public void RenderChunk(IWorldChunk Chunk)
        {
            for (int x = 0; x < Chunk.Tiles.Length; x++)
            {
                TileData[] y_array = Chunk.Tiles[x];
                for (int y = 0; y < y_array.Length; y++)
                {
                    RenderTile(Chunk, y_array[y]);
                }
            }
        }

        private void RenderTile(IWorldChunk Chunk, TileData tile)
        {
            GameObject _go = new GameObject();
            (int, int) coords = Chunk.WorldCoordinate(tile);
            _go.name = string.Format("Tile [{0}-{1}]", coords.Item1, coords.Item2);
            SpriteRenderer renderer = _go.AddComponent<SpriteRenderer>();
            _go.transform.position = new Vector3(coords.Item1, coords.Item2);
            renderer.sprite = WorldSystem.ConcreteTileSprite;

        }
    }
}
