using ColonySim.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColonySim.Creatures
{
    public interface ICreatureNavigation
    {
        WorldPoint TileCoordinates { get; }
        Vector2 Position { get; }

        void SetTilePosition(WorldPoint tileCoordinates);
    }

    public class CreatureBaseNavigation : ICreatureNavigation
    {
        public WorldPoint TileCoordinates { get; protected set; }
        public Vector2 Position { get; protected set; }

        public void SetTilePosition(WorldPoint tileCoordinates)
        {
            this.TileCoordinates = tileCoordinates;
            this.Position = new Vector2(tileCoordinates.X+0.5F, tileCoordinates.Y+0.5F);
        }
    }
}
