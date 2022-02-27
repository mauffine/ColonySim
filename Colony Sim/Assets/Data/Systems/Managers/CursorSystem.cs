using ColonySim.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static ColonySim.InputControlMap;
using ColonySim.LoggingUtility;
using ILogger = ColonySim.LoggingUtility.ILogger;

namespace ColonySim.Systems
{
    public class CursorSystem : System, IMouseActions, ILogger
    {
        #region Static
        private static CursorSystem instance;
        public static CursorSystem Get => instance;
        public List<string> Logs { get; set; } = new List<string>();
        public LoggingLevel LoggingLevel { get => _loggingLevel; set => _loggingLevel = value; }
        [SerializeField]
        private LoggingLevel _loggingLevel = LoggingLevel.Warning;
        public LoggingPriority LoggingPriority { get => _loggingPriority; set => _loggingPriority = value; }
        [SerializeField]
        private LoggingPriority _loggingPriority = LoggingPriority.AlwaysShow;
        public bool Stamp { get => _stamp; set => _stamp = value; }
        public string LoggingPrefix => "<color=purple>[CURSOR]</color>";
        [SerializeField]
        private bool _stamp = false;
        #endregion

        public override void Init()
        {
            this.Notice("<color=blue>[Cursor System Init]</color>");
            instance = this;
            base.Init();
        }

        public WorldPoint currentMousePosition;
        public WorldPoint oldMousePosition;
        public ITileData highlightedTile;

        public override void Tick()
        {
            UpdateMousePositions();
            if (highlightedTile == null)
            {
                return;
            }
        }

        private void UpdateMousePositions()
        {
            oldMousePosition = currentMousePosition;
            Vector3 mousePos = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            Vector3 mouseVector = ray.GetPoint(-ray.origin.z / ray.direction.z);
            WorldPoint worldPoint = WorldSystem.Get.VectorToWorldPoint(mouseVector);
            if (worldPoint != currentMousePosition)
            {
                currentMousePosition = worldPoint;
                highlightedTile = WorldSystem.Tile(worldPoint);
            }
            
        }

        public void OnMousePointer(InputAction.CallbackContext context)
        {
            
        }
    }
}
