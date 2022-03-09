using ColonySim.Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static ColonySim.InputControlMap;
using ColonySim.World;
using ColonySim.LoggingUtility;
using ILogger = ColonySim.LoggingUtility.ILogger;
using ColonySim.Systems.Actions;
using ColonySim.Systems.Tasks;

namespace ColonySim.Systems
{
    public class ConstructionSystem : System, IConstructionActions, ILogger, IEntityTaskManager
    {
        #region Static
        private static ConstructionSystem instance;
        public static ConstructionSystem Get => instance;
        public List<string> Logs { get; set; } = new List<string>();
        public LoggingLevel LoggingLevel { get => _loggingLevel; set => _loggingLevel = value; }
        [SerializeField]
        private LoggingLevel _loggingLevel = LoggingLevel.Warning;
        public LoggingPriority LoggingPriority { get => _loggingPriority; set => _loggingPriority = value; }
        [SerializeField]
        private LoggingPriority _loggingPriority = LoggingPriority.AlwaysShow;
        public bool Stamp { get => _stamp; set => _stamp = value; }
        public string LoggingPrefix => "<color=purple>[CONSTRUCTION]</color>";
        [SerializeField]
        private bool _stamp = false;
        #endregion

        public string BuildingDef = "entity.wall";

        private WorldPoint CurrentPosition => CursorSystem.Get.currentMousePosition;
        private WorldPoint OldPosition => CursorSystem.Get.oldMousePosition;

        private bool placingEntity;

        public override void Init()
        {
            this.Notice("> Construction System Init <");
            instance = this;
            base.Init();
        }

        public override void OnInitialized()
        {
            InputSystem.ConstructionActions.SetCallbacks(this);
            //InputSystem.ConstructionActions.Enable();
            base.OnInitialized();
        }

        public override void Tick()
        {
            base.Tick();
            if (InputSystem.AllowMouseEvent)
            {
                if (CurrentPosition != OldPosition)
                {
                    if (placingEntity)
                    {
                        PlaceTile();
                    }
                }
            }
        }

        public void OnPlaceTile(InputAction.CallbackContext context)
        {
            placingEntity = context.performed && context.control.IsActuated() && InputSystem.AllowMouseEvent;
            if (placingEntity)
            {
                PlaceTile();
            }
        }

        public void OnRemoveTile(InputAction.CallbackContext context)
        {
            if (context.performed && InputSystem.AllowMouseEvent)
            {
                RemoveTile();
            }
        }

        private void PlaceTile()
        {
            this.Debug("Clicked::" + CurrentPosition);
            ITileData Data = CursorSystem.Get.highlightedTile;
            if (Data != null)
            {
                this.Verbose($"Placing new Entity at {Data.Coordinates}..");

                IEntity EntityDef = EntitySystem.GetDef(BuildingDef);
                if (EntityDef != null)
                {
                    Task_GetEntityPlacementFlags defPlacementFlagsTask = new Task_GetEntityPlacementFlags(this);
                    EntityDef.AssignTask(defPlacementFlagsTask);

                    List<IEntity> toReplace = new List<IEntity>();
                    bool outcomeReplace = false;
                    bool outcome = true;

                    if (defPlacementFlagsTask.Completed)
                    {
                        this.Verbose($"[I]Placement Flags::{defPlacementFlagsTask.PlacementFlags[0].Height},{defPlacementFlagsTask.PlacementFlags[0]._onPlacement}");

                        bool replace = false;
                        // Whether the entity thinks it can be placed
                        bool canPlace = true;
                        // Whether existing entities think it can be placed.
                        bool allowPlace = true;
                        foreach (var entity in Data.Container)
                        {
                            foreach (var placementFlag in defPlacementFlagsTask.PlacementFlags)
                            {
                                var _ret = placementFlag.OnPlacement(entity);
                                canPlace = canPlace && _ret;
                                this.Verbose($"[I]CanPlace::{(canPlace ? "PASS" : "FAIL")}");
                                if(canPlace) replace = replace || (placementFlag._onPlacement & EntityHeightRule.Replace) != 0;
                            }
                            if (canPlace)
                            {
                                Task_GetEntityPlacementFlags entityPlacementFlags = new Task_GetEntityPlacementFlags(this);
                                entity.AssignTask(entityPlacementFlags);

                                if (entityPlacementFlags.Completed)
                                {
                                    foreach (var placementFlag in entityPlacementFlags.PlacementFlags)
                                    {
                                        var _ret = placementFlag.AllowPlacement(EntityDef);
                                        allowPlace = allowPlace && _ret;
                                        this.Verbose($"[O]Allow Place::{(allowPlace ? "PASS" : "FAIL")}");
                                        if (!allowPlace && replace && toReplace.Contains(entity) == false)
                                        {
                                            toReplace.Add(entity);
                                        }
                                    }
                                }
                            }   
                        }
                        if (!allowPlace)
                        {
                            outcome = outcome && canPlace && replace;
                            this.Verbose($"[CHECK-REPLACE?]::{(outcome ? "PASS" : "FAIL")}");
                            if (outcome) outcomeReplace = true;
                        }
                        else
                        {
                            outcome = outcome && canPlace && allowPlace;
                            this.Verbose($"[CHECK]::{(outcome ? "PASS" : "FAIL")}");
                        }
                        
                    }
                    else
                    {
                        bool allowPlace = true;
                        foreach (var entity in Data.Container)
                        {
                            Task_GetEntityPlacementFlags entityPlacementFlags = new Task_GetEntityPlacementFlags(this);
                            entity.AssignTask(entityPlacementFlags);

                            if (entityPlacementFlags.Completed)
                            {
                                foreach (var placementFlag in entityPlacementFlags.PlacementFlags)
                                {
                                    var _ret = placementFlag.AllowPlacement(EntityDef);
                                    allowPlace = allowPlace && _ret;
                                    this.Verbose($"[O]Placement Flags::{placementFlag}::{(allowPlace ? "PASS" : "FAIL")}");
                                }
                            }
                        }
                        outcome = outcome && allowPlace;
                    }

                    if (outcome)
                    {
                        this.Verbose($"Placing Entity..");
                        IEntity newEntity = EntitySystem.Get.CreateEntity(BuildingDef);
                        

                        if (outcomeReplace && toReplace.Count > 0)
                        {
                            this.Verbose($"Replacing Existing Entities..");
                            new Action_PlaceReplaceEntity(CurrentPosition, newEntity, toReplace.ToArray()).AutoExec();
                            
                        }
                        else
                        {
                            new Action_PlaceEntity(CurrentPosition, newEntity).AutoExec();
                        }
                    }
                    else
                    {
                        this.Debug("Entity Forbidden Placement!");
                    }

                    
                }
            }
            else
            {
                this.Debug("No Tile Data");
            }
        }

        private void RemoveTile()
        {
            this.Debug("Clicked::" + CurrentPosition);
            ITileData Data = CursorSystem.Get.highlightedTile;
            if (Data != null)
            {
                IEntity[] toRemove = Data.Container.Contents;
                foreach (var entity in toRemove)
                {
                    if (entity.EntityType != EntityType.FLOOR)
                    {
                        EntitySystem.Get.RemoveEntity(entity, Data);
                    }
                }
            }
        }

        public void OnDrawGizmos()
        {
            if (Initialized)
            {

            }          
        }

        public void OnUndo(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                ActionHandler.Undo(1);
            }
            
        }

        public void OnRedo(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                ActionHandler.Redo(1);
            }          
        }

        public void SetConcreteWall()
        {
            BuildingDef = "entity.wall";
        }

        public void SetDoor()
        {
            BuildingDef = "entity.door";
        }

        public void SetFloor()
        {
            BuildingDef = "entity.concretefloor";
        }

        public bool TaskResponse(IEntityTaskWorker Worker, EntityTask Task)
        {
            return true;
        }
    }
}
