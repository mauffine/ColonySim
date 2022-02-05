using System.Collections;
using System.Collections.Generic;

namespace ColonySim.World
{
    public interface ITileData
    {
        (int X, int Y) Coordinates { get; }
    }
    /// <summary>
    /// Game Tile
    /// </summary>
    public class TileData : ITileData
    {
        public TileData((int X, int Y) Coordinates)
            : this(Coordinates.X, Coordinates.Y) { }

        public TileData(int X, int Y)
        {
            coordinates = (X, Y);           
        }
        public (int X, int Y) Coordinates { get { return coordinates; } }

        private readonly (int X, int Y) coordinates;
    }
}
