using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColonySim
{
    public interface IWorldTick
    {
        void WorldTick(float delta);
    }

    public class WorldSimulation
    {
        List<IWorldTick> Simulated;

        public void Tick()
        {
            float delta = Time.deltaTime;
            foreach (var obj in Simulated)
            {
                obj.WorldTick(delta);
            }
        }

        public void Simulate(IWorldTick obj)
        {
            if (Simulated == null) Simulated = new List<IWorldTick>() { obj };
            else { Simulated.Add(obj); }
        }
    }
}
