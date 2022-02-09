using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ColonySim.World;
using ColonySim.Entities;
using ColonySim.LoggingUtility;
using ILogger = ColonySim.LoggingUtility.ILogger;

namespace ColonySim.Systems
{
    public class WorldRenderer : ILogger, IEntityTaskManager
    {
        #region Static
        private static WorldRenderer instance;
        public static WorldRenderer Get => instance;
        public List<string> Logs { get; set; } = new List<string>();
        public LoggingLevel LoggingLevel { get => _loggingLevel; set => _loggingLevel = value; }
        [SerializeField]
        private LoggingLevel _loggingLevel = LoggingLevel.Warning;
        public bool Stamp { get => _stamp; set => _stamp = value; }
        [SerializeField]
        private bool _stamp = false;
        #endregion

        public Transform TileMapTransform;

        public WorldRenderer()
        {
            instance = this;
        }

        public Dictionary<IWorldChunk, RenderedChunk> RenderedChunks = new Dictionary<IWorldChunk, RenderedChunk>();

        public void RenderChunk(IWorldChunk Chunk)
        {
            RenderedChunk _chunk = new RenderedChunk();
            _chunk.Create(TileMapTransform, Chunk);
            RenderedChunks.Add(Chunk, _chunk);
            for (int x = 0; x < Chunk.TileData.Length; x++)
            {
                TileData[] y_array = Chunk.TileData[x];
                for (int y = 0; y < y_array.Length; y++)
                {
                    var renderedTile = RenderNewTile(_chunk, y_array[y]);
                    _chunk.SetTile(x, y, renderedTile);
                }
            }
        }

        private RenderedTile RenderNewTile(RenderedChunk Chunk, TileData Data)
        {
            RenderedTile Tile = new RenderedTile();
            Tile.Create(Chunk.Object.transform, Data.Coordinates);
            ITileContainer _container = Data.Container;

            string _name = "Unknown Tile";
            Task_GetTileName NameDetermination = new Task_GetTileName(this);
            _container.AssignTask(NameDetermination);

            if (NameDetermination.Completed)
            {
                _name = NameDetermination.Name;
            }           
            Tile.Object.name = $"{_name} [{Data.Coordinates.X}-{Data.Coordinates.Y}]";

            foreach (var entity in _container.AllEntities())
            {
                Tile.RenderedEntities.Add(RenderNewEntity(Tile, _container, entity));
            }

            return Tile;
        }

        private RenderedEntity RenderNewEntity(RenderedTile Tile, ITileContainer ContainerData, IEntity Data)
        {
            RenderedEntity Entity = new RenderedEntity();
            Entity.Create(Tile.Object.transform, Data, ContainerData.GetEntityID(Data));

            Module_EntitySprite SpriteModule = Data.FindModule<Module_EntitySprite>();
            if (SpriteModule != null)
            {
                Texture2D texture = ResourceManager.GetEntityTileSprite(SpriteModule.TextureName);
                if (texture != null)
                {
                    Entity.SetTexture(texture);
                }
            }
            return Entity;
        }

        public void RenderTile(WorldPoint Coordinates)
        {
            Debug.Log("Rendering Tile");
            IWorldChunk Chunk = WorldSystem.Get.GetChunk(Coordinates);
            if (Chunk != null)
            {
                RenderedChunk RenderedChunk = RenderedChunks[Chunk];
                ITileData TileData = WorldSystem.Get.GetTileData(Coordinates);
                RenderedTile RenderedTile = RenderedChunk.GetTile(Coordinates);

                foreach (var entity in RenderedTile.RenderedEntities)
                {
                    Module_EntitySprite SpriteModule = TileData.Container.GetEntity(entity.ID).FindModule<Module_EntitySprite>();
                    if (SpriteModule != null)
                    {
                        Texture2D texture = ResourceManager.GetEntityTileSprite(SpriteModule.TextureName);
                        if (texture != null)
                        {
                            entity.SetTexture(texture);
                        }
                    }
                }
            }
        }

        public bool TaskResponse(IEntityTaskWorker Worker, EntityTask Task)
        { return true; }
    }

    public class RenderedChunk
    {
        public GameObject Object;
        public RenderedTile[][] RenderedTiles;

        public RenderedChunk()
        {
            RenderedTiles = new RenderedTile[WorldSystem.CHUNK_SIZE][];
            for (int i = 0; i < RenderedTiles.Length; i++)
            {
                RenderedTiles[i] = new RenderedTile[WorldSystem.CHUNK_SIZE];
            }
        }

        public void Create(Transform Parent, IWorldChunk Chunk)
        {
            GameObject _go = new GameObject();
            _go.transform.SetParent(Parent);
            _go.name = $"Chunk ({Chunk.Coordinates.X}-{Chunk.Coordinates.Y})";
            Object = _go;
        }

        public void SetTile(int X, int Y, RenderedTile Set)
        {
            RenderedTiles[X][Y] = Set;
        }

        public RenderedTile GetTile(LocalPoint Coordinates)
        {
            return RenderedTiles[Coordinates.X][Coordinates.Y];
        }
    }

    public class RenderedTile
    {
        public GameObject Object;
        public List<RenderedEntity> RenderedEntities = new List<RenderedEntity>();

        public void Create(Transform Parent, WorldPoint Coordinates)
        {
            GameObject _go = new GameObject();
            _go.transform.SetParent(Parent);
            _go.transform.SetPositionAndRotation(new Vector3(Coordinates.X, Coordinates.Y), Quaternion.identity);
            Object = _go;
        }
    }

    public class RenderedEntity
    {
        public GameObject Object;
        public SpriteRenderer Renderer;
        public EntityID ID;

        public void Create(Transform Parent, IEntity Data, EntityID ID)
        {
            GameObject _go = new GameObject();
            _go.transform.SetParent(Parent);
            _go.name = $"{Data.Name} #{ID.ID}";
            _go.transform.localPosition = Vector2.zero;
            Renderer = _go.AddComponent<SpriteRenderer>();
            this.ID = ID;
            Object = _go;          
        }

        public void SetTexture(Texture2D Texture)
        {
            Sprite _sprite = Sprite.Create(Texture, new Rect(0.0f, 0.0f, Texture.width, Texture.height), Vector2.zero, 500);
            Renderer.sprite = _sprite;
        }
    }
}
