using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColonySim.Entities
{
    public interface IEntityTrait : IEntityTriggerSystem, IEntityModuleSearch, IEntityTaskSystem
    {
        IEntityModule[] TraitModules { get; }
    }

    public abstract class EntityBaseTrait : IEntityTrait
    {
        public abstract IEntityModule[] TraitModules { get; }
        public void Trigger(IEntityTrigger Event)
        {
            foreach (var module in TraitModules)
            {
                if (module is IEntityTriggerSystem _trigger)
                {
                    _trigger.Trigger(Event);
                }               
            }
        }

        public ModuleType FindModule<ModuleType>() where ModuleType : IEntityModule, new()
        {
            foreach (var module in TraitModules)
            {
                if (module is ModuleType _match)
                {
                    return _match;
                }
            }
            return default;
        }

        public virtual void AssignTask(EntityTask Task)
        {
            if (this is IEntityTaskWorker Worker)
            {
                Task.Execute(Worker);
            }
            foreach (var module in TraitModules)
            {
                if (module is IEntityTaskWorker ModuleWorker)
                {
                    Task.Execute(ModuleWorker);
                }
            }
        }
    }

    public class Trait_IsTile : EntityBaseTrait, ITaskWorker_GetTileName
    {
        public override IEntityModule[] TraitModules { get; }
        protected string Name;

        public Trait_IsTile(string Name, string TextureName)
        {
            this.Name = Name;   
            TraitModules = new IEntityModule[]
            {
                new Module_EntitySprite().SetTexture(TextureName)
            };
        }

        public string GetTileName()
        {
            return Name;
        }
    }

    public class Trait_MessageOnTileEntry : EntityBaseTrait
    {
        public override IEntityModule[] TraitModules { get; }

        public Trait_MessageOnTileEntry()
        {
            TraitModules = new IEntityModule[]
            {
                new Module_MessageOnTileEntry()
            };
        }
    }
}
