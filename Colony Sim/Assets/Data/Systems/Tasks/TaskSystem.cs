using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ColonySim.LoggingUtility;
using ILogger = ColonySim.LoggingUtility.ILogger;

namespace ColonySim.Systems.Tasks
{
    public class TaskSystem : System, ILogger
    {
        #region Static
        private static TaskSystem instance;
        public static TaskSystem Get => instance;
        public List<string> Logs { get; set; } = new List<string>();
        public LoggingLevel LoggingLevel { get => _loggingLevel; set => _loggingLevel = value; }
        [SerializeField]
        private LoggingLevel _loggingLevel = LoggingLevel.Warning;
        public LoggingPriority LoggingPriority { get => _loggingPriority; set => _loggingPriority = value; }
        [SerializeField]
        private LoggingPriority _loggingPriority = LoggingPriority.AlwaysShow;
        public bool Stamp { get => _stamp; set => _stamp = value; }
        public string LoggingPrefix => "<color=red>[WORK]</color>";
        [SerializeField]
        private bool _stamp = false;

        #endregion

        public Queue<IWorkOrder> TaskQueue = new Queue<IWorkOrder>();
        private List<IWorker> workers = new List<IWorker>();

        public override void Init()
        {
            this.Notice("> Task System Init.. <");
            instance = this;
            base.Init();
        }

        public static void Delegate(IWorkOrder Order)
        {
            instance.TaskDelegated(Order);
        }

        private void TaskDelegated(IWorkOrder Order)
        {
            this.Verbose("Delegating Work..");
            foreach (var worker in workers)
            {
                if (worker.Available)
                {
                    AssignTaskTo(Order, worker, TaskAssignmentMethod.DEFAULT);
                    return;
                }
            }
            TaskQueue.Enqueue(Order);
        }

        public static void Assign(IWorkOrder Task, IWorker Worker, TaskAssignmentMethod AssignmentMode = TaskAssignmentMethod.DEFAULT)
        {
            instance.AssignTaskTo(Task, Worker, AssignmentMode);
        }

        private void AssignTaskTo(IWorkOrder Task, IWorker Worker, TaskAssignmentMethod AssignmentMode)
        {
            instance.Verbose("Assigning Task to Worker..");
            Worker.AssignTask(Task, AssignmentMode);
        }

        public static void Worker(IWorker Worker)
        {
            instance.NewWorker(Worker);
        }

        private void NewWorker(IWorker Worker)
        {
            this.Verbose("Available Worker Added!");
            workers.Add(Worker);
        }
    }
}
