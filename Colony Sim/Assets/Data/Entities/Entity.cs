using ColonySim.World;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColonySim.Entities
{
    public interface IEntityTriggerSystem
    {
        void Trigger(IEntityTrigger Event);
    }

    public interface IEntityModuleSearch
    {
        ModuleType FindModule<ModuleType>() where ModuleType : IEntityModule, new();
    }

    /// <summary>
    /// Game Object
    /// </summary>
    public interface IEntity : IEntityTriggerSystem, IEntityModuleSearch
    {
        string Name { get; }
        IEntityTrait[] Traits { get; }
    }

    public abstract class EntityBase : IEntity
    {
        public abstract string Name { get; }
        public abstract IEntityTrait[] Traits { get; }

        public void Trigger(IEntityTrigger Event)
        {
            foreach (var trait in Traits)
            {
                trait.Trigger(Event);
            }
        }

        public ModuleType FindModule<ModuleType>() where ModuleType : IEntityModule, new()
        {
            if (Traits != null)
            {
                foreach (var trait in Traits)
                {
                    return trait.FindModule<ModuleType>();
                }
            }
            return default;
        }
    }
}
