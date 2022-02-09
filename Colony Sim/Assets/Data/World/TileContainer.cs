using System.Collections;
using System.Collections.Generic;
using ColonySim.Entities;

namespace ColonySim.World
{
    public interface ITileContainer : IEntityTriggerSystem, IEntityModuleSearch, IEntityTaskSystem
    {
        IEnumerable<IEntity> AllEntities();
        void AddEntity(IEntity Entity);
        void RemoveEntity(IEntity Entity);
        EntityID GetEntityID(IEntity Entity);
        IEntity GetEntity(EntityID EntityID);
    }
    /// <summary>
    /// Game Tile
    /// </summary>
    public class TileContainer : ITileContainer
    {
        List<IEntity> Entities;

        public IEnumerable<IEntity> AllEntities()
        {
            foreach (var entity in Entities)
            {
                yield return entity;
            }
        }

        public void AddEntity(IEntity Entity)
        {
            if (Entities == null) { Entities = new List<IEntity>() { Entity }; }
            else { Entities.Add(Entity); }
            Entity.ID = new EntityID(Entities.Count);
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

        public EntityID GetEntityID(IEntity Entity)
        {
            foreach (var entity in AllEntities())
            {
                if (entity == Entity)
                {
                    return entity.ID;
                }
            }
            return default;
        }

        public IEntity GetEntity(EntityID ID)
        {
            foreach (var entity in AllEntities())
            {
                if (entity.ID == ID)
                {
                    return entity;
                }
            }
            return null;
        }

        public void AssignTask(EntityTask Task)
        {
            foreach (var entity in AllEntities())
            {
                entity.AssignTask(Task);
            }
        }
    }
}
