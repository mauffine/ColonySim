using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ColonySim.World;
using ColonySim.Entities;
using ColonySim.LoggingUtility;
using ILogger = ColonySim.LoggingUtility.ILogger;

namespace ColonySim.Systems
{
    public class EntitySystem : System, ILogger
    {
        #region Static
        private static EntitySystem instance;
        public static EntitySystem Get => instance;
        public List<string> Logs { get; set; } = new List<string>();
        public LoggingLevel LoggingLevel { get => _loggingLevel; set => _loggingLevel = value; }
        [SerializeField]
        private LoggingLevel _loggingLevel = LoggingLevel.Warning;
        public bool Stamp { get => _stamp; set => _stamp = value; }
        [SerializeField]
        private bool _stamp = false;
        #endregion

        public override void Init()
        {
            this.Verbose("<color=blue>[Entity System Init]</color>");
            instance = this;
            base.Init();
        }

        public void CreateWallEntity(ITileData Data)
        {
            ITileContainer Container = Data.Container;
            WallEntity entity = new WallEntity();
            Container.AddEntity(entity);
        }
    }
}
