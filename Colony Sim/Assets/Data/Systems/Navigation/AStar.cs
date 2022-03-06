using ColonySim.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ColonySim.LoggingUtility;
using ILoggerSlave = ColonySim.LoggingUtility.ILoggerSlave;
using Priority_Queue;

namespace ColonySim.Systems.Navigation
{
    public class AStar : ILoggerSlave
    {
        public LoggingUtility.ILogger Master => NavigationSystem.Get;
        public string LoggingPrefix => $"<color=yellow>[ASTAR]</color>";

        private readonly SimplePriorityQueue<INavNode> OpenList = new SimplePriorityQueue<INavNode>();
        private readonly List<INavNode> ClosedList = new List<INavNode>();
        private readonly Dictionary<INavNode, INavNode> Trace = new Dictionary<INavNode, INavNode>();

        private readonly Dictionary<INavNode, float> Weights = new Dictionary<INavNode, float>();
        private readonly Dictionary<INavNode, float> HeuristicScores = new Dictionary<INavNode, float>();

        private readonly WorldPoint Start;
        private readonly WorldPoint End;
        private INavNode Current;

        public AStar(WorldPoint Start, WorldPoint End)
        {
            this.Start = Start;
            this.End = End;
        }

        public Stack<INavNode> Path()
        {
            this.Verbose("Generating Heuristic Path...");
            INavNode startNode = WorldSystem.Tile(Start);
            INavNode endNode = WorldSystem.Tile(End);

            if (startNode == null || endNode == null)
            {
                this.Warning($"Invalid Nodes:: {Start} - {End}");
                return null;
            }

            OpenList.Enqueue(startNode, 0);
            Weights[startNode] = 0;
            HeuristicScores[startNode] = HeuristicWeight((Start.X, Start.Y), (End.X, End.Y));

            bool targetReached = false;
            int nodesTraversed = 0;
            while (OpenList.Count > 0)
            {
                nodesTraversed++;
                Current = OpenList.Dequeue();
                // We did it!
                if (Current == endNode)
                {
                    this.Debug("Located Path Target");
                    targetReached = true;
                    break;
                }

                ClosedList.Add(Current);

                foreach (var edge in Current.Edges)
                {
                    if (edge == null) continue;
                    INavNode neighbour = edge.Destination;
                    if (ClosedList.Contains(neighbour)) continue;

                    float weight = Weights[Current] + edge.PathingCost + Distance((Current.X, Current.Y), (neighbour.X, neighbour.Y));
                    bool alreadyOpen = OpenList.Contains(neighbour);
                    if (alreadyOpen && weight >= Weights[neighbour]) continue;

                    if (Trace.ContainsKey(neighbour) == false) Trace.Add(neighbour, Current);
                    else Trace[neighbour] = Current;
                    if (Weights.ContainsKey(neighbour) == false) Weights.Add(neighbour, weight);
                    else Weights[neighbour] = weight;

                    float heuristicWeight = weight + HeuristicWeight((neighbour.X, neighbour.Y), (endNode.X, endNode.Y));
                    if (HeuristicScores.ContainsKey(neighbour) == false) HeuristicScores.Add(neighbour, heuristicWeight);
                    else HeuristicScores[neighbour] = heuristicWeight;

                    if (!alreadyOpen)
                    {
                        OpenList.Enqueue(neighbour, HeuristicScores[neighbour]);
                    }
                }
            }  

            if (targetReached)
            {
                this.Verbose($"Path Located ({nodesTraversed}) after {nodesTraversed} nodes.");
                Stack<INavNode> Path = new Stack<INavNode>();
                do
                {
                    Path.Push(Current);
                    Current = Trace[Current];
                } while (Current != null && Current != startNode);
                return Path;
            }
            this.Verbose("No Path Found");
            return null;
        }

        private float HeuristicWeight((int X, int Y) Start, (int X, int Y) End)
        {
            return Mathf.Sqrt(
                Mathf.Pow(Start.X - End.X, 2) +
                Mathf.Pow(Start.Y - End.Y, 2));
        }

        private float Distance((int X, int Y) Start, (int X, int Y) End)
        {
            float xDif = Mathf.Abs(Start.X - End.X);
            float yDif = Mathf.Abs(Start.Y - End.Y);
            if (xDif + yDif == 1) return 1f;
            else if (xDif == 1 && yDif == 1) return 1.414213562373f;
            else
            {
                return HeuristicWeight(Start, End);
            }
        }


    }
}
