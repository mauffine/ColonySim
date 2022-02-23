using ColonySim.Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static ColonySim.InputControlMap;
using ColonySim.World;
using ColonySim.LoggingUtility;
using ILogger = ColonySim.LoggingUtility.ILogger;

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
        public bool Stamp { get => _stamp; set => _stamp = value; }
        public string LoggingPrefix => "<color=purple>[CONSTRUCTION]</color>";
        [SerializeField]
        private bool _stamp = false;
        #endregion

        private WorldPoint CurrentPosition => CursorSystem.Get.currentMousePosition;
        private WorldPoint OldPosition => CursorSystem.Get.oldMousePosition;

        private InputControl placementCtxt;
        private InputControl removalCtxt;

        public override void Init()
        {
            this.Verbose("<color=blue>[Construction System Init]</color>");
            instance = this;
            base.Init();
        }

        public override void OnInitialized()
        {
            InputSystem sys = InputSystem.Get;
            sys.ConstructionActions.SetCallbacks(this);
            sys.ConstructionActions.Enable();
            base.OnInitialized();
        }

        public void OnPlaceTile(InputAction.CallbackContext context)
        {
            placementCtxt = context.control;
            if (context.performed && InputSystem.AllowMouseEvent)
            {
                PlaceTile();
            }
        }

        public void OnRemoveTile(InputAction.CallbackContext context)
        {
            removalCtxt = context.control;
            if (context.performed && InputSystem.AllowMouseEvent)
            {

            }
        }

        private void PlaceTile()
        {
            this.Debug("Clicked::" + CurrentPosition);
            ITileData Data = CursorSystem.Get.highlightedTile;
            if (Data != null)
            {
                EntitySystem.Get.CreateWallEntity(Data);
                this.Verbose("Placed Entity");
            }
            else
            {
                this.Verbose("No Tile Data");
            }
        }

    }
}
