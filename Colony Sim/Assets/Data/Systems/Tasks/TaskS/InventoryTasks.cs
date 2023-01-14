using ColonySim.Creatures;
using ColonySim.Entities;
using ColonySim.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColonySim.Systems.Tasks
{
    public class PickUpItem : TaskState
    {
        private readonly IEntity Item;
        private ICreatureInventory Inventory => this.Worker.Inventory;

        public PickUpItem(IEntity Item)
        {
            this.Item = Item;
            SetStatus(ITaskStatus.Pending);
        }

        public override void Execute()
        {
            //Worker.Navigation.Destination(Destination);
            SetStatus(ITaskStatus.Running);
        }

        public override void Tick()
        {
            UpdateState();
            if (Running)
            {
                if (Inventory.AddItem(Item))
                {
                    FinishState = ITaskStatus.Success;
                    Transition(ITaskTransition.Finishing);
                    EntitySystem.InvalidateEntity(Item);
                }
            }
        }
    }

    public class MoveToPickupItem : WorkOrder
    {
        public WorldPoint Destination { get; private set; }

        public MoveToPickupItem(WorldPoint Destination, IEntity Item)
        {
            this.Destination = Destination;
            TaskList = new ITaskState[]
            {
                new MoveTo(Destination),
                new PickUpItem(Item)
            };
        }
    }
}
