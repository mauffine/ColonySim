using ColonySim.Entities;
using ColonySim.World.Tiles;
using ColonySim.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColonySim.World
{

    public interface ITileNavData
    {
        int Cost { get; }
        bool Traversible { get; }
        INavEdge[] Edges { get; set; }

        void NavEntityAdded(IEntityNavData NavData);
        void NavEntityRemoved(IEntityNavData NavData);
    }

    public interface ITileNavData_Walk : ITileNavData
    {

    }
}

namespace ColonySim.Systems.Navigation
{
    public class TileNav_Walkable : ITileNavData_Walk
    {
        public int Cost { get; private set; }
        public bool Traversible => intraversible == 0;
        public INavEdge[] Edges { get; set; }

        private int intraversible = 0;
        private WorldPoint Coordinates;

        public TileNav_Walkable(WorldPoint Coordinates)
        {
            this.Coordinates = Coordinates;
        }

        public void NavEntityAdded(IEntityNavData NavData)
        {
            Debug.Log("Nav Entity Added!");
            if (NavData is IWalkNavData WalkData)
            {
                Cost += WalkData.Cost;
                if (!WalkData.Walkable) intraversible++;
            }
        }

        public void NavEntityRemoved(IEntityNavData NavData)
        {
            Debug.Log("Nav Entity Removed!");
            if (NavData is IWalkNavData WalkData)
            {
                Cost -= WalkData.Cost;
                if (!WalkData.Walkable) intraversible--;
            }
        }
    }

    public class TileEdge_Walkable : INavEdge
    {
        public int PathingCost { get; set; }

        public INavNode Destination { get; private set; }

        public TileEdge_Walkable(int PathingCost, INavNode node)
        {
            this.PathingCost = PathingCost;
            this.Destination = node;
        }
    }
}
