using System.Collections;
using System.Collections.Generic;

namespace ColonySim.Creatures
{
    public interface ICreature
    {
        ICreatureTracking Tracking { get; }
    }

    public abstract class CreatureBase : ICreature
    {
        public abstract ICreatureTracking Tracking { get; }
    }

    public class BasicCreature : CreatureBase
    {
        public override ICreatureTracking Tracking => _tracking;
        private BasicCreatureTracking _tracking;
    }
}
