using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ColonySim.LoggingUtility;
using ILogger = ColonySim.LoggingUtility.ILogger;
using ColonySim.World;
using ColonySim.Systems;

namespace ColonySim.World
{
    public class TileManager : ColonySim.Systems.System
    {
        #region Static
        private static TileManager instance;
        public static TileManager Get => instance;
        public List<string> Logs { get; set; } = new List<string>();
        public LoggingLevel LoggingLevel { get => _loggingLevel; set => _loggingLevel = value; }
        [SerializeField]
        private LoggingLevel _loggingLevel = LoggingLevel.Warning;
        public bool Stamp { get => _stamp; set => _stamp = value; }
        [SerializeField]
        private bool _stamp = false;
        #endregion

        public struct AdjacentTileData
        {
            public ITileData Origin;
            //[0] [1] [2]
            //[3] [O] [4]
            //[5] [6] [7]
            public ITileData[] AdjacencyData;

            public ITileData North => AdjacencyData[1];
            public ITileData South => AdjacencyData[6];
            public ITileData East => AdjacencyData[4];
            public ITileData West => AdjacencyData[3];
            public ITileData NEast => AdjacencyData[2];
            public ITileData NWest => AdjacencyData[0];
            public ITileData SEast => AdjacencyData[7];
            public ITileData SWest => AdjacencyData[5];

            public AdjacentTileData(ITileData Origin, ITileData[] AdjacencyData)
            {
                this.Origin = Origin; this.AdjacencyData = AdjacencyData;
            }
        }

        public AdjacentTileData GetAdjacentTiles(ITileData OriginData)
        {
            WorldPoint Origin = (WorldPoint)OriginData.Coordinates;
            int _X = Origin.X;
            int _Y = Origin.Y;
            ITileData[] AdjacencyData = new ITileData[9];
            int i = 0;
            for (int y = _Y+1; y > _Y-1; y--)
            {
                for (int x = _X-1; x < _X+1; x++)
                {
                    AdjacencyData[i] = GetTileData((x, y));
                    i++;
                }
            }
            AdjacentTileData AdjacenctTiles = new AdjacentTileData(OriginData, AdjacencyData);
            return AdjacenctTiles;
        }

        public ITileData GetTileData((int X, int Y) Coordinates)
        {
            return WorldSystem.Get.GetTileData(new WorldPoint(Coordinates.X,Coordinates.Y));
        }
    }
}
