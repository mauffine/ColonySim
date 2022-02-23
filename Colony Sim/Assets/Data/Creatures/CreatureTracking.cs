using ColonySim.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColonySim.Creatures
{
    public interface ICreatureTracking
    {
        WorldPoint Coordinates { get; }
    }

    public abstract class BasicCreatureTracking : ICreatureTracking
    {
        public WorldPoint Coordinates { get; }
    }
}
