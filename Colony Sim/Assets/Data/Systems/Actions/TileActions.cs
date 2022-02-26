using ColonySim.Entities;
using ColonySim.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColonySim.Systems.Actions
{
    public class Action_PlaceEntity : IReversibleAction
    {
        private readonly WorldPoint Coordinates;
        private readonly IEntity EntityToPlace;

        public Action_PlaceEntity(WorldPoint Coordinates, IEntity EntityToPlace)
        {
            this.Coordinates = Coordinates; this.EntityToPlace = EntityToPlace;
        }

        public bool Execute()
        {
            if (EntitySystem.Get.PlaceEntity(EntityToPlace, WorldSystem.Tile(Coordinates)))
            {
                return true;
            }
            return false;
        }

        public bool UnExecute()
        {
            if (EntityToPlace != null)
            {
                return EntitySystem.Get.RemoveEntity(EntityToPlace, WorldSystem.Tile(Coordinates));
            }
            return false;
        }
    }
}
