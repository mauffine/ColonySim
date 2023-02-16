using ColonySim.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static ColonySim.InputControlMap;
using ColonySim.LoggingUtility;
using ILogger = ColonySim.LoggingUtility.ILogger;
using ColonySim.Entities;

namespace ColonySim.Systems
{
    public class CursorSystem : System, IMouseActions, ILogger
    {
        #region Static
        private static CursorSystem instance;
        public static CursorSystem Get => instance;
        public List<string> Logs { get; set; } = new List<string>();
        public LoggingLevel LoggingLevel { get => _loggingLevel; set => _loggingLevel = value; }
        [SerializeField]
        private LoggingLevel _loggingLevel = LoggingLevel.Warning;
        public LoggingPriority LoggingPriority { get => _loggingPriority; set => _loggingPriority = value; }
        [SerializeField]
        private LoggingPriority _loggingPriority = LoggingPriority.AlwaysShow;
        public bool Stamp { get => _stamp; set => _stamp = value; }
        public string LoggingPrefix => "<color=purple>[CURSOR]</color>";
        [SerializeField]
        private bool _stamp = false;
        #endregion

        private MeshData meshData;
        private Material selectionOverlayMat;

        public override void Init()
        {
            this.Notice("> Cursor System Init <");
            instance = this;
            base.Init();
        }

        public override void OnInitialized()
        {
            base.OnInitialized();
            BuildSelectionOverlayMesh();
        }

        public WorldPoint currentMousePosition;
        public WorldPoint oldMousePosition;
        public static ITileData CursorTile => instance.highlightedTile;
        public ITileData highlightedTile;

        public override void Tick()
        {
            UpdateMousePositions();
            if (highlightedTile == null)
            {
                return;
            }
            RenderSelectionOverlay();
        }

        private void UpdateMousePositions()
        {
            oldMousePosition = currentMousePosition;
            Vector3 mousePos = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            Vector3 mouseVector = ray.GetPoint(-ray.origin.z / ray.direction.z);
            WorldPoint worldPoint = WorldSystem.ToWorldPoint(mouseVector);
            if (worldPoint != currentMousePosition)
            {
                currentMousePosition = worldPoint;
                highlightedTile = WorldSystem.Tile(worldPoint, out cbTileState cbTileState);
                if (cbTileState == cbTileState.OutOfBounds) return;
                this.Debug($"MouseVector {mouseVector} translates to {worldPoint}");
            }
            
        }

        public void OnMousePointer(InputAction.CallbackContext context)
        {
            
        }

        private void RenderSelectionOverlay()
        {
            if (highlightedTile != null)
            {
                Vector3 selectionPos = currentMousePosition;
                Graphics.DrawMesh(
                            meshData.mesh,
                            new Vector3(selectionPos.x, selectionPos.y, -(int)EntityLayer.ABOVE),
                            Quaternion.identity,
                            selectionOverlayMat,
                            0
                         );
            }
            
        }

        private void BuildSelectionOverlayMesh()
        {
            this.selectionOverlayMat = new Material(ResourceManager.LoadEntityMaterial("basic"));
            Texture2D Texture = ResourceManager.LoadUtilityTexture("utility.selection-overlay");
            this.selectionOverlayMat.mainTexture = Texture;
            if(meshData == null) meshData = new MeshData(1, MeshFlags.ALL);
            else { meshData.Clear(); }

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

            meshData.colors.Add(new Color(1, 1, 1, 0.75f));
            meshData.colors.Add(new Color(1, 1, 1, 0.75f));
            meshData.colors.Add(new Color(1, 1, 1, 0.75f));
            meshData.colors.Add(new Color(1, 1, 1, 0.75f));

            meshData.Build();
        }
    }
}
