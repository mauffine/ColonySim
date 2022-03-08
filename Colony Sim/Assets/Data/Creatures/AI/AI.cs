using ColonySim.Systems.Tasks;
using System.Collections;
using System.Collections.Generic;
using ColonySim.LoggingUtility;
using ILogger = ColonySim.LoggingUtility.ILogger;
using ILoggerSlave = ColonySim.LoggingUtility.ILoggerSlave;

namespace ColonySim.Creatures.AI
{
    //public enum TransitionType
    //{
    //    Wait = 0,
    //    Interrupt = 1,
    //    FinishTask = 2,
    //}
    //public struct Transition
    //{
    //    public AIStates FromState { get; private set;}
    //    public TransitionType TransitionType { get; private set; }

    //    public Transition(AIStates from, TransitionType transition)
    //    {
    //        FromState = from;
    //        TransitionType = transition;
    //    }
    //}
    public interface ICreatureAI : IWorker
    {
        ICreature Creature { get; }
        void Tick();
    }
    public interface IAIState
    {
        ICreatureAI AI { get; set; }
        void Tick();
    }
    public abstract class AIWorkState : IAIState, IWorker, ILoggerSlave
    {
        public ICreatureAI AI { get; set; }
        public IWorkOrder CurrentOrder { get; protected set; }
        public ICreatureNavigation Navigation => AI.Navigation;
        public LinkedList<IWorkOrder> TaskQueue { get; protected set; } = new LinkedList<IWorkOrder>();
        public bool Available => TaskQueue.Count < 3;

        public ILogger Master => (ILogger)AI;

        public string LoggingPrefix => "{AI WORK STATE}";

        public void AssignTask(IWorkOrder Task, TaskAssignmentMethod AssignmentMode = TaskAssignmentMethod.DEFAULT)
        {
            switch (AssignmentMode)
            {
                case TaskAssignmentMethod.DEFAULT:
                    Task.Assign(this);
                    TaskQueue.AddLast(Task);
                    break;
                case TaskAssignmentMethod.ENQUEUE:
                    Task.Assign(this);
                    TaskQueue.AddLast(Task);
                    break;
                case TaskAssignmentMethod.INTERRUPT:
                    Task.Assign(this);
                    if (CurrentOrder != null)
                    {
                        TaskQueue.AddFirst(CurrentOrder);
                        CurrentOrder.Interrupt();
                    }
                    TaskQueue.AddFirst(Task);
                    break;
                case TaskAssignmentMethod.CLEAR:
                    this.Debug("Clearing Work Queue..");
                    Task.Assign(this);
                    if (CurrentOrder != null)
                        CurrentOrder.Stop();
                    TaskQueue.Clear();
                    TaskQueue.AddFirst(Task);
                    break;
                default:
                    break;
            }
        }
        public void Tick()
        {
            if (CurrentOrder == null)
            {
                if (TaskQueue.Count > 0)
                {
                    CurrentOrder = TaskQueue.First.Value;
                    TaskQueue.RemoveFirst();
                    CurrentOrder.Execute();
                }
                return;
            }
            else
                CurrentOrder.Tick();

            if ((CurrentOrder.Status & ITaskStatus.FINISHED) != 0)
            {
                CurrentOrder = null;
            }
            else if ((CurrentOrder.Status & ITaskStatus.PAUSED) != 0)
            {
                CurrentOrder = null;
            }
        }
        public void Start()
        {

        }
        public void End()
        {
            TaskQueue.Clear();
            CurrentOrder.Stop();
        }
    }
    public class TestAIState : AIWorkState
    {
        public TestAIState(ICreatureAI ai)
        {
            AI = ai;
        }
    }

    public abstract class BaseAI : ICreatureAI, ILogger
    {
        public List<string> Logs { get; set; } = new List<string>();
        public LoggingLevel LoggingLevel { get; set; } = LoggingLevel.AlwaysShow;
        public LoggingPriority LoggingPriority { get; set; } = LoggingPriority.AlwaysShow;
        public bool Stamp { get => _stamp; set => _stamp = value; }
        public string LoggingPrefix => "<color=fuchsia>[AI]</color>";
        private bool _stamp = false;
        public ICreature Creature { get; protected set; }
        public IAIState CurrentState { get; protected set; }

        public IWorkOrder CurrentOrder => ((IWorker)CurrentState).CurrentOrder;

        public ICreatureNavigation Navigation => Creature.Navigation;

        public LinkedList<IWorkOrder> TaskQueue => ((IWorker)CurrentState).TaskQueue;

        public bool Available => ((IWorker)CurrentState).Available;

        ////set out in <To, From> format
        //public Transition AvailTransitions { get; private set; }
        //public Dictionary<Transition, AIStates> Transitions { get; private set; }

        //public virtual AIStates GoNext(TransitionType transitionType)
        //{
        //    Transition transition = new Transition(CurrentState, transitionType);
        //    AIStates nextState;
        //    if (!Transitions.TryGetValue(transition, out nextState))
        //    {
        //        this.Verbose($"Invalid AI Transition");
        //    }
        //    return nextState;
        //}
        public void Tick()
        {
            CurrentState.Tick();
        }

        public void AssignTask(IWorkOrder Task, TaskAssignmentMethod AssignmentMode = TaskAssignmentMethod.DEFAULT)
        {
            ((IWorker)CurrentState).AssignTask(Task, AssignmentMode);
        }
    }
    public class TestAI : BaseAI
    {
        public TestAI(ICreature creature)
        {
            Creature = creature;
            CurrentState = new TestAIState(this);
        }
    }
}
