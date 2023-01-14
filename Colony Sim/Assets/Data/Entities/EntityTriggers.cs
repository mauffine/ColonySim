using ColonySim.World;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColonySim
{
    public interface IEntityTrigger
    {
        ITriggerCondition[] Conditions { get; }
    }

    public interface ITileTrigger : IEntityTrigger
    {
        ITileData Data { get; }
    }

    public interface ITriggerCondition
    {
    }
}

namespace ColonySim.Entites { 

    public class EntityTrigger_OnTileEnter : ITileTrigger
    {
        public ITriggerCondition[] Conditions { get; }

        public ITileData Data => data;
        private readonly ITileData data;
        public ITileContainer Container => container;
        private readonly ITileContainer container;

        public EntityTrigger_OnTileEnter(ITileData Data, ITileContainer Container)
        { data = Data; container = Container; }
    }

    public class EntityTrigger_OnTileExit : ITileTrigger
    {
        public ITriggerCondition[] Conditions { get; }

        public ITileData Data => data;
        private readonly ITileData data;
        public ITileContainer Container => container;
        private readonly ITileContainer container;

        public EntityTrigger_OnTileExit(ITileData Data, ITileContainer Container)
        { data = Data; container = Container; }
    }
}
