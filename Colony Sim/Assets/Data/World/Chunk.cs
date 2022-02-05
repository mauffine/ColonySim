using System.Collections;
using System.Collections.Generic;

namespace ColonySim
{
    public interface IChunk
    {
        (int X, int Y) Coordinates { get; }
    }

    public class Chunk
    {
        public World World => World.Get();

        public Chunk((int X, int Y) Coordinates)
            : this(Coordinates.X, Coordinates.Y) { }

        public Chunk(int X, int Y)
        {
            coordinates = (X, Y);
        }
        public (int X, int Y) Coordinates { get { return coordinates; } }

        private readonly (int X, int Y) coordinates;
    }
}
