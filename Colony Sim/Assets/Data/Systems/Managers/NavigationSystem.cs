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

        public override void Init()
        {
            this.Notice("<color=blue>[Navigation System Init]</color>");
            instance = this;
            base.Init();
        }

        public override void OnInitialized()
        {
            base.OnInitialized();
            GenerateWalkNavData();
        }
        private void GenerateWalkNavData()
        {
            this.Verbose("Generating Nav Data..");
            foreach (var tile in WorldSystem.World)
            {
                tile.NavData = new Dictionary<NavigationMode, ITileNavData>();
                tile.NavData.Add(
                    NavigationMode.Walking, 
                    new TileNav_Walkable(tile.Coordinates));
            }
            foreach (var tile in WorldSystem.World)
            {
                ITileNavData data;
                if (tile.NavData.TryGetValue(NavigationMode.Walking, out data))
                {
                    List<INavEdge> edges = new List<INavEdge>();
                    foreach (var neighbour in TileManager.AdjacentTiles(tile.Coordinates))
                    {
                        if (neighbour != null)
                        {
                            INavEdge edge = new TileEdge_Walkable(data.Cost, neighbour);
                            edges.Add(edge);
                        }
                    }
                    data.Edges = edges.ToArray();
                }
                else
                {
                    this.Warning($"Nav Data at {tile.Coordinates} not found");

                }
            }
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
        public static Node Node(INavNode navNode) => Node(navNode.X, navNode.Y);
        public static Node Node(WorldPoint Point) => Node(Point.X, Point.Y);
        public static Node Node(int X, int Y) 
        {
            ITileData TileData = WorldSystem.Tile(X, Y);
            if (TileData != null)
            {
                ITileNavData NavData = TileData.NavData[NavigationMode.Walking];
                if (NavData != null)
                {
                    if (NavData.Traversible)
                    {
                        Node newNode = new Node(X, Y, NavData.Cost);
                        List<PathEdge> Edges = new List<PathEdge>();
                        foreach (var _edge in NavData.Edges)
                        {
                            Edges.Add(new PathEdge(_edge.PathingCost, _edge.Destination));
                        }
                        newNode.SetEdges(Edges.ToArray());
                        return newNode;
                    }                   
                }
            }
            return null;
        }

        public static Node[] Adjacent(WorldPoint Point) => Adjacent(Point.X, Point.Y);
        public static Node[] Adjacent(int X, int Y)
        {
            List<Node> adjacentNodes = new List<Node>();
            foreach (var pos in TileManager.AdjacentCoordinates(X, Y))
            {
                Node node = Node(pos);
                if (node != null)
                {
                    adjacentNodes.Add(node);
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
                if (drawNavMeshGizmo)
                {
                    if (nodeGizmoData == null)
                    {
                        nodeGizmoData = new Dictionary<Vector3, Vector3?[]>();
                        for (int x = 0; x < WorldSystem.World.Size.x; x++)
                        {
                            for (int y = 0; y < WorldSystem.World.Size.y; y++)
                            {
                                foreach (var tile in WorldSystem.World)
                                {
                                    ITileNavData data;
                                    if (tile.NavData.TryGetValue(NavigationMode.Walking, out data))
                                    {
                                        Vector2 nodePos = new Vector2 { x = tile.X + 0.5f, y = tile.Y + 0.5f };
                                        Vector3?[] edgePositions = new Vector3?[8];
                                        for (int i = 0; i < tile.Edges.Length; i++)
                                        {
                                            if (tile.Edges[i] != null)
                                            {
                                                Vector2 edgeDir = AdjacentTileData.ToCoordinate[i];
                                                edgePositions[i] = new Vector3(nodePos.x + edgeDir.x, nodePos.y + edgeDir.y);
                                            }
                                        }
                                        nodeGizmoData.Add(nodePos, edgePositions);
                                    }
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

        #endregion
#endif
    }
}
