using System.Collections;
using System.Collections.Generic;
using ColonySim.Entities;

namespace ColonySim.World
{
    public interface ITileContainer
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
            Entity.Enter(this);
        }

        public void RemoveEntity(IEntity Entity)
        {
            Entities.Remove(Entity);
            Entity.Exit(this);
        }
    }
}
