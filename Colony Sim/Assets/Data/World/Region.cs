using ColonySim.Helpers;
using ColonySim.Systems;
using ColonySim.World.Tiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColonySim.World
{
    public class WorldRegion
    {
        public readonly WorldPoint Origin;
        public readonly ChunkLocation Location;
        public ITileData[] Tiles;
        public WorldPoint[] Boundaries;

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
