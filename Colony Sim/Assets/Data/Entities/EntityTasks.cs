using ColonySim.World;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColonySim.Entities
{
    public interface IEntityTask
    {
        public bool Completed { get; }
        public void Execute(IEntityTaskWorker Worker);
    }

    public interface IEntityTaskManager
    {
        bool TaskResponse(IEntityTaskWorker Worker, EntityTask Task);
    }

    public interface IEntityTaskWorker
    {
        
    }

    public abstract class EntityTask : IEntityTask
    {
        protected IEntityTaskManager Manager;

        public abstract void Execute(IEntityTaskWorker Worker);
        public bool Completed { get; protected set; }

        public EntityTask(IEntityTaskManager Manager)
        {
            this.Manager = Manager;
        }

        protected virtual bool Running => !Completed;

        protected virtual bool CompleteTask(IEntityTaskWorker Worker)
        {
            return Completed = Manager.TaskResponse(Worker, this);
        }
        
    }

    public interface ITaskWorker_GetTileName : IEntityTaskWorker
    {
        string GetTileName();
    }

    public class Task_GetTileName : EntityTask
    {
        public string Name;

        public Task_GetTileName(IEntityTaskManager Manager) : base(Manager) { }

        public override void Execute(IEntityTaskWorker Worker)
        {
            if (Running && Worker is ITaskWorker_GetTileName TileNameWorker)
            {
                Name = TileNameWorker.GetTileName();
                CompleteTask(TileNameWorker);
            }
        }

        public Task_GetTileName SetName(string Name)
        {
            this.Name = Name;
            return this;
        }
    }


}
