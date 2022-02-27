using ColonySim.Entities.Material;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColonySim.Entities
{
    public interface IEntityTrait : IEntityTriggerSystem, IEntityModuleSearch, IEntityTaskSystem
    {
        string TRAIT_DEF_NAME { get; }
        IEntityModule[] TraitModules { get; }
    }

    public abstract class EntityBaseTrait : IEntityTrait
    {
        public abstract string TRAIT_DEF_NAME { get; }
        public abstract IEntityModule[] TraitModules { get; }
        public void Trigger(IEntityTrigger Event)
        {
            if (TraitModules != null)
            {
                foreach (var module in TraitModules)
                {
                    if (module is IEntityTriggerSystem _trigger)
                    {
                        _trigger.Trigger(Event);
                    }
                }
            }           
        }

        public ModuleType FindModule<ModuleType>() where ModuleType : IEntityModule, new()
        {
            if (TraitModules != null)
            {
                foreach (var module in TraitModules)
                {
                    if (module is ModuleType _match)
                    {
                        return _match;
                    }
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
            if (TraitModules != null)
            {
                foreach (var module in TraitModules)
                {
                    if (module is IEntityTaskWorker ModuleWorker)
                    {
                        Task.Execute(ModuleWorker);
                    }
                }
            }           
        }
    }

    public class Trait_HasMaterial : EntityBaseTrait
    {
        public override string TRAIT_DEF_NAME => "HAS_MATERIAL";
        public override IEntityModule[] TraitModules { get; }

        public IEntityMaterialDef MaterialDef;

        public Trait_HasMaterial(IEntityMaterialDef MaterialDef)
        {
            this.MaterialDef = MaterialDef; 
        }
    }

    public class Trait_IsTile : EntityBaseTrait, ITaskWorker_GetTileName, IWalkData
    {
        public override string TRAIT_DEF_NAME => "TILE";
        public override IEntityModule[] TraitModules { get; }
        public int Cost { get; private set; }
        public bool Walkable { get; private set; }

        protected string Name;

        public Trait_IsTile(string Name, bool Navigable = true, int cost = 1)
        {
            this.Name = Name;
            Walkable = Navigable;
            Cost = cost;
        }

        public string GetTileName()
        {
            return Name;
        }
    }
    public interface INavData
    {
        int Cost { get; }
    }
    public interface IWalkData : INavData
    {
        bool Walkable { get; }
    }
}
