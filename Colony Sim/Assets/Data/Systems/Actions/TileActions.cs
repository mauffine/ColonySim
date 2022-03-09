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

    public class Action_PlaceReplaceEntity : IReversibleAction
    {
        private readonly WorldPoint Coordinates;
        private readonly IEntity EntityToPlace;
        private readonly IEntity[] EntitiesToReplace;

        public Action_PlaceReplaceEntity(WorldPoint Coordinates, IEntity EntityToPlace, IEntity[] EntitiesToReplace)
        {
            this.Coordinates = Coordinates; this.EntityToPlace = EntityToPlace;
            this.EntitiesToReplace = EntitiesToReplace;
        }

        public bool Execute()
        {
            if (EntitySystem.Get.PlaceEntity(EntityToPlace, WorldSystem.Tile(Coordinates)))
            {
                bool success = true;
                foreach (var entity in EntitiesToReplace)
                {
                    success = success && EntitySystem.Get.RemoveEntity(entity, WorldSystem.Tile(Coordinates));
                }
                return success;
            }
            return false;
        }

        public bool UnExecute()
        {
            if (EntityToPlace != null)
            {
                if(EntitySystem.Get.RemoveEntity(EntityToPlace, WorldSystem.Tile(Coordinates)))
                {
                    bool success = true;
                    foreach (var entity in EntitiesToReplace)
                    {
                        success = success && EntitySystem.Get.PlaceEntity(entity, WorldSystem.Tile(Coordinates));
                    }
                }
            }
            return false;
        }
    }
}
