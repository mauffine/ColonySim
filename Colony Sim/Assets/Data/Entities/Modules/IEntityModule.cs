using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ColonySim.World;

namespace ColonySim.Entities
{
    public interface IEntityModule : IEntityTriggerSystem
    {

    }

    public class Module_MessageOnTileEntry : IEntityModule
    {
        public void Trigger(IEntityTrigger Event)
        {
            if (Event is ITileTrigger tileEvent)
            {
                Debug.Log("Event Triggered!");
            }
        }
    }

    public class Module_TileNameDetermination : IEntityModule
    {
        public string Name;

        public void Trigger(IEntityTrigger Event)
        {
            if (Event is Module_TileNameDetermination nameDetermination)
            {
                nameDetermination.Name = this.Name;
            }
        }

        public Module_TileNameDetermination SetName(string Name)
        {
            this.Name = Name;
            return this;
        }
    }
}
