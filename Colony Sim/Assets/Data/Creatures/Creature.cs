using ColonySim.Systems;
using ColonySim.Systems.Tasks;
using ColonySim.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ColonySim.LoggingUtility;
using ILoggerSlave = ColonySim.LoggingUtility.ILoggerSlave;

namespace ColonySim.Creatures
{
    public interface ICreature : IWorldTick, ICreatureRenderData
    {
        ICreatureNavigation Navigation { get; }
    }

    public abstract class CreatureBase : ICreature, ILoggerSlave
    {
        public LoggingUtility.ILogger Master => CreatureController.Get;
        public string LoggingPrefix => $"<color=red>[TESTCREATURE]</color>";

        public abstract ICreatureNavigation Navigation {get;}
        public Vector2 RenderPoint => Navigation.Position;
        public Quaternion RenderFacing => Navigation.Facing;
        public abstract string RenderTexture { get; }

        public virtual void WorldTick(float delta)
        {
            
        }

        public virtual void SetTilePosition(WorldPoint Point)
        {
            Navigation.SetTilePosition(Point);
        }
    }

    public class TestCreature : CreatureBase, IWorker
    {
        public IWorkOrder CurrentOrder { get; private set; }
        public LinkedList<IWorkOrder> TaskQueue { get; } = new LinkedList<IWorkOrder>();
        public bool Available { get; } = true;

        public override ICreatureNavigation Navigation => _navigation;
        private readonly CreatureBaseNavigation _navigation;
        public override string RenderTexture => "survivor-temp";

        public TestCreature()
        {
            _navigation = new CreatureBaseNavigation(this);
        }

        public override void WorldTick(float delta)
        {
            base.WorldTick(delta);
            if (CurrentOrder != null)
            {
                CurrentOrder.Tick();
                if ((CurrentOrder.Status & ITaskStatus.FINISHED) != 0)
                {
                    TaskQueue.RemoveFirst();
                    CurrentOrder = null;
                    LookForWork();
                }
                else if ((CurrentOrder.Status & ITaskStatus.PAUSED) != 0)
                {
                    CurrentOrder = null;
                    LookForWork();
                }
            }
            _navigation.WorldTick(delta);

        }

        public void AssignTask(IWorkOrder Task, TaskAssignmentMethod Assignment = TaskAssignmentMethod.DEFAULT)
        {
            this.Verbose("Assigning Task..");
            if (Assignment == TaskAssignmentMethod.DEFAULT || Assignment == TaskAssignmentMethod.ENQUEUE)
            {
                TaskQueue.AddLast(Task);
                LookForWork();
            }
            else if (Assignment == TaskAssignmentMethod.INTERRUPT)
            {
                if (CurrentOrder != null)
                {
                    CurrentOrder.Interrupt();                  
                }
                TaskQueue.AddFirst(Task);
            }
            else if (Assignment == TaskAssignmentMethod.CLEAR)
            {
                if (CurrentOrder != null)
                {
                    CurrentOrder.Stop();
                }
                TaskQueue.Clear();
                TaskQueue.AddFirst(Task);
            }
            
        }

        private void LookForWork()
        {
            if (CurrentOrder == null)
            {
                if (TaskQueue.Count > 0)
                {
                    BeginTask(TaskQueue.First.Value);
                }
            }
        }

        private void BeginTask(IWorkOrder Task)
        {
            CurrentOrder = Task;
            Task.Assign(this);
            Task.Execute();
        }
    }
}
