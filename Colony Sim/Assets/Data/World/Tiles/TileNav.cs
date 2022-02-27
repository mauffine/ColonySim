using ColonySim.Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColonySim.World
{

    public interface ITileNavData
    {
        int Cost { get; }
        bool Traversible { get; }

        void NavEntityAdded(IEntityNavData NavData);
        void NavEntityRemoved(IEntityNavData NavData);
    }

    public interface ITileNavData_Walk : ITileNavData
    {

    }

    public class TileNav_Walkable : ITileNavData_Walk
    {
        public int Cost { get; private set; }
        public bool Traversible => intraversible == 0;

        private int intraversible = 0;

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
}
