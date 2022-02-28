using ColonySim.Systems.Navigation;
using ColonySim.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ColonySim.LoggingUtility;
using ILoggerSlave = ColonySim.LoggingUtility.ILoggerSlave;

namespace ColonySim.Creatures
{
    public interface ICreatureNavigation : IWorldTick
    {
        WorldPoint Coordinates { get; }
        Vector2 Position { get; }

        void SetTilePosition(WorldPoint tileCoordinates);
        void Destination(WorldPoint destination);
    }

    public class CreatureBaseNavigation : ICreatureNavigation, ILoggerSlave
    {
        public LoggingUtility.ILogger Master => NavigationSystem.Get;
        public string LoggingPrefix => $"<color=yellow>[CREATURENAV]</color>";

        public WorldPoint Coordinates { get; protected set; }
        public Vector2 Position { get; protected set; }

        private Stack<Node> currentPath;
        private WorldPoint _destination;

        private Vector2? currentNode;

        public void SetTilePosition(WorldPoint tileCoordinates)
        {
            this.Coordinates = tileCoordinates;
            this.Position = new Vector2(tileCoordinates.X+0.5F, tileCoordinates.Y+0.5F);
        }

        public void Destination(WorldPoint destination)
        {
            this.Verbose($"Navigating Creature to {destination}..");
            _destination = destination;
            currentPath = NavigationSystem.Path(Coordinates, destination);
        }

        public void WorldTick(float delta)
        {
            Move(delta);
        }

        //private void Move(float delta)
        //{
        //    moveDelta += delta;
        //    if (moveDelta > 0.5f)
        //    {
        //        var nextNode = currentPath.Pop();
        //        SetTilePosition(new WorldPoint(nextNode.Position));
        //        moveDelta = 0;
        //        if (currentPath.Count == 0)
        //        {
        //            currentPath = null;
        //        }
        //    }

        //}

        private void Move(float delta)
        {
            if(currentNode != null && currentNode != Position)
            {
                Position = Vector2.MoveTowards(Position, (Vector2)currentNode, delta * 8);
            }
            else if (currentPath != null)
            {
                // If we're at the next node
                var next = currentPath.Pop();
                currentNode = new Vector2(next.Position.x + 0.5f, next.Position.y + 0.5f);
                if (currentPath.Count == 0) currentPath = null;
            }
            else
            {
                currentNode = null;
            }
        }
    }
}
