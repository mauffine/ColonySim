using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColonySim.Entities
{
    public interface IEntityTrait : IEntityTriggerSystem, IEntityModuleSearch
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
                module.Trigger(Event);
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
    }

    public class Trait_TileNameDetermination : EntityBaseTrait
    {
        public override IEntityModule[] TraitModules { get; }

        public Trait_TileNameDetermination(string Name)
        {
            TraitModules = new IEntityModule[]
            {
                new Module_TileNameDetermination().SetName(Name)
            };
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
