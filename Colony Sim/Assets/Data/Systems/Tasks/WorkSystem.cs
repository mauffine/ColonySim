using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ColonySim.LoggingUtility;
using ILogger = ColonySim.LoggingUtility.ILogger;

namespace ColonySim.Systems.Tasks
{
    public class WorkSystem : System, ILogger
    {
        #region Static
        private static WorkSystem instance;
        public static WorkSystem Get => instance;
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

        public Queue<IWorkOrder> WorkOrderQueue = new Queue<IWorkOrder>();
        private List<IWorker> workers = new List<IWorker>();

        public override void Init()
        {
            this.Notice("<color=blue>[Task System Init]</color>");
            instance = this;
            base.Init();
        }

        public static void Delegate(IWorkOrder Order)
        {
            instance.WorkDelegated(Order);
        }

        private void WorkDelegated(IWorkOrder Order)
        {
            this.Verbose("Delegating Work..");
            foreach (var worker in workers)
            {
                if (worker.Available)
                {
                    AssignWorkTo(Order, worker);
                    return;
                }
            }
            WorkOrderQueue.Enqueue(Order);
        }

        public static void Assign(IWorkOrder Task, IWorker Worker)
        {
            instance.AssignWorkTo(Task, Worker);
        }

        private void AssignWorkTo(IWorkOrder Task, IWorker Worker)
        {
            instance.Verbose("Assigning Task to Worker..");
            Worker.AssignTask(Task);
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
