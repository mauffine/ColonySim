using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using ColonySim.LoggingUtility;
using ILogger = ColonySim.LoggingUtility.ILogger;

namespace ColonySim.Systems
{
    public class InputSystem : System, ILogger
    {
        #region Static
        private static InputSystem instance;
        public static InputSystem Get => instance;
        public List<string> Logs { get; set; } = new List<string>();
        public LoggingLevel LoggingLevel { get => _loggingLevel; set => _loggingLevel = value; }
        [SerializeField]
        private LoggingLevel _loggingLevel = LoggingLevel.Warning;
        public bool Stamp { get => _stamp; set => _stamp = value; }
        [SerializeField]
        private bool _stamp = false;
        #endregion
        [SerializeField] private InputActionAsset InputAsset;
        private InputControlMap inputActions;
        private void Awake()
        {
            instance = this;
        }
        public override void Init()
        {
            this.Verbose("<color=blue>[Input System Init]</color>");
            inputActions = new InputControlMap();
            Initialized = true;
            base.Init();
        }
        public override void OnInitialized()
        {

        }

        public InputControlMap.ConstructionActions ConstructionActions => inputActions.Construction;
        public InputControlMap.CameraActions CameraActions => inputActions.Camera;

        public static bool AllowMouseEvent
        {
            get
            {
                return true;
            }
        }
    }
}
