using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ColonySim.World;
using ColonySim.Entities;

namespace ColonySim.Systems
{
    public class WorldRenderer
    {
        #region Static
        private static WorldRenderer instance;
        public static WorldRenderer Get() => instance;
        #endregion

        public Transform TileMapTransform;

        public const int CHUNK_SIZE = 5;
        private WorldSystem WorldSystem;
        public WorldRenderer()
        {
            instance = this;
            WorldSystem = WorldSystem.Get;
        }

        public void RenderChunk(IWorldChunk Chunk)
        {
            for (int x = 0; x < Chunk.TileData.Length; x++)
            {
                TileData[] y_array = Chunk.TileData[x];
                for (int y = 0; y < y_array.Length; y++)
                {
                    RenderTile(Chunk, y_array[y]);
                }
            }
        }

        private void RenderTile(IWorldChunk Chunk, TileData tile)
        {
            GameObject _go = new GameObject();
            _go.transform.SetParent(TileMapTransform);
            WorldPoint Coordinates = tile.Coordinates;
            ITileContainer _container = tile.Container;

            string _name = "Unkown Tile";
            Module_TileNameDetermination NameDetermination = _container.FindModule<Module_TileNameDetermination>();
            if (NameDetermination != null)
            {
                _name = NameDetermination.Name;
            }

            EntityTrigger_DetermineName nameEvent = new EntityTrigger_DetermineName();
            _container.Trigger(nameEvent);
            
            _go.name = $"{_name} [{Coordinates}]";

            SpriteRenderer renderer = _go.AddComponent<SpriteRenderer>();
            _go.transform.position = new Vector3(Coordinates.X, Coordinates.Y);

            renderer.sprite = WorldSystem.ConcreteTileSprite;

        }
    }
}
