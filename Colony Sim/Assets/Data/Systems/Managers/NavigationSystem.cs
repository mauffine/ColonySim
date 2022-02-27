using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ColonySim.LoggingUtility;
using ILogger = ColonySim.LoggingUtility.ILogger;
using ColonySim.Rendering;
using ColonySim.World;
using ColonySim.World.Tiles;
using System.Diagnostics;

namespace ColonySim.Systems.Navigation
{
    public class NavigationSystem : System, ILogger
    {
        #region Static
        private static NavigationSystem instance;
        public static NavigationSystem Get => instance;
        public List<string> Logs { get; set; } = new List<string>();
        public LoggingLevel LoggingLevel { get => _loggingLevel; set => _loggingLevel = value; }
        [SerializeField]
        private LoggingLevel _loggingLevel = LoggingLevel.Warning;
        public LoggingPriority LoggingPriority { get => _loggingPriority; set => _loggingPriority = value; }
        [SerializeField]
        private LoggingPriority _loggingPriority = LoggingPriority.AlwaysShow;
        public bool Stamp { get => _stamp; set => _stamp = value; }
        public string LoggingPrefix => "<color=yellow>[NAVSYS]</color>";
        [SerializeField]
        private bool _stamp = false;
        #endregion

        [SerializeField]
        private bool drawNavMeshGizmo = false;

        private NavMesh navMesh;

        public override void Init()
        {
            this.Notice("<color=blue>[Navigation System Init]</color>");
            instance = this;
            base.Init();
        }

        public override void OnInitialized()
        {
            base.OnInitialized();
            navMesh = new NavMesh();
            navMesh.GenerateNavMesh(WorldSystem.World);
        }

        public static void SetTileDirty(WorldPoint Coordinates)
        {
            instance.instance_SetTileDirty(Coordinates.X, Coordinates.Y);
        }

        private void instance_SetTileDirty(int X, int Y)
        {
            this.Verbose($"Regenerating Dirty Node ({X},{Y})");
            navMesh.RefreshNode(X, Y);
            UPDATE_GIZMOS();
        }

        [Conditional("DEBUG")]
        private void UPDATE_GIZMOS()
        {
            this.Verbose($"GIZMO REFRESH");
            nodeGizmoData = null;
        }

#if DEBUG

        #region Gizmos

        private Dictionary<Vector3, Vector3?[]> nodeGizmoData;

        public void OnDrawGizmos()
        {
            if (Initialized)
            {
                if (navMesh != null)
                {
                    if (drawNavMeshGizmo)
                    {
                        if (nodeGizmoData == null)
                        {
                            nodeGizmoData = new Dictionary<Vector3, Vector3?[]>();
                            for (int x = 0; x < WorldSystem.World.Size.x; x++)
                            {
                                for (int y = 0; y < WorldSystem.World.Size.y; y++)
                                {
                                    var node = navMesh[x, y];
                                    Vector3 nodePos = new Vector3(x + .5f, y + .5f);
                                    if (node != null)
                                    {                                       
                                        Vector3?[] edgePositions = new Vector3?[8];
                                        for (int i = 0; i < node.edges.Length; i++)
                                        {
                                            if (node.edges[i] != null)
                                            {
                                                Vector2 edgeDir = AdjacentTileData.ToCoordinate[i];
                                                edgePositions[i] = new Vector3(nodePos.x + edgeDir.x, nodePos.y + edgeDir.y);
                                            }
                                        }
                                        nodeGizmoData.Add(nodePos, edgePositions);
                                    }
                                    else
                                    {
                                        nodeGizmoData.Add(nodePos, null);
                                    }                                    
                                }
                            }
                        }
                        else
                        {
                            foreach (var kV in nodeGizmoData)
                            {
                                bool validNode = kV.Value != null;
                                if (validNode) Gizmos.color = new Color(0.25f, 0.25f, 1f, 0.8f);
                                else Gizmos.color = new Color(1, 0.25f, 0.25f, 0.8f);
                                Gizmos.DrawCube(kV.Key, new Vector3(0.25f, 0.25f, 0.25f));
                                if (validNode)
                                {
                                    Vector3 dif = new Vector3(CursorSystem.Get.currentMousePosition.X - kV.Key.x, CursorSystem.Get.currentMousePosition.Y - kV.Key.y);
                                    if (Mathf.Abs(dif.x) < 3 && Mathf.Abs(dif.y) < 3)
                                    {
                                        float difAlphaMod = 0.9f - (Mathf.Abs(dif.x) + Mathf.Abs(dif.y)) * 0.15f;
                                        Gizmos.color = new Color(0.25f, 0.25f, 1f, difAlphaMod);
                                        for (int i = 0; i < kV.Value.Length; i++)
                                        {
                                            if (kV.Value[i] != null)
                                            {
                                                Gizmos.DrawLine(kV.Key, (Vector3)kV.Value[i]);
                                            }
                                        }
                                    }
                                }                                                             
                            }
                        }                        
                    }                    
                }
            }
        }

        #endregion
#endif
    }
}
