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
            this.Notice("> Navigation System Init.. <");
            instance = this;
            GenerateWalkNavData();
            base.Init();
        }

        public override void OnInitialized()
        {
            base.OnInitialized();
        }
        private void GenerateWalkNavData()
        {
            this.Verbose("Generating Nav Data..");
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
                            ITileNavData neighbourNavData = neighbour.NavData[NavigationMode.Walking];
                            if (neighbourNavData != null)
                            {
                                if (data.Traversible && neighbourNavData.Traversible)
                                {
                                    INavEdge edge = new TileEdge_Walkable(data.Cost, neighbour);
                                    edges.Add(edge);
                                }
                                UpdateEdges(neighbourNavData, neighbour.Coordinates);
                            }
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

        public static Stack<INavNode> Path(WorldPoint Start, WorldPoint End)
        {
            instance.Notice($"Navigating From {Start} to {End}");
            var heuristic = new AStar(Start, End);
            return heuristic.Path();
        }

        public static void InvalidateNavData(ITileData TileData)
        {
            instance.Verbose($"Invalidating NavData at {TileData.Coordinates}..");
            ITileNavData navData = TileData.NavData[NavigationMode.Walking];
            if (navData != null)
            {
                List<INavEdge> edges = new List<INavEdge>();
                foreach (var neighbour in TileManager.AdjacentTiles(TileData.Coordinates))
                {
                    if (neighbour != null)
                    {
                        ITileNavData neighbourNavData = neighbour.NavData[NavigationMode.Walking];
                        if (neighbourNavData != null)
                        {
                            if (navData.Traversible && neighbourNavData.Traversible)
                            {
                                INavEdge edge = new TileEdge_Walkable(navData.Cost, neighbour);
                                edges.Add(edge);
                            }
                            UpdateEdges(neighbourNavData, neighbour.Coordinates);
                        }                        
                    }                    
                }
                navData.Edges = edges.ToArray();
            }
            instance.UPDATE_GIZMOS();
        }

        private static void UpdateEdges(ITileNavData navData, WorldPoint Coordinates)
        {
            instance.Debug($"Updating Edges for {Coordinates}..");
            List<INavEdge> edges = new List<INavEdge>();
            if (navData.Traversible)
            {
                foreach (var neighbour in TileManager.AdjacentTiles(Coordinates))
                {
                    if (neighbour != null)
                    {
                        ITileNavData neighbourData = neighbour.NavData[NavigationMode.Walking];
                        if (neighbourData != null && neighbourData.Traversible)
                        {
                            INavEdge edge = new TileEdge_Walkable(navData.Cost, neighbour);
                            edges.Add(edge);
                        }
                    }
                }
            }            
            navData.Edges = edges.ToArray();
        }

#if DEBUG

        #region Gizmos

        [Conditional("DEBUG")]
        private void UPDATE_GIZMOS()
        {
            this.Debug($"GIZMO REFRESH");
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
                        foreach (var tile in WorldSystem.World)
                        {
                            if (tile != null)
                            {
                                if (tile.NavData.ContainsKey(NavigationMode.Walking))
                                {
                                    Vector2 nodePos = new Vector2 { x = tile.X + 0.5f, y = tile.Y + 0.5f };
                                    List<Vector3?> edges = new List<Vector3?>();
                                    foreach (var edge in tile.Edges)
                                    {
                                        Vector2 edgeDir = new Vector2(edge.Destination.X+0.5f, edge.Destination.Y+0.5f);
                                        edges.Add(edgeDir);
                                    }
                                    if (edges.Count > 0)
                                    {
                                        nodeGizmoData.Add(nodePos, edges.ToArray());
                                    }
                                    else
                                    {
                                        nodeGizmoData.Add(nodePos, null);
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
