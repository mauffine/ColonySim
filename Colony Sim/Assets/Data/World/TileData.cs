using System.Collections;
using System.Collections.Generic;
using ColonySim.Entities;
using ColonySim.World.Tiles;

namespace ColonySim.World
{
    /// <summary>
    /// Game Tile
    /// </summary>
    public interface ITileData
    {
        LocalPoint Coordinates { get; }
        ITileContainer Container { get; }
    }
}

namespace ColonySim.World.Tiles { 

    public class TileData : ITileData
    {
        public ITileContainer Container { get; }
        public LocalPoint Coordinates { get { return coordinates; } }
        private readonly LocalPoint coordinates;

        public TileData((int X, int Y) Chunk, int X, int Y)
        { coordinates = new LocalPoint(Chunk, X, Y); Container = new TileContainer(); }
    }

    public class ConcreteWall : EntityBase
    {
        public override string DefName => "Concrete Wall";
        public override IEntityTrait[] Traits { get; }

        public ConcreteWall()
        {
            Traits = new IEntityTrait[]
            {
                new Trait_IsTile(DefName),
                new Trait_MessageOnTileEntry()
            };
            EntityGraphicsDef = new EntityGraphics(
                "entity.wall-single",
                "basic",
                RenderLayer.CONSTRUCTS);

            EntityGraphicsDef.AddTextureRules(new TextureAdjacentSelectionRule[]
            {
                new TextureAdjacentSelectionRule
                (
                   this,
                   "entity.wall-single",
                   TextureAdjacentSelectionRule.TransformRule.Fixed,
                   new int[]
                   {
                        -1, -1, -1,
                        -1,     -1,
                        -1, -1, -1
                   }
                 ),

                new TextureAdjacentSelectionRule
                (
                   this,
                   "entity.wall-straight",
                   TextureAdjacentSelectionRule.TransformRule.Fixed,
                   new int[]
                   {
                        -1, 1, -1,
                        -1,    -1,
                        -1, 1, -1
                   }
                 ),

                new TextureAdjacentSelectionRule
                (
                   this,
                   "entity.wall-end",
                   TextureAdjacentSelectionRule.TransformRule.Rotated,
                   new int[]
                   {
                        -1, -1, -1,
                        -1,     -1,
                         0,  1,  0
                   }
                 ),

                new TextureAdjacentSelectionRule
                (
                   this,
                   "entity.wall-edge",
                   TextureAdjacentSelectionRule.TransformRule.Rotated,
                   new int[]
                   {
                        -1, -1, -1,
                         1,      1,
                         1,  1,  1
                   }
                 ),

                new TextureAdjacentSelectionRule
                (
                   this,
                   "entity.wall-full",
                   TextureAdjacentSelectionRule.TransformRule.Fixed,
                   new int[]
                   {
                         1, 1, 1,
                         1,    1,
                         1, 1, 1
                   }
                 ),

                new TextureAdjacentSelectionRule
                (
                   this,
                   "entity.wall-trijunction",
                   TextureAdjacentSelectionRule.TransformRule.Rotated,
                   new int[]
                   {
                         -1,  1,  0,
                          1,     -1,
                         -1,  1,  0
                   }
                 ),

                new TextureAdjacentSelectionRule
                (
                   this,
                   "entity.wall-quadjunction",
                   TextureAdjacentSelectionRule.TransformRule.Fixed,
                   new int[]
                   {
                         -1,  1,  -1,
                          1,       1,
                         -1,  1,  -1
                   }
                 )
            });
        }
    }

    public class ConcreteFloor : EntityBase
    {
        public override string DefName => "Concrete Floor";

        public override IEntityTrait[] Traits { get; }

        public ConcreteFloor()
        {
            Traits = new IEntityTrait[]
            {
                new Trait_IsTile(DefName)
            };
            EntityGraphicsDef = new EntityGraphics(
                "entity.concretefloor",
                "simpleNoise",
                RenderLayer.TILE);
            
        }
    }
}
