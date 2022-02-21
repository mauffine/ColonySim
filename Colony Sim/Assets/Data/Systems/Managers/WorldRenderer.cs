using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ColonySim.World;
using ColonySim.Entities;
using ColonySim.LoggingUtility;
using ILogger = ColonySim.LoggingUtility.ILogger;
using ColonySim.Helpers;
using ColonySim.Systems;
using System;
using System.Linq;
using ColonySim.World.Tiles;

namespace ColonySim.Entities
{
/* [0]BASE
 * [1-2]TILE - Water, Dirt, Floors, Grass
 * [3-4]FURNITURE - Chairs, Tables, Lamps
 * [5-6]OBJECTS - Entities, Food, Containers
 * [7-8]CHARACTERS - Players, Mobs, Wounds
 * [9-10]CONSTRUCTS - Walls, Windows, Trees, Buildings
 * [11]ABOVE - Tree Foliage, Rooves, Flying Creatures
 * [12]SKY - Clouds, Sunlight
 * 
 * SHADOWS - Rendered at layer; IE. Characters cast shadows 
 * over a table, but not over a wall.
 */
    public enum RenderLayer
    {
        BASE = 0,
        TILE = 1,
        TILEDETAIL = TILE + 1,
        FURNITURE = TILEDETAIL + 1,
        FURNITUREDETAIL = FURNITURE + 1,
        OBJECTS = FURNITUREDETAIL + 1,
        OBJECTDETAIL = OBJECTS + 1,
        CHARACTERS = FURNITUREDETAIL + 1,
        CHARACTERDETAIL = CHARACTERS + 1,
        CONSTRUCTS = CHARACTERDETAIL + 1,
        CONSTRUCTDETAIL = CONSTRUCTS + 1,
        ABOVE = CONSTRUCTDETAIL + 1,
        SKY = ABOVE + 1
    }
}

namespace ColonySim.Rendering
{
    public class WorldRenderer : ILogger, IEntityTaskManager
    {
        #region Static
        private static WorldRenderer instance;
        public static WorldRenderer Get => instance;
        public List<string> Logs { get; set; } = new List<string>();
        public LoggingLevel LoggingLevel { get => _loggingLevel; set => _loggingLevel = value; }
        private LoggingLevel _loggingLevel = WorldSystem.Get._rendererLogging;
        public bool Stamp { get => _stamp; set => _stamp = value; }
        private bool _stamp = false;
        #endregion

        public Transform TileMapTransform;

        public WorldRenderer()
        {
            instance = this;
        }

        private Dictionary<IWorldChunk, RenderedChunk> RenderedChunks = new Dictionary<IWorldChunk, RenderedChunk>();
        private List<IRenderObject> renderObjects_dirty = new List<IRenderObject>();
        private List<IRenderObject> renderMeshQueue = new List<IRenderObject>();

        public void Tick()
        {
            RenderDirty();
            RenderMeshes();
        }

        private void RenderDirty()
        {
            IRenderObject[] renderQueue = renderObjects_dirty.ToArray();
            renderObjects_dirty.Clear();
            if (renderQueue != null)
            {
                foreach (var dirtyObj in renderQueue)
                {
                    dirtyObj.RenderDirty();
                }
            }            
        }

        private void RenderMeshes()
        {
            foreach (var RenderObject in renderMeshQueue)
            {
                RenderObject.RenderMeshes();
            }
        }

        public RenderedChunk RenderNewChunk(IWorldChunk ChunkData)
        {
            RenderedChunk _chunk = new RenderedChunk(TileMapTransform, ChunkData);
            RenderedChunks.Add(ChunkData, _chunk);
            _chunk.SetDirty();
            return _chunk;
        }

        public void RenderObject_SetDirty(IRenderObject RenderObject)
        {
            if(!renderObjects_dirty.Contains(RenderObject)) renderObjects_dirty.Add(RenderObject);
        }

        public void RenderObject_AddToMeshQueue(IRenderObject RenderObject)
        {
            if (!renderMeshQueue.Contains(RenderObject)) { renderMeshQueue.Add(RenderObject); }
        }

        public bool TaskResponse(IEntityTaskWorker Worker, EntityTask Task)
        { return true; }

        #region Helper Functions

        public static void SetChunkDirty(RenderedChunk Chunk) 
        { instance.RenderObject_SetDirty(Chunk); }

        // Set Tile Dirty
        public static void SetTileDirty(ITileData Data)
        { instance.instance_SetTileDirty(Data.Coordinates); }

        public static void SetTileDirty(WorldPoint Coordinates)
        { instance.instance_SetTileDirty(Coordinates); }

        public static void SetTileDirty(RenderedTile RenderObject)
        { instance.RenderObject_SetDirty(RenderObject); }

        private void instance_SetTileDirty(WorldPoint Coordinates)
        {  RenderObject_SetDirty(RenderedChunks[GetChunkData(Coordinates)].GetTile(Coordinates)); }

        // Set Entity Dirty
        public static void SetEntityDirty(IEntity EntityData, ITileData TileData)
        { instance.instance_SetEntityDirty(EntityData, TileData); }

        public static void SetEntityDirty(RenderedEntity RenderedObject)
        { instance.RenderObject_SetDirty(RenderedObject); }

        private void instance_SetEntityDirty(IEntity EntityData, ITileData TileData)
        { RenderObject_SetDirty(GetRenderedEntity(EntityData, TileData)); }

        // Get Chunk Data

        private IWorldChunk GetChunkData(WorldPoint Coordinates)
        { return WorldSystem.Get.GetChunk(Coordinates); }

        public IWorldChunk GetChunkData(RenderedChunk RenderObject)
        { return WorldSystem.Get.GetChunk(RenderObject.Coordinates); }

        private RenderedChunk GetRenderedChunk(IWorldChunk ChunkData)
        { return RenderedChunks[ChunkData]; }

        // Get Tile Data
        private ITileData GetTileData(WorldPoint Coordinates)
        { return WorldSystem.Get.GetTileData(Coordinates); }

        public ITileData GetTileData(RenderedTile RenderObject)
        { return WorldSystem.Get.GetTileData(RenderObject.Coordinates); }

        private RenderedTile GetRenderedTile(ITileData TileData)
        { return RenderedChunks[GetChunkData(TileData.Coordinates)].GetTile(TileData.Coordinates); }

        public RenderedTile GetRenderedTile(WorldPoint Coordinates)
        { return RenderedChunks[GetChunkData(Coordinates)].GetTile(Coordinates); }

        // Get Entity Data

        private IEntity GetEntityData(WorldPoint Coordinates, EntityID ID)
        { return GetTileData(Coordinates).Container.GetEntity(ID); }

        private IEntity GetEntityData(RenderedEntity RenderObject)
        { return GetTileData(RenderObject.Coordinates).Container.GetEntity(RenderObject.ID); }

        private RenderedEntity GetRenderedEntity(IEntity EntityData, ITileData TileData) 
        { return GetRenderedTile(TileData).GetRenderedEntity(EntityData); }

        // Logging
        public static void Verbose(string text) => instance.Verbose(text);
        public static void Debug(string text) => instance.Debug(text);
        public static void Notice(string text) => instance.Notice(text);
        public static void Warning(string text) => instance.Warning(text);
        public static void Error(string text) => instance.Error(text);

        #endregion
    }

    public interface IRenderObject
    {
        WorldPoint Coordinates { get; }
        /// <summary>
        /// Set for Render
        /// </summary>
        void SetDirty();
        /// <summary>
        /// Re-Render the item
        /// </summary>
        void RenderDirty();
        /// <summary>
        /// Render the relevant meshes
        /// </summary>
        void RenderMeshes();
        bool Rendering { get; }

        void OnRenderUpdate(Action<IRenderObject> act);
        void CancelOnRenderUpdate(Action<IRenderObject> act);
    }

    public class RenderedChunk : IRenderObject
    {
        public GameObject Object;
        public RenderedTile[,] RenderedTiles;
        public IEnumerable<RenderedTile> GetRenderedTiles() {
            for (int x = 0; x < RenderedTiles.GetLength(0); x++) {
                for (int y = 0; y < RenderedTiles.GetLength(1); y++) {
                    yield return RenderedTiles[x, y];
                }
            }
        }
        public WorldPoint Coordinates { get => coordinates; }
        private ChunkLocation coordinates;
        public bool Rendering { get; }

        private Action<IRenderObject> renderUpdateEvent;

        public RenderedChunk(Transform Parent, IWorldChunk ChunkData)
        {
            WorldRenderer.Get.RenderObject_AddToMeshQueue(this);

            GameObject _go = new GameObject();
            _go.transform.SetParent(Parent);
            _go.name = $"Chunk ({ChunkData.Coordinates.X}-{ChunkData.Coordinates.Y})";
            Object = _go;

            this.coordinates = ChunkData.Coordinates;

            WorldRenderer.Verbose($"Created Render Chunk::{_go.name}");

            InitialiseRenderedTiles();
        }

        private void InitialiseRenderedTiles()
        {
            RenderedTiles = new RenderedTile[WorldSystem.CHUNK_SIZE, WorldSystem.CHUNK_SIZE];
            for (int x = 0; x < RenderedTiles.GetLength(0); x++) {
                for (int y = 0; y < RenderedTiles.GetLength(1); y++) {
                    RenderedTiles[x, y] = new RenderedTile(Object.transform, new WorldPoint(Coordinates.X + x, Coordinates.Y + y));
                }
            }
        }

        public RenderedTile GetTile(LocalPoint Coordinates)
        { 
            if(Coordinates.X >= 0 && Coordinates.Y >= 0 
                && Coordinates.X <= RenderedTiles.GetLength(0)
                && Coordinates.Y <= RenderedTiles.GetLength(1))
                return RenderedTiles[Coordinates.X, Coordinates.Y];
            return null;
        }

        public void RenderMeshes()
        {
            if (Rendering)
            {
                foreach (var RenderedTile in GetRenderedTiles())
                {
                    WorldRenderer.Debug($"Building Mesh For Tile {RenderedTile.Object.name}");
                    RenderedTile.RenderMeshes();
                    
                }
            }            
        }

        public void RenderDirty() { renderUpdateEvent?.Invoke(this); }

        public void SetDirty()
        {
            foreach (var RenderedTile in GetRenderedTiles())
            {
                WorldRenderer.SetTileDirty(RenderedTile);
            }
            WorldRenderer.SetChunkDirty(this);
        }

        public void OnRenderUpdate(Action<IRenderObject> act)
        {
            renderUpdateEvent += act;
        }

        public void CancelOnRenderUpdate(Action<IRenderObject> act)
        {
            renderUpdateEvent -= act;
        }
    }

    public class RenderedTile : IRenderObject, IEntityTaskManager
    {
        public GameObject Object;
        public Dictionary<EntityID, RenderedEntity> RenderedEntities = new Dictionary<EntityID, RenderedEntity>();
        public IEnumerable<RenderedEntity> GetRenderedEntities()
        {
            foreach (var kv in RenderedEntities)
            {
                yield return kv.Value;
            }
        }
        public WorldPoint Coordinates { get => coordinates; }
        private readonly WorldPoint coordinates;
        public bool Rendering { get; } = true;

        private Action<IRenderObject> renderUpdateEvent;
        private int tileContentsHash;



        public RenderedTile(Transform Parent, WorldPoint Coordinates)
        {
            GameObject _go = new GameObject();
            _go.transform.SetParent(Parent);
            _go.transform.SetPositionAndRotation(new Vector3(Coordinates.X, Coordinates.Y), Quaternion.identity);
            _go.name = "Tile";
            Object = _go;

            this.coordinates = Coordinates;
        }

        public void SetDirty()
        {
            foreach (var RenderedEntityPair in RenderedEntities)
            {
                WorldRenderer.SetEntityDirty(RenderedEntityPair.Value);
            }
            WorldRenderer.SetTileDirty(this);
        }

        public void RenderDirty()
        {
            WorldRenderer.Debug($"Rendering Tile {Coordinates}");
            renderUpdateEvent?.Invoke(this);
            ITileData Data = WorldRenderer.Get.GetTileData(this);

            // Update the tile name
            string _name = "Unknown Tile";
            Task_GetTileName NameDetermination = new Task_GetTileName(this);
            Data.Container.AssignTask(NameDetermination);

            if (NameDetermination.Completed)
            {
                _name = NameDetermination.Name;
            }
            Object.name = $"{_name} [{Coordinates.X}-{Coordinates.Y}]";

            // Update the tile's entities
            if (RenderedEntities != null)
            {
                int p_tileContentsHash = tileContentsHash;
                tileContentsHash = Data.Container.GetHashCode();
                //Debug.Log($"Tile Hash Code: {p_tileContentsHash} / {tileContentsHash}");

                // If the list of entities has changed
                if (tileContentsHash != p_tileContentsHash)
                {
                    var _renderedEntities = RenderedEntities;
                    foreach (var RenderedEntity in _renderedEntities)
                    {
                        // If a rendered entity has been removed
                        if (!Data.Container.HasEntity(RenderedEntity.Key))
                        {
                            GameObject.Destroy(RenderedEntity.Value.Object);
                            RenderedEntities.Remove(RenderedEntity.Key);
                        }
                    }

                    foreach (IEntity entity in Data.Container.TileEntities())
                    {
                        // If there's a new entity to render
                        if (!RenderedEntities.ContainsKey(entity.ID)){
                            RenderedEntity renderedEntity = new RenderedEntity(Object.transform, this, entity);
                            RenderedEntities.Add(entity.ID, renderedEntity);
                        }
                    }
                    SetDirty();
                }
            }
        }

        public RenderedEntity GetRenderedEntity(IEntity EntityData)
        { return RenderedEntities[EntityData.ID]; }

        public RenderedEntity GetRenderedEntity(EntityID ID)
        { return RenderedEntities[ID]; }

        public bool TaskResponse(IEntityTaskWorker Worker, EntityTask Task)
        { return true; }

        public void RenderMeshes()
        {
            if (Rendering)
            {
                foreach (var RenderedEntity in RenderedEntities)
                {
                    WorldRenderer.Debug($"Building Mesh For Entity {RenderedEntity.Value.Object.name}");
                    RenderedEntity.Value.RenderMeshes();
                }
            }           
        }

        public void OnRenderUpdate(Action<IRenderObject> act)
        {
            renderUpdateEvent += act;
        }

        public void CancelOnRenderUpdate(Action<IRenderObject> act)
        {
            renderUpdateEvent -= act;
        }
    }

    public class RenderedEntity : IRenderObject, IEntityTaskManager
    {
        public GameObject Object;
        public MeshRenderer Renderer;
        public MeshFilter MeshFilter;
        public EntityID ID;
        public WorldPoint Coordinates { get => coordinates; }
        private readonly WorldPoint coordinates;
        public bool Rendering { get; } = true;

        public Material Material;
        public Texture2D Texture;
        public Color Color;
        public IEntityGraphics GraphicsDef;


        private Action<IRenderObject> renderUpdateEvent;
        private readonly RenderedTile Tile;
        private MeshData meshData;
        private bool[] readingFromNeighbours;

        public RenderedEntity(Transform Parent, RenderedTile Tile, IEntity Data)
        {
            WorldRenderer.Debug($"Creating Render Object For Entity {Tile.Coordinates}({Data.ID})");
            GameObject _go = new GameObject();
            _go.transform.SetParent(Parent);
            _go.name = $"{Data.DefName} #{Data.ID}";
            _go.transform.localPosition = Vector2.zero;
            Renderer = _go.AddComponent<MeshRenderer>();
            MeshFilter = _go.AddComponent<MeshFilter>();

            Object = _go;

            this.ID = Data.ID;
            this.Tile = Tile;
            this.coordinates = Tile.Coordinates;

            if (Data.EntityGraphicsDef != null)
            {
                GraphicsDef = Data.EntityGraphicsDef;
                this.Material = new Material(ResourceManager.LoadEntityMaterial(GraphicsDef.MaterialID));
                _go.transform.localPosition = new Vector3(_go.transform.localPosition.x, _go.transform.localPosition.y, -(int)GraphicsDef.Layer);
            }
        }

        public void SetDirty()
        { WorldRenderer.SetEntityDirty(this); }

        public void RenderDirty()
        {
            WorldRenderer.Debug($"Rendering Dirty Entity {Tile.Coordinates}({ID})");
            renderUpdateEvent?.Invoke(this);
            Renderer.enabled = Rendering;
            BuildMesh();
        }

        public void RenderMeshes()
        {
            Renderer.enabled = Rendering;
        }

        private void BuildMesh()
        {
            if (GraphicsDef != null)
            {
                EntityTextureSettings TextureSettings = GraphicsDef.GetTexture(WorldRenderer.Get.GetTileData(Tile));
                if (TextureSettings != null)
                {
                    WorldRenderer.Debug($"Building Entity Mesh {Coordinates} With Texture ({TextureSettings.TextureID})");
                    if (readingFromNeighbours != TextureSettings.ReadFromNeighbours)
                    {
                        if (TextureSettings.ReadFromNeighbours != null)
                        {
                            if (readingFromNeighbours != null)
                            {
                                for (int i = 0; i < readingFromNeighbours.Length; i++)
                                {
                                    // If we need to subscribe
                                    if (readingFromNeighbours[i] && !TextureSettings.ReadFromNeighbours[i])
                                    {
                                        Vector2Int neighbourPos = AdjacentTileData.ToCoordinate[i];
                                        RenderedTile Neighbour = WorldRenderer.Get.GetRenderedTile(
                                            new WorldPoint(Coordinates.X + neighbourPos.x, Coordinates.Y + neighbourPos.y));
                                        if (Neighbour != null)
                                        {
                                            Neighbour.OnRenderUpdate(x => SetDirty());
                                        }                                        
                                    }
                                    // If we need to unsubscribe 
                                    else if (!TextureSettings.ReadFromNeighbours[i] && readingFromNeighbours[i])
                                    {
                                        Vector2Int neighbourPos = AdjacentTileData.ToCoordinate[i];
                                        RenderedTile Neighbour = WorldRenderer.Get.GetRenderedTile(
                                            new WorldPoint(Coordinates.X + neighbourPos.x, Coordinates.Y + neighbourPos.y));
                                        if (Neighbour != null)
                                        {
                                            Neighbour.CancelOnRenderUpdate(x => SetDirty());
                                        }                                        
                                    }
                                }                               
                            }
                            else
                            {
                                readingFromNeighbours = TextureSettings.ReadFromNeighbours;
                                for (int i = 0; i < readingFromNeighbours.Length; i++)
                                {
                                    Vector2Int neighbourPos = AdjacentTileData.ToCoordinate[i];
                                    RenderedTile Neighbour = WorldRenderer.Get.GetRenderedTile(
                                        new WorldPoint(Coordinates.X + neighbourPos.x, Coordinates.Y + neighbourPos.y));
                                    if (Neighbour != null)
                                    {
                                        Neighbour.OnRenderUpdate(x => SetDirty());
                                    }
                                    
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < readingFromNeighbours.Length; i++)
                            {
                                Vector2Int neighbourPos = AdjacentTileData.ToCoordinate[i];
                                RenderedTile Neighbour = WorldRenderer.Get.GetRenderedTile(
                                    new WorldPoint(Coordinates.X + neighbourPos.x, Coordinates.Y + neighbourPos.y));
                                if (Neighbour != null)
                                {
                                    Neighbour.CancelOnRenderUpdate(x => SetDirty());
                                }                               
                            }
                            readingFromNeighbours = null;
                        }                      
                    }
                    this.Texture = ResourceManager.LoadEntityTexture(TextureSettings.TextureID);
                    this.Material.mainTexture = Texture;

                    if (TextureSettings.Angle != 0 && TextureSettings.Angle != 360)
                    {
                        Debug.Log($"Rotating tile {Coordinates} to {TextureSettings.Angle}");
                        Vector3 target = Vector3.zero;
                        switch (Mathf.Abs(TextureSettings.Angle))
                        {
                            case 90:
                                target = Vector3.right;
                                break;
                            case 180:
                                target = Vector3.right + Vector3.up;
                                break;
                            case 270:
                                target = Vector3.up;
                                break;
                            case -90:
                                target = Vector3.right;
                                break;
                            case -180:
                                target = Vector3.right + Vector3.up;
                                break;
                            case -270:
                                target = Vector3.right;
                                break;
                        }
                        Object.transform.eulerAngles = Vector3.forward * TextureSettings.Angle;
                        Object.transform.position = Object.transform.position + target;

                    }
                    
                    if (meshData == null)
                    {
                        meshData = new MeshData(1, MeshFlags.UV);
                    }
                    int vIndex = meshData.vertices.Count;

                    meshData.vertices.Add(new Vector3(0, 0));
                    meshData.vertices.Add(new Vector3(0, 1));
                    meshData.vertices.Add(new Vector3(1, 1));
                    meshData.vertices.Add(new Vector3(1, 0));

                    meshData.UVs.Add(new Vector2(0, 0));
                    meshData.UVs.Add(new Vector2(0, 1));
                    meshData.UVs.Add(new Vector2(1, 1));
                    meshData.UVs.Add(new Vector2(1, 0));

                    meshData.AddTriangle(vIndex, 0, 1, 2);
                    meshData.AddTriangle(vIndex, 0, 2, 3);

                    meshData.Build();

                    MeshFilter.mesh = meshData.mesh;
                    Renderer.material = Material;
                }
            }
        }

        public void OnRenderUpdate(Action<IRenderObject> act)
        {
            renderUpdateEvent += act;
        }

        public void CancelOnRenderUpdate(Action<IRenderObject> act)
        {
            renderUpdateEvent -= act;
        }

        public bool TaskResponse(IEntityTaskWorker Worker, EntityTask Task)
        { return true; }
    }
}
