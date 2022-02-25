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
        public LoggingPriority LoggingPriority { get => _loggingPriority; set => _loggingPriority = value; }
        [SerializeField]
        private LoggingPriority _loggingPriority = LoggingPriority.AlwaysShow;
        public bool Stamp { get => _stamp; set => _stamp = value; }
        public string LoggingPrefix => "<color=purple>[INPUT]</color>";
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
            this.Notice("<color=blue>[Input System Init]</color>");
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
