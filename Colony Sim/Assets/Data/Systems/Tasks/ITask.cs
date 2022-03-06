using ColonySim.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColonySim.Systems.Tasks
{

    public interface ITask
    {
        IWorkOrder WorkOrder { get; }
        IWorkState State { get; }

        void SetState(IWorkState State);
        bool Execute();
        void Tick();
        void Finish();
        void Assign(IWorkOrder Order);
    }

    public class MoveTo : ITask
    {
        public IWorkOrder WorkOrder { get; private set; }
        public IWorkState State { get; private set; }
        private WorldPoint Destination;

        private IWorker Worker => WorkOrder.Worker;

        public MoveTo(WorldPoint Destination)
        {
            this.Destination = Destination;
            SetState(IWorkState.Pending);
        }

        public void Assign(IWorkOrder Order)
        {
            this.WorkOrder = Order;
        }

        public void SetState(IWorkState State)
        {
            this.State = State;
        }

        public bool Execute()
        {
            if (Worker.Navigation == null)
            {
                Debug.LogWarning("Navigation is null!!");
            }
            else
            {
                Worker.Navigation.Destination(Destination);
            }

            SetState(IWorkState.Running);
            return true;
        }

        public void Tick()
        {
            if (State == IWorkState.Running)
            {
                if (WorkOrder.Worker.Navigation.Coordinates == Destination)
                {
                    SetState(IWorkState.Success);
                }
            }           
        }

        public void Finish()
        {
            SetState(IWorkState.Completed);
        }
    }
}
