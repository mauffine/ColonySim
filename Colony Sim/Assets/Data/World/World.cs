using System.Collections;
using System.Collections.Generic;
using ColonySim.Systems;

namespace ColonySim.World
{
    /// <summary>
    /// Game World
    /// </summary>
    public class GameWorld
    {
        #region Static
        private static GameWorld instance;
        public static GameWorld Get()
        {
            return instance;
        }
        #endregion

        private IWorldChunk[][] WorldChunks;
        public IEnumerable<IWorldChunk> GetChunks()
        {
            for (int x = 0; x < WorldChunks.Length; x++)
            {
                for (int y = 0; y < WorldChunks.Length; y++)
                {
                    yield return WorldChunks[x][y];
                }
            }
        }

        private readonly WorldSystem SYSTEM;

        public (int X, int Y) WorldBounds { get { return worldBounds; } }
        private readonly (int X, int Y) worldBounds;

        public GameWorld(int X, int Y)
        {
            SYSTEM = WorldSystem.Get();
            WorldChunks = new IWorldChunk[X][];
            for (int _x = 0; _x < X; _x++)
            {
                WorldChunks[_x] = new IWorldChunk[Y];
                for (int _y = 0; _y < Y; _y++)
                {
                    WorldChunks[_x][_y] = GenerateNewChunk(_x, _y);
                }
            }
            worldBounds = (X, Y);
        }

        private IWorldChunk GenerateNewChunk(int X, int Y)
        {
            IWorldChunk Chunk = new WorldChunk((X,Y), WorldSystem.CHUNK_SIZE);
            return Chunk;
        }
    }
}
