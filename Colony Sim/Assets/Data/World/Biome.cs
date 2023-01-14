using ColonySim.Entities;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ColonySim
{
    public interface IBiome
    {
        void BiomeGeneration(IWorldChunk Chunk, int seed);
    }
}

namespace ColonySim.World
{
    public abstract class BiomeDef : IBiome
    {
        public abstract void BiomeGeneration(IWorldChunk Chunk, int seed);
    }

    public class TemperateBiome : BiomeDef
    {
        public override void BiomeGeneration(IWorldChunk Chunk, int seed)
        {
            Random rand = new Random(seed);
            foreach (var tile in Chunk.GetTiles())
            {
                WorldPoint Point = tile.Coordinates;
                IEntity _entity;
                _entity = EntitySystem.Get.CreateEntity("entity.basicfloor");
                if (rand.Next(1,100) < 10)
                {
                    IEntity _shrub = EntitySystem.Get.CreateEntity("entity.shrub");
                    tile.Container.AddEntity(_shrub);
                }
                tile.Container.AddEntity(_entity);
            }
        }
    }
}
