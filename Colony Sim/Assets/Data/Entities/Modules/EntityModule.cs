using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ColonySim.World;

namespace ColonySim.Entities
{
    public interface IEntityModule
    {

    }

    public class Module_MessageOnTileEntry : IEntityModule, IEntityTriggerSystem
    {
        public void Trigger(IEntityTrigger Event)
        {
            if (Event is ITileTrigger tileEvent)
            {
                Debug.Log("Event Triggered!");
            }
        }
    }
}
