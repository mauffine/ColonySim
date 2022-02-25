using ColonySim.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColonySim.Creatures
{
    public interface ICreatureNavigation
    {
        WorldPoint Coordinates { get; }
    }

    public class CreatureBaseNavigation : ICreatureNavigation
    {
        public WorldPoint Coordinates { get; }
    }
}
