using ColonySim.Helpers;
using ColonySim.Systems;
using ColonySim.World.Tiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColonySim
{
    public interface IWorldRegion
    {
        public WorldPoint Origin { get; }
        public ChunkLocation Location { get; }
        public ITileData[] Tiles { get; }
        public WorldPoint[] Boundaries { get; }

        public void SetRegionTiles(ITileData[] Tiles, WorldPoint[] Boundaries);
    }
}

namespace ColonySim.World
{
    public class WorldRegion : IWorldRegion
    {
        public WorldPoint Origin { get; private set; }
        public ChunkLocation Location { get; private set; }
        public ITileData[] Tiles { get; private set; }
        public WorldPoint[] Boundaries { get; private set; }

        public WorldRegion(WorldPoint Origin, ChunkLocation Location)
        {
            this.Origin = Origin; this.Location = Location;
        }

        public void SetRegionTiles(ITileData[] Tiles, WorldPoint[] Boundaries)
        {
            this.Tiles = Tiles; this.Boundaries = Boundaries;
        }
    }
}
