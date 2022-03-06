using ColonySim.Systems.Navigation;
using ColonySim.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ColonySim.LoggingUtility;
using ILoggerSlave = ColonySim.LoggingUtility.ILoggerSlave;
using System;

namespace ColonySim.Creatures
{
    public interface ICreatureNavigation : IWorldTick
    {
        ICreature Creature { get; }
        WorldPoint Coordinates { get; }
        Vector2 Position { get; }
        Quaternion Facing { get; }

        void SetTilePosition(WorldPoint tileCoordinates);
        void Destination(WorldPoint destination);
        void Stop(Action cbStopEvent);
    }

    public class CreatureBaseNavigation : ICreatureNavigation, ILoggerSlave
    {
        public LoggingUtility.ILogger Master => NavigationSystem.Get;
        public string LoggingPrefix => $"<color=yellow>[CREATURENAV]</color>";

        public ICreature Creature { get; }
        public WorldPoint Coordinates { get; protected set; }
        public Vector2 Position { get
            {
                if (currentDestination != null)
                {
                    float X = Mathf.Lerp(Coordinates.X, currentDestination.Value.x, movementPercentage);
                    float Y = Mathf.Lerp(Coordinates.Y, currentDestination.Value.y, movementPercentage);
                    return new Vector2(X, Y);
                }
                else
                {
                    return new Vector2(Coordinates.X, Coordinates.Y);
                }
            } }

        public Quaternion Facing => currentFacing;
        private Quaternion currentFacing = Quaternion.identity;

        private Stack<INavNode> currentPath;
        private Vector2? currentDestination;
        private Action cbOnMovementEnd;

        public CreatureBaseNavigation(ICreature Creature)
        {
            this.Creature = Creature;
        }

        public void SetTilePosition(WorldPoint tileCoordinates)
        {
            this.Coordinates = tileCoordinates;
            UpdateTilePosition();
        }

        private void UpdateTilePosition()
        {
            ITileData TileData = WorldSystem.TileUnsf(Coordinates);
            TileData.Creature = Creature;
        }

        public void Destination(WorldPoint destination)
        {
            this.Verbose($"Navigating Creature to {destination}..");
            currentPath = NavigationSystem.Path(Coordinates, destination);
            INavNode nextNode = currentPath.Pop();
            currentDestination = new Vector2(nextNode.X, nextNode.Y);
            startFacing = currentFacing;
        }

        public void Stop(Action cbStopEvent)
        {
            this.cbOnMovementEnd = cbStopEvent;
            if (currentPath != null)
            {
                currentPath.Clear();
            }
            else
            {
                cbStopEvent();
            }
            
        }

        public void WorldTick(float delta)
        {
            Move(delta);
        }

        private float movementPercentage;
        Quaternion startFacing;

        private void Move(float delta)
        {
            if (currentDestination != null)
            {
                Vector2 destination = (Vector2)currentDestination;
                if (movementPercentage > 0)
                {
                    Quaternion targetFacing = Quaternion.FromToRotation(Position, destination - Position);
                    currentFacing = Quaternion.Lerp(startFacing, targetFacing, movementPercentage);
                }
                
                float distance = Mathf.Sqrt(
                    Mathf.Pow(Coordinates.X-destination.x, 2)+
                    Mathf.Pow(Coordinates.Y - destination.y, 2)
                    );

                float speed = 3.5f * delta;
                float travel = speed / distance;

                movementPercentage += travel;
                if (movementPercentage >= 1)
                {
                    Coordinates = WorldSystem.ToWorldPoint(destination);

                    var nextPoint = NextDestination();
                    if (nextPoint != null)
                    {
                        currentDestination = new Vector2(nextPoint.Value.X, nextPoint.Value.Y);
                    }
                    else 
                    {
                        FinishMovement();
                    }

                    movementPercentage = 0;
                    startFacing = currentFacing;
                    UpdateTilePosition();
                }
            }
        }

        private void FinishMovement()
        {
            cbOnMovementEnd?.Invoke();
            cbOnMovementEnd = null;
            currentDestination = null;
            currentPath = null;
        }

        private WorldPoint? NextDestination()
        {
            if (currentPath != null)
            {
                if (currentPath.Count == 0)
                {
                    currentPath = null;
                    return null;
                }
                INavNode nextNode = currentPath.Pop();
                return new WorldPoint(nextNode.X, nextNode.Y);
            }
            return null;
        }
    }
}
