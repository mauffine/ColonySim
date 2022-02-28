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

        private static NavMesh navMesh;

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

        public static void InvalidateNavigation(WorldPoint Coordinates)
        {
            instance.instance_InvalidateNavigation(Coordinates.X, Coordinates.Y);
        }

        private void instance_InvalidateNavigation(int X, int Y)
        {
            this.Verbose($"Regenerating Dirty Node ({X},{Y})");
            navMesh.RefreshNode(X, Y);
            UPDATE_GIZMOS();
        }

        public static Stack<Node> Path(WorldPoint Start, WorldPoint End)
        {
            instance.Notice($"Navigating From {Start} to {End}");
            var heuristic = new AStar(Start, End);
            return heuristic.Path();
        }

        public static Coroutine RunCoroutine(IEnumerator coroutine)
        {
            return instance.StartCoroutine(coroutine);
        }

        #region Helpers

        public static Node Node(WorldPoint Point) => navMesh[Point.X, Point.Y];
        public static Node Node(int X, int Y) => navMesh[X, Y];
        public static Node[] Adjacent(WorldPoint Point) => Adjacent(Point.X, Point.Y);
        public static Node[] Adjacent(int X, int Y)
        {
            List<Node> adjacentNodes = new List<Node>();
            foreach (var pos in TileManager.AdjacentCoordinates(X, Y))
            {
                if (navMesh[pos.X, pos.Y] != null)
                {
                    adjacentNodes.Add(navMesh[pos.X,pos.Y]);
                }
            }
            return adjacentNodes.ToArray();
        }

        #endregion

#if DEBUG

        #region Gizmos

        [Conditional("DEBUG")]
        private void UPDATE_GIZMOS()
        {
            this.Verbose($"GIZMO REFRESH");
            nodeGizmoData = null;
        }

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
                                        for (int i = 0; i < node.Edges.Length; i++)
                                        {
                                            if (node.Edges[i] != null)
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
