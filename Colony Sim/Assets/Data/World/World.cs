using System.Collections;
using System.Collections.Generic;

namespace ColonySim.Systems
{
    public class World
    {
        #region Static
        private static World instance;
        public static World Get()
        {
            return instance;
        }
        #endregion

        private const int CHUNK_SIZE = 5;

        private List<IChunk> WorldChunks;

        public (int X, int Y) WorldBounds { get { return worldBounds; } }
        private readonly (int X, int Y) worldBounds;
    }
}
