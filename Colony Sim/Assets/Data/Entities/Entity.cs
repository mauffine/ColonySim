using ColonySim.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColonySim.Entities
{
    /// <summary>
    /// Game Object
    /// </summary>
    public interface IEntity
    {
        void Enter(ITileContainer Container);
        void Exit(ITileContainer Container);
    }

    public interface ISimulatedEntity : IEntity
    {
        void Tick(float delta);
    }

    public class WallEntity : IEntity
    {
        public void Enter(ITileContainer Container)
        {

        }

        public void Exit(ITileContainer Container)
        {

        }
    }
}
