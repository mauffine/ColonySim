using ColonySim.Entities.Material;
using ColonySim.Systems.Navigation;
using ColonySim.World;
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

        #region Delegations
        public virtual void Trigger(IEntityTrigger Event)
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

        public virtual ModuleType FindModule<ModuleType>() where ModuleType : IEntityModule, new()
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

        #endregion
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

    public class Trait_Ground : EntityBaseTrait, ITaskWorker_GetTileName, IWalkNavData
    {
        public override string TRAIT_DEF_NAME => "TILE";
        public override IEntityModule[] TraitModules { get; }
        public int Cost { get; private set; }
        public bool Walkable { get; private set; }

        protected string Name;

        public Trait_Ground(string Name, bool Navigable = true, int cost = 1)
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

    public class Trait_Impassable : EntityBaseTrait, IWalkNavData
    {
        public override string TRAIT_DEF_NAME => "IMPASSABLE";
        public override IEntityModule[] TraitModules { get; }
        public int Cost { get; private set; } = 0;
        public bool Walkable { get; private set; } = false;

        public override void Trigger(IEntityTrigger Event)
        {
            if(Event is EntityTrigger_OnTileEnter EntryEvent)
            {
                ITileNavData navData = EntryEvent.Data.NavData[NavigationMode.Walking];
                navData.NavEntityAdded(this);

            }
            else if (Event is EntityTrigger_OnTileExit ExitEvent)
            {
                ITileNavData navData = ExitEvent.Data.NavData[NavigationMode.Walking];
                navData.NavEntityRemoved(this);
            }
        }
    }

    public interface IEntityNavData
    {
        int Cost { get; }
    }
    public interface IWalkNavData : IEntityNavData
    {
        bool Walkable { get; }
    }
}
