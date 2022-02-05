using System.Collections;
using System.Collections.Generic;

namespace ColonySim
{
    public interface ITile
    {
        (int X, int Y) Coordinates { get; }
    }

    public class Tile
    {
        public Tile((int X, int Y) Coordinates)
            : this(Coordinates.X, Coordinates.Y) { }

        public Tile(int X, int Y)
        {
            coordinates = (X, Y);
        }
        public (int X, int Y) Coordinates { get { return coordinates; } }

        private readonly (int X, int Y) coordinates;
    }
}
