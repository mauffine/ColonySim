using ColonySim.World;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColonySim.Entities
{
    public interface IEntityTrigger
    {
        ITriggerCondition[] Conditions { get; }
    }

    public interface ITileTrigger : IEntityTrigger
    {
        ITileContainer Data { get; }
    }

    public class EntityTrigger_OnTileEnter : ITileTrigger
    {
        public ITriggerCondition[] Conditions { get; }

        public ITileContainer Data => data;
        private readonly ITileContainer data;

        public EntityTrigger_OnTileEnter(ITileContainer ContainerData)
        { data = ContainerData; }
    }

    public class EntityTrigger_DetermineName : IEntityTrigger
    {
        public ITriggerCondition[] Conditions { get; }
        public string ReturnName;
    }

    public interface ITriggerCondition
    {
    }
}
