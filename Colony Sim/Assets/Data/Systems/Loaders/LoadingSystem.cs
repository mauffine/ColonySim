using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ColonySim.LoggingUtility;
using ILogger = ColonySim.LoggingUtility.ILogger;
using ColonySim.Entities;

namespace ColonySim.Systems
{
    public class LoadingSystem : System, ILogger
    {
        #region Static
        private static LoadingSystem instance;
        public static LoadingSystem Get => instance;
        public List<string> Logs { get; set; } = new List<string>();
        public LoggingLevel LoggingLevel { get => _loggingLevel; set => _loggingLevel = value; }
        [SerializeField]
        private LoggingLevel _loggingLevel = LoggingLevel.Warning;
        public LoggingPriority LoggingPriority { get => _loggingPriority; set => _loggingPriority = value; }
        [SerializeField]
        private LoggingPriority _loggingPriority = LoggingPriority.AlwaysShow;
        public bool Stamp { get => _stamp; set => _stamp = value; }
        public string LoggingPrefix => "<color=yellow>[LOADINGSYS]</color>";
        [SerializeField]
        private bool _stamp = false;
        #endregion

        public Dictionary<string, IEntity> EntityDefs = new Dictionary<string, IEntity>();

        public override void Init()
        {
            this.Notice(" > Loading System Init.. <");
            instance = this;
            EntityLoader loader = new EntityLoader();
            EntityDefs = loader.LoadComponentsFromAssembly();
            base.Init();
        }
    }
}
