using UnityEngine;
using System.Collections.Generic;

using ColonySim.LoggingUtility;
using ILogger = ColonySim.LoggingUtility.ILogger;
using ColonySim.Creatures;
using ColonySim.World;
using ColonySim.Rendering;
using System;
using ColonySim.Entities;
using ColonySim.Systems.Tasks;

namespace ColonySim.Systems
{
    public class CreatureController : System, ILogger, IWorldTick
    {
        #region Static
        private static CreatureController instance;
        public static CreatureController Get => instance;
        public List<string> Logs { get; set; } = new List<string>();
        public LoggingLevel LoggingLevel { get => _loggingLevel; set => _loggingLevel = value; }
        [SerializeField]
        private LoggingLevel _loggingLevel = LoggingLevel.Warning;
        public LoggingPriority LoggingPriority { get => _loggingPriority; set => _loggingPriority = value; }
        [SerializeField]
        private LoggingPriority _loggingPriority = LoggingPriority.AlwaysShow;
        public bool Stamp { get => _stamp; set => _stamp = value; }
        public string LoggingPrefix => "<color=red>[CREATURES]</color>";
        [SerializeField]
        private bool _stamp = false;
        #endregion

        [SerializeField]
        private bool drawCharactersGizmo;

        private List<ICreature> simulatedCreatures;

        public static TestCreature TEST_CREATURE => (TestCreature)instance.simulatedCreatures[0];

        public override void Init()
        {
            this.Notice("<color=blue>[Creature Controller Init]</color>");
            instance = this;
            base.Init();
        }

        public override void OnInitialized()
        {
            base.OnInitialized();
            WorldSimulation.Get.Simulate(this);

            CreateCreature(new WorldPoint(3, 3));
        }

        public void CreateCreature(WorldPoint Coordinate)
        {
            TestCreature testCreature = new TestCreature();
            testCreature.SetTilePosition(Coordinate);

            if (simulatedCreatures == null) simulatedCreatures = new List<ICreature>() { testCreature };
            else simulatedCreatures.Add(testCreature);

            WorkSystem.Worker(testCreature);

            RenderedCreature renderObject = new RenderedCreature(testCreature);
            renderObjects_dirty.Add(renderObject);
        }

        public void WorldTick(float delta)
        {
            foreach (var creature in simulatedCreatures)
            {
                creature.WorldTick(delta);
            }
            RenderCreatures();
        }

        #region Rendering

        private readonly List<IRenderObject> renderObjects_dirty = new List<IRenderObject>();
        private readonly List<IRenderObject> renderMeshQueue = new List<IRenderObject>();

        public static void RenderMeshQueue(IRenderObject renderObject)
        {
            instance.renderMeshQueue.Add(renderObject);
        }

        public static void Dirty(IRenderObject renderObject)
        {
            instance.renderObjects_dirty.Add(renderObject);
        }

        private void RenderCreatures()
        {
            // Rebuild Dirty Render Objects
            IRenderObject[] renderQueue = renderObjects_dirty.ToArray();
            renderObjects_dirty.Clear();
            if (renderQueue != null)
            {
                foreach (var dirtyObj in renderQueue)
                {
                    dirtyObj.RenderDirty();
                }
            }

            // Render the meshes

            foreach (var renderObject in renderMeshQueue)
            {
                renderObject.RenderMeshes();
            }
        }

        #endregion


        #region Gizmos

        public void OnDrawGizmos()
        {
            if (Initialized)
            {
                if (simulatedCreatures != null)
                {
                    if (drawCharactersGizmo)
                    {
                        foreach (var creature in simulatedCreatures)
                        {
                            Gizmos.color = new Color(1f, 0.8f, 0.7f, 1f);
                            Vector3 nosePosition = new Vector3(creature.Navigation.Position.x, creature.Navigation.Position.y + 0.2f);
                            Gizmos.DrawCube(creature.Navigation.Position, new Vector3(0.8f, 0.2f, 0.2f));
                            Gizmos.DrawCube(nosePosition, new Vector3(0.1f, 0.2f, 0.1f));

                            Gizmos.color = new Color(0.6f, 1, 0.25f, 1f);
                            Gizmos.DrawCube(creature.Navigation.Position, new Vector3(0.5f, 0.35f, 0.5f));



                        }
                    }
                }
            }
        }

        #endregion
    }

    public interface ICreatureRenderData
    {
        Vector2 RenderPoint { get; }
        Quaternion RenderFacing { get; }
        string RenderTexture { get; }
    }

    public class RenderedCreature : IRenderObject
    {
        public LoggingUtility.ILogger Master => CreatureController.Get;
        public string LoggingPrefix => $"<color=green>[CHARACTER]</color>";

        public WorldPoint Coordinates { get; set; }
        public bool Rendering { get; } = true;

        private readonly ICreatureRenderData Data;

        private Material Material;
        private Color Color;

        private MeshData meshData;
        private Action<IRenderObject> renderUpdateEvent;

        public RenderedCreature(ICreatureRenderData Data)
        {
            this.Data = Data;
            this.Material = new Material(ResourceManager.LoadEntityMaterial("basic"));
            CreatureController.RenderMeshQueue(this);
        }

        public void SetDirty()
        { CreatureController.Dirty(this); }

        public void RenderDirty()
        {
            this.Debug($"Rendering Character..");
            renderUpdateEvent?.Invoke(this);
            BuildMesh();
        }

        public void OnRenderUpdate(Action<IRenderObject> act) { }
        public void CancelOnRenderUpdate(Action<IRenderObject> act) { }

        public void RenderMeshes()
        {
            if (Rendering)
            {
                if (meshData != null)
                {
                    Quaternion rotation = Quaternion.identity;
                    Vector3 position = new Vector3(Data.RenderPoint.x, Data.RenderPoint.y);
                    if (Data.RenderFacing != null && Data.RenderFacing.eulerAngles != Vector3.zero)
                    {
                        //rotation = Data.RenderFacing;                        
                    }

                    Graphics.DrawMesh(
                        meshData.mesh,
                        new Vector3(position.x, position.y, -(int)RenderLayer.CHARACTERS),
                        rotation,
                        this.Material,
                        0
                     );
                }

            }
        }

        private void BuildMesh()
        {
            Texture2D Texture = ResourceManager.LoadCharacterTexture(Data.RenderTexture);
            this.Material.mainTexture = Texture;

            if (meshData == null) meshData = new MeshData(1, MeshFlags.UV);
            else meshData.Clear();

            int vIndex = meshData.vertices.Count;

            meshData.vertices.Add(new Vector3(0, 0));
            meshData.vertices.Add(new Vector3(0, 1));
            meshData.vertices.Add(new Vector3(1, 1));
            meshData.vertices.Add(new Vector3(1, 0));

            meshData.UVs.Add(new Vector3(0, 0));
            meshData.UVs.Add(new Vector2(0, 1));
            meshData.UVs.Add(new Vector2(1, 1));
            meshData.UVs.Add(new Vector2(1, 0));

            meshData.AddTriangle(vIndex, 0, 1, 2);
            meshData.AddTriangle(vIndex, 0, 2, 3);

            meshData.Build();
        }
    }
}