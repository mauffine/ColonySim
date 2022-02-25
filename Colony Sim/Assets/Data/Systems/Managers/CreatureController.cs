using UnityEngine;
using System.Collections.Generic;

using ColonySim.LoggingUtility;
using ILogger = ColonySim.LoggingUtility.ILogger;

namespace ColonySim.Systems
{
    public class CreatureController : System, ILogger, IWorldTick
    {
        #region Static
        private static CreatureController instance;
        public static CreatureController Get => instance;
        public List<string> Logs { get; set; } = new List<string>();
        public LoggingLevel LoggingLevel { get => _loggingLevel; set => _loggingLevel = value; }
        [SerializeField]
        private LoggingLevel _loggingLevel = LoggingLevel.Warning;
        public LoggingPriority LoggingPriority { get => _loggingPriority; set => _loggingPriority = value; }
        [SerializeField]
        private LoggingPriority _loggingPriority = LoggingPriority.AlwaysShow;
        public bool Stamp { get => _stamp; set => _stamp = value; }
        public string LoggingPrefix => "<color=red>[CREATURES]</color>";
        [SerializeField]
        private bool _stamp = false;
        #endregion

        public override void Init()
        {
            this.Notice("<color=blue>[Creature Controller Init]</color>");
            instance = this;
            base.Init();
        }

        public override void OnInitialized()
        {
            base.OnInitialized();
            WorldSimulation.Get.Simulate(this);
        }

        public void WorldTick(float delta)
        {
            
        }
    }
}