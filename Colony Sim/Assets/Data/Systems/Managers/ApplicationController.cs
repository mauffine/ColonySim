using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ColonySim.LoggingUtility;
using ILogger = ColonySim.LoggingUtility.ILogger;

namespace ColonySim.Systems
{
    /// <summary>
    /// Main Thread
    /// </summary>
    public class ApplicationController : MonoBehaviour, ILogger
    {
        #region Static
        private static ApplicationController instance;
        public static ApplicationController Get() => instance;

        public List<string> Logs { get; set; } = new List<string>();
        public LoggingLevel LoggingLevel { get => _loggingLevel; set => _loggingLevel = value; }
        [SerializeField]
        private LoggingLevel _loggingLevel = LoggingLevel.Warning;
        public LoggingPriority LoggingPriority { get => _loggingPriority; set => _loggingPriority = value; }
        [SerializeField]
        private LoggingPriority _loggingPriority = LoggingPriority.AlwaysShow;
        public bool Stamp { get => _stamp; set => _stamp = value; }
        public string LoggingPrefix => "<color=blue>[APPLICATION]</color>";
        [SerializeField]
        private bool _stamp = false;

        #endregion

        public static string DataPath;

        [SerializeField]
        private System[] Systems;

        private System[] InitializedSystems;
        private int InitializedCount;
        private bool Initialised;
        
        // Start is called before the first frame update
        void Awake()
        {
            instance = this;
            DataPath = Application.dataPath + "/Data/";
            if (Systems != null)
            {
                this.Notice("[Initialising Application Systems..]");
                InitializedSystems = new System[Systems.Length];
                foreach (var sys in Systems)
                {
                    this.Debug("[System Init..]");
                    sys.Init();
                }
            }           
        }

        // Update is called once per frame
        void Update()
        {
            if (Initialised)
            {
                foreach (var sys in Systems)
                {
                    sys.Tick();
                }
            }       
        }

        public void Initialized(System sys)
        {
            InitializedSystems[InitializedCount] = sys;
            InitializedCount++;
            if (InitializedCount >= InitializedSystems.Length)
            {
                this.Debug("[System Initialized..]");
                FinishInit();
            }
        }

        public void FinishInit()
        {
            Initialised = true;
            foreach (var sys in InitializedSystems)
            {
                sys.OnInitialized();
            }
            this.Notice("<color=blue>[Applicated Initialised]</color>");
        }
    }
}
