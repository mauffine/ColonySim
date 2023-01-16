using System.Collections;
using System.Collections.Generic;
using ColonySim.Creatures;
using ColonySim.Entities;
using ColonySim.Entities.Material;
using ColonySim.Systems.Navigation;
using ColonySim.World.Tiles;

namespace ColonySim
{
    public interface INavNode : ICoordinate
    {
        INavEdge[] Edges { get; }
    }

    public interface INavEdge
    {
        int PathingCost { get; }
        INavNode Destination { get; }
    }

    /// <summary>
    /// Game Tile
    /// </summary>
    public interface ITileData : INavNode
    {
        LocalPoint Coordinates { get; }
        ITileContainer Container { get; }
        ICreature Creature { get; set; }

        Dictionary<NavigationMode, ITileNavData> NavData { get; set; }
        ITileVisibilityData VisibilityData { get; set; }
    }
}

namespace ColonySim.World.Tiles {

    public class TileData : ITileData
    {
        public ITileContainer Container { get; }
        public LocalPoint Coordinates { get { return coordinates; } }
        private readonly LocalPoint coordinates;
        public int X => coordinates.WorldX;
        public int Y => coordinates.WorldY;
        public ICreature Creature { get; set; }
        //TODO: Edges by mode
        public INavEdge[] Edges => NavData[NavigationMode.Walking].Edges;

        public Dictionary<NavigationMode, ITileNavData> NavData { get; set; }
        public ITileVisibilityData VisibilityData { get; set; }

        public TileData((int X, int Y) Chunk, int X, int Y)
        { coordinates = new LocalPoint(Chunk, X, Y); Container = new TileContainer(this); }
    }
}
