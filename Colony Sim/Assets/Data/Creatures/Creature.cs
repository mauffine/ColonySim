using ColonySim.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColonySim.Creatures
{
    public interface ICreature : IWorldTick
    {
        ICreatureNavigation Navigation { get; }
    }

    public abstract class CreatureBase : ICreature
    {
        public abstract ICreatureNavigation Navigation {get;}

        public virtual void WorldTick(float delta)
        {
            
        }

        public virtual void SetTilePosition(WorldPoint Point)
        {
            Navigation.SetTilePosition(Point);
        }
    }

    public class TestCreature : CreatureBase
    {
        public override ICreatureNavigation Navigation => _navigation;
        private CreatureBaseNavigation _navigation;

        public TestCreature()
        {
            _navigation = new CreatureBaseNavigation();
        }
    }
}
