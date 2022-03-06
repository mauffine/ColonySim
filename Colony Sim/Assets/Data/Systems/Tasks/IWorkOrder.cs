using ColonySim.Creatures;
using ColonySim.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ColonySim.LoggingUtility;
using ILoggerSlave = ColonySim.LoggingUtility.ILoggerSlave;

namespace ColonySim.Systems.Tasks
{
    public enum IWorkState
    {
        Nil = 0,
        Stopped = 1,
        Pending = 2,
        Running = 3,
        Interrupted = 4,
        Completed = 5,
        Success = 6,
        Failure = 7
    }

    public enum IWorkTransitionState
    {
        Nil = 0,
        Stopping = 1,
        Interrupting = 2,
        Finishing = 3
    }

    public enum WorkAssignmentMode
    {
        DEFAULT = 0,
        ENQUEUE = 1,
        INTERRUPT = 2,
        CLEAR = 3,
    }

    public interface IWorker
    {
        IWorkOrder CurrentOrder { get; }
        ICreatureNavigation Navigation { get; }
        LinkedList<IWorkOrder> TaskQueue { get; }
        bool Available { get; }

        void AssignTask(IWorkOrder Task, WorkAssignmentMode AssignmentMode = WorkAssignmentMode.DEFAULT);
    }

    public interface IWorkOrder
    {
        IWorker Worker { get; }
        IWorkState State { get; }
        IWorkTransitionState TransitionState { get; }
        ITask CurrentTask { get; }
        ITask[] TaskList { get; }

        void Assign(IWorker Worker);
        void SetState(IWorkState taskState);
        void Tick();

        void Start();
        void Stop();
        void Run();
        void Complete();
        void Interrupt();
    }

    public interface IWorkDestination : IWorkOrder
    {
        WorldPoint Destination { get; }
    }

    public abstract class BaseTask : IWorkOrder, ILoggerSlave
    {
        public LoggingUtility.ILogger Master => WorkSystem.Get;
        public string LoggingPrefix => $"<color=red>[ORDER]</color>";

        public IWorkState State { get; protected set; }
        public IWorkTransitionState TransitionState { get; protected set; }
        public IWorker Worker { get; protected set; }
        public ITask CurrentTask { get; protected set; }
        public ITask[] TaskList { get; protected set; }

        protected int TaskIndex = 0;
        protected bool Running => State == IWorkState.Running;

        public void Assign(IWorker Worker)
        {
            this.Worker = Worker;
        }

        public virtual void SetState(IWorkState taskState)
        {
            this.State = taskState;
            if (CurrentTask != null)
            {
                CurrentTask.SetState(taskState);
            }
        }

        public virtual void Start() { this.Verbose("Starting Order.."); Run(); }
        public virtual void Stop() { SetState(IWorkState.Stopped); }
        public virtual void Run() { SetState(IWorkState.Running); }
        public virtual void Complete() { SetState(IWorkState.Completed); }
        public virtual void Interrupt() { SetState(IWorkState.Interrupted); }

        public void Tick()
        {
            if (Running)
            {
                if (CurrentTask != null)
                {
                    CurrentTask.Tick();
                }
                if (CurrentTask == null || CurrentTask.State == IWorkState.Success)
                {
                    // Is there another task?
                    if (Next())
                    {
                        // If so, run.
                        CurrentTask.Execute();
                    }
                    else
                    {
                        // Otherwise we're done.
                        Complete();
                    }
                }
            }
        }

        protected bool Next()
        {
            this.Verbose("Finding Next Task..");
            if (TaskList != null && TaskIndex < TaskList.Length)
            {
                if (CurrentTask != null)
                {
                    CurrentTask.Finish();
                }
                CurrentTask = TaskList[TaskIndex];
                CurrentTask.Assign(this);
                TaskIndex++;
                return true;
            }
            return false;
        }
    }

    public class MoveToWaypointTask : BaseTask, IWorkDestination
    {
        public WorldPoint Destination { get; private set; }

        public MoveToWaypointTask(WorldPoint Destination)
        {
            this.Destination = Destination;
            TaskList = new ITask[]
            {
                new MoveTo(Destination)
            };
        }

        public override void Interrupt()
        {
            base.Interrupt();
        }
    }
}
