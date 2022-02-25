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

        private InputControl placementCtxt;
        private InputControl removalCtxt;

        private ITileData waypointTile;
        private CharacterWaypoint waypoint;

        public override void Init()
        {
            this.Notice("<color=blue>[Construction System Init]</color>");
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
                PlaceWaypoint();
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
                this.Debug("No Tile Data");
            }
        }

        private void PlaceWaypoint()
        {
            ITileData Data = CursorSystem.Get.highlightedTile;
            if (Data != null)
            {
                if (waypoint == null) { waypoint = EntitySystem.Get.CreateWaypoint(Data); }
                else { EntitySystem.Get.PlaceWaypoint(waypoint, waypointTile, Data); }
                waypointTile = Data;
                this.Verbose("Placed Waypoint");
            }
        }

        public void OnDrawGizmos()
        {
            if (Initialized)
            {
                if (waypoint != null)
                {
                    WorldPoint Coordinates = waypointTile.Coordinates;
                    Gizmos.color = new Color(1f, 0.5f, 0.5f, 0.7f);
                    Gizmos.DrawSphere(
                        new Vector3(Coordinates.X+0.5f, Coordinates.Y+0.5f, 1f),
                        0.3f
                    );
                }
            }          
        }
    }

    public class CharacterWaypoint : EntityBase
    {
        public override string DefName => "Character Waypoint";

        public override IEntityTrait[] Traits { get; }

        public CharacterWaypoint()
        {
            
        }
    }
}
