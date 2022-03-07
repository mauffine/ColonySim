using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ColonySim.World;
using ColonySim.World.Tiles;
using ColonySim.LoggingUtility;
using ILogger = ColonySim.LoggingUtility.ILogger;
using ColonySim.Rendering;
using ColonySim.Systems;
using ISystem = ColonySim.Systems.System;
using ColonySim.Systems.Navigation;

namespace ColonySim.Entities
{
    public class EntitySystem : ISystem, ILogger
    {
        #region Static
        private static EntitySystem instance;
        public static EntitySystem Get => instance;
        public List<string> Logs { get; set; } = new List<string>();
        public LoggingLevel LoggingLevel { get => _loggingLevel; set => _loggingLevel = value; }
        [SerializeField]
        private LoggingLevel _loggingLevel = LoggingLevel.Warning;
        public LoggingPriority LoggingPriority { get => _loggingPriority; set => _loggingPriority = value; }
        [SerializeField]
        private LoggingPriority _loggingPriority = LoggingPriority.AlwaysShow;
        public bool Stamp { get => _stamp; set => _stamp = value; }
        public string LoggingPrefix => "<color=green>[ENTITYSYS]</color>";
        [SerializeField]
        private bool _stamp = false;
        #endregion

        public override void Init()
        {
            this.Notice("<color=blue>[Entity System Init]</color>");
            instance = this;
            base.Init();
        }

        public void PlaceWaypoint(IEntity EntityData, ITileData TileData, ITileData NewTileData)
        {
            if (TileData.Container.HasEntity(EntityData.ID))
            {
                TileData.Container.RemoveEntity(EntityData);
                NewTileData.Container.AddEntity(EntityData);
            }
        }

        public bool PlaceEntity(IEntity EntityData, ITileData TileData)
        {
            TileData.Container.AddEntity(EntityData);
            WorldRenderer.SetTileDirty(TileData);
            NavigationSystem.InvalidateNavData(TileData);
            WorldSystem.InvalidateChunk(TileData.Coordinates);
            return true;
        }

        public bool RemoveEntity(IEntity EntityData, ITileData TileData)
        {
            TileData.Container.RemoveEntity(EntityData);
            WorldRenderer.SetTileDirty(TileData);
            NavigationSystem.InvalidateNavData(TileData);
            WorldSystem.InvalidateChunk(TileData.Coordinates);
            return true;
        }

        public IEntity CreateWallEntity()
        {
            return new ConcreteWall();
            //PlaceEntity(entity, Data);
        }
    }
}