using System.Collections;
using System.Collections.Generic;
using ColonySim.Entities;

namespace ColonySim.World
{
    public interface ITileContainer : IEntityTriggerSystem, IEntityModuleSearch, IEntityTaskSystem
    {
        IEnumerable<IEntity> TileEntities();
        void AddEntity(IEntity Entity);
        void RemoveEntity(IEntity Entity);
        EntityID GetEntityID(IEntity Entity);
        IEntity GetEntity(EntityID EntityID);
        IEntity GetEntity(string EntityDefName);

        bool HasEntity(EntityID EntityID);
    }
    /// <summary>
    /// Game Tile
    /// </summary>
    public class TileContainer : ITileContainer
    {
        List<IEntity> Entities;
        private int _entityIDCounter;
        private readonly ITileData TileData;

        public TileContainer(ITileData TileData)
        {
            this.TileData = TileData;
        }

        public void AddEntity(IEntity Entity)
        {
            if (Entities == null) { Entities = new List<IEntity>() { Entity }; }
            else { Entities.Add(Entity); }
            Entity.ID = new EntityID(_entityIDCounter++);

            Entity.Trigger(new EntityTrigger_OnTileEnter(TileData, this));
        }

        public void RemoveEntity(IEntity Entity)
        {
            Entities.Remove(Entity);
            Entity.Trigger(new EntityTrigger_OnTileExit(TileData, this));
        }

        #region Events

        public void Trigger(IEntityTrigger Event)
        {
            foreach (var entity in Entities)
            {
                entity.Trigger(Event);
            }
        }

        public void AssignTask(EntityTask Task)
        {
            foreach (var entity in TileEntities())
            {
                entity.AssignTask(Task);
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

        #endregion

        #region Helpers

        public EntityID GetEntityID(IEntity Entity)
        {
            foreach (var entity in TileEntities())
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
            foreach (var entity in TileEntities())
            {
                if (entity.ID == ID)
                {
                    return entity;
                }
            }
            return null;
        }

        public IEntity GetEntity(string EntityDefName)
        {
            foreach (var entity in Entities)
            {
                if (entity.DefName == EntityDefName)
                {
                    return entity;
                }
            }
            return null;
        }

        public bool HasEntity(EntityID ID) => GetEntity(ID) != null;

        public IEnumerable<IEntity> TileEntities()
        {
            if (Entities != null)
            {
                foreach (var entity in Entities)
                {
                    yield return entity;
                }
            }
        }

        public override int GetHashCode()
        {
            return Entities.GetHashCode() ^ Entities.Count;
        }

        #endregion
    }
}