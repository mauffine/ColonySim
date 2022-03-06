using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ColonySim.LoggingUtility;
using ILogger = ColonySim.LoggingUtility.ILogger;
using TMPro;
using ColonySim.Systems;

namespace ColonySim
{
    public class InterfaceController : ColonySim.Systems.System, ILogger
    {
        #region Static
        private static InterfaceController instance;
        public static InterfaceController Get => instance;
        public List<string> Logs { get; set; } = new List<string>();
        public LoggingLevel LoggingLevel { get => _loggingLevel; set => _loggingLevel = value; }
        [SerializeField]
        private LoggingLevel _loggingLevel = LoggingLevel.Warning;
        public LoggingPriority LoggingPriority { get => _loggingPriority; set => _loggingPriority = value; }
        [SerializeField]
        private LoggingPriority _loggingPriority = LoggingPriority.AlwaysShow;
        public bool Stamp { get => _stamp; set => _stamp = value; }
        public string LoggingPrefix => "<color=blue>[INTERFACE]</color>";
        [SerializeField]
        private bool _stamp = false;
        #endregion

        [SerializeField]
        private TextMeshProUGUI BUILD_MODE_TEXT;
        private bool buildModeToggle = false;

        public override void Init()
        {
            this.Notice("<color=blue>[Interface Controller Init]</color>");
            instance = this;
            base.Init();
        }

        public void TOGGLE_CONSTRUCTION()
        {
            buildModeToggle = !buildModeToggle;
            BUILD_MODE_TEXT.gameObject.SetActive(buildModeToggle);
            InputSystem.ConstructionMapInput(buildModeToggle);
        }
    }
}
