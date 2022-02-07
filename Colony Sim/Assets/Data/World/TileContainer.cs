using System.Collections;
using System.Collections.Generic;
using ColonySim.Entities;

namespace ColonySim.World
{
    public interface ITileContainer : IEntityTriggerSystem, IEntityModuleSearch
    {
        void AddEntity(IEntity Entity);
        void RemoveEntity(IEntity Entity);
    }
    /// <summary>
    /// Game Tile
    /// </summary>
    public class TileContainer : ITileContainer
    {
        List<IEntity> Entities;

        public void AddEntity(IEntity Entity)
        {
            if (Entities == null) { Entities = new List<IEntity>() { Entity }; }
            else { Entities.Add(Entity); }
            EntityTrigger_OnTileEnter _event = new EntityTrigger_OnTileEnter(this);
            Entity.Trigger(_event);
        }

        public void RemoveEntity(IEntity Entity)
        {
            Entities.Remove(Entity);
            Entity.Trigger(new EntityTrigger_OnTileEnter(this));
        }

        public void Trigger(IEntityTrigger Event)
        {
            foreach (var entity in Entities)
            {
                entity.Trigger(Event);
            }
        }

        public ModuleType FindModule<ModuleType>() where ModuleType : IEntityModule, new()
        {
            if (Entities != null)
            {
                foreach (var entity in Entities)
                {
                    return entity.FindModule<ModuleType>();
                }
            }
            return default;
        }
    }
}
