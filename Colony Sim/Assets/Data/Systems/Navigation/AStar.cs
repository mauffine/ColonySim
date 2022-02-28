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

        private readonly SimplePriorityQueue<Node> OpenList = new SimplePriorityQueue<Node>();
        private readonly List<Node> ClosedList = new List<Node>();
        private readonly WorldPoint Start;
        private readonly WorldPoint End;
        private Node Current;

        public AStar(WorldPoint Start, WorldPoint End)
        {
            this.Start = Start;
            this.End = End;
        }

        public Stack<Node> Path()
        {
            this.Verbose("Generating Heuristic Path...");
            Node startNode = NavigationSystem.Node(Start);
            Node endNode = NavigationSystem.Node(End);

            if (startNode == null || endNode == null)
            {
                this.Warning($"Invalid Nodes:: {Start} - {End}");
                return null;
            }

            OpenList.Enqueue(startNode, 0);

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

                foreach (var navEdge in Current.Edges)
                {
                    if (navEdge == null) continue;
                    PathEdge edge = (PathEdge)navEdge;
                    Node neighbour = edge.Node;
                    if (ClosedList.Contains(neighbour)) continue;
                    if (OpenList.Contains(neighbour)) continue;

                    neighbour.OriginNode = Current;
                    // Distance is always 1 to an adjacent tile - No diagonal adjustment.
                    neighbour.Weight = Current.Weight + edge.PathingCost + Distance((Current.X, Current.Y), (neighbour.X, neighbour.Y));
                    neighbour.HeuristicScore = neighbour.Weight + HeuristicWeight((neighbour.X, neighbour.Y), (endNode.X, endNode.Y));

                    OpenList.Enqueue(neighbour, neighbour.HeuristicScore);

                }
            }  

            if (targetReached)
            {
                this.Verbose($"Path Located ({nodesTraversed}) at cost::{Current.HeuristicScore}");
                Stack<Node> Path = new Stack<Node>();
                do
                {
                    Path.Push(Current);
                    Current = Current.OriginNode;
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
