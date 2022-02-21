using ColonySim.Systems;
using ColonySim.World;
using ColonySim.World.Tiles;
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

    public interface IEntityTaskSystem
    {
        void AssignTask(EntityTask Task);
    }

    public struct EntityID
    {
        public int ID;

        public EntityID(int ID)
        {
            this.ID = ID;
        }

        public static implicit operator int(EntityID EntityID) { return EntityID.ID; }
        public static explicit operator EntityID(int ID) { return new EntityID(ID); }

        public override string ToString()
        {
            return ID.ToString();
        }
    }

    /// <summary>
    /// Game Object
    /// </summary>
    public interface IEntity : IEntityTriggerSystem, IEntityModuleSearch, IEntityTaskSystem
    {
        string DefName { get; }
        EntityID ID { get; set; }
        IEntityTrait[] Traits { get; }
        IEntityGraphics EntityGraphicsDef { get; }
    }

    public abstract class EntityBase : IEntity
    {
        public abstract string DefName { get; }
        public EntityID ID { get; set; }
        public abstract IEntityTrait[] Traits { get; }
        public IEntityGraphics EntityGraphicsDef { get; protected set; }

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

        public void AssignTask(EntityTask Task)
        {
            if (Traits != null)
            {
                foreach (var trait in Traits)
                {
                    trait.AssignTask(Task);
                }
            }
        }
    }
}
