using System.Collections;
using System.Collections.Generic;
using ColonySim.Entities;

namespace ColonySim.World
{
    public interface ITileData
    {
        LocalPoint Coordinates { get; }
        ITileContainer Container { get; }
    }
    /// <summary>
    /// Game Tile
    /// </summary>
    public class TileData : ITileData
    {
        public ITileContainer Container { get; }

        public TileData((int X, int Y) Chunk, int X, int Y)
        { coordinates = new LocalPoint(Chunk, X, Y); Container = new TileContainer(); }
        public LocalPoint Coordinates { get { return coordinates; } }

        private readonly LocalPoint coordinates;
    }

    public class ConcreteWall : EntityBase
    {
        public override string Name => "Concrete Wall";
        public string TextureID => "ConcreteWall";
        public override IEntityTrait[] Traits { get; }

        public ConcreteWall()
        {
            Traits = new IEntityTrait[]
            {
                new Trait_TileNameDetermination(this.Name),
                new Trait_MessageOnTileEntry()
            };
        }
    }

    public class ConcreteFloor : EntityBase
    {
        public override string Name => "Concrete Floor";
        public string TextureID => "ConcreteFloor";

        public override IEntityTrait[] Traits { get; }

        public ConcreteFloor()
        {
            Traits = new IEntityTrait[]
            {
                new Trait_TileNameDetermination(this.Name)
            };
        }
    }
}
