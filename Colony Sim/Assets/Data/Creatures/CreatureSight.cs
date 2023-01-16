using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ColonySim.LoggingUtility;
using ILoggerSlave = ColonySim.LoggingUtility.ILoggerSlave;
using ColonySim.Systems;
using System;

namespace ColonySim
{
    public interface IVision
    {
        WorldPoint[] VisibleTiles { get; }
        WorldPoint Coordinates { get; }

        Vector2 Facing { get; }
        int ViewDistance { get; }
        float FOV { get; }
        bool Invalidated { get; }

        void UpdateVision(WorldPoint[] VisibleTiles);
        void OnVisibilityUpdate(Action<IVision> callback);
        void RemoveVisibilityUpdate(Action<IVision> callback);
    }

    public interface IPlayerVisionEntity : IVision
    {

    }
}

namespace ColonySim.Creatures
{
    public class CreatureSight : IPlayerVisionEntity, ILoggerSlave
    {
        public LoggingUtility.ILogger Master => VisionSystem.Get;
        public string LoggingPrefix => $"<color=yellow>[CREATURESIGHT]</color>";

        public WorldPoint[] VisibleTiles { get; private set; }
        public WorldPoint Coordinates => Creature.Navigation.Coordinates;
        public ICreature Creature;

        public int ViewDistance { get; private set; }
        public Vector2 Facing => Creature.Navigation.Facing;
        public float FOV => 135.0f;
        public bool Invalidated { get; set; }

        private Action<IVision> cbOnVisibilityUpdate;

        public CreatureSight(ICreature Creature, int ViewDistance)
        {
            this.Creature = Creature; this.ViewDistance = ViewDistance;
            if (Creature.Navigation != null)
            {
                Creature.Navigation.OnTileMovement(TileMovement);
            }
            Invalidated = true;
            VisionSystem.RequestVisionUpdate(this);
        }

        public void TileMovement(ITileData From, ITileData To)
        {
            if (From != To)
            {
                this.Verbose("Requesting Vision Update After Tile Movement..");
                Invalidated = true;
                VisionSystem.RequestVisionUpdate(this);
            }
        }

        public void UpdateVision(WorldPoint[] VisibleTiles)
        {
            this.Verbose($"CreatureVision Updated [{VisibleTiles.Length} Tiles]");
            this.VisibleTiles = VisibleTiles;
            Invalidated = false;
            cbOnVisibilityUpdate?.Invoke(this);
        }

        public void OnVisibilityUpdate(Action<IVision> callback)
        {
            cbOnVisibilityUpdate += callback;
        }

        public void RemoveVisibilityUpdate(Action<IVision> callback)
        {
            cbOnVisibilityUpdate -= callback;
        }
    }
}
