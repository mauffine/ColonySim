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
    public class ConstructionSystem : System, IConstructionActions, ILogger
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

        private WorldPoint CurrentPosition => CursorSystem.Get.currentMousePosition;
        private WorldPoint OldPosition => CursorSystem.Get.oldMousePosition;

        private bool placingEntity;

        public override void Init()
        {
            this.Notice("<color=blue>[Construction System Init]</color>");
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
            placingEntity = context.control.IsActuated() && InputSystem.AllowMouseEvent;
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
                IEntity newEntity = EntitySystem.Get.CreateWallEntity();
                new Action_PlaceEntity(CurrentPosition, newEntity).AutoExec();
                this.Verbose("Placed Entity");
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
    }
}
