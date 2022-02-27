using System.Collections;
using System.Collections.Generic;
using ColonySim.Entities;
using ColonySim.Entities.Material;
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
                new Trait_HasMaterial(new BasicWallMaterialDef()),
            };

            EntityGraphicsDef = new EntityGraphics(
                "entity.wall-single",
                "basic",
                DefName,
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
                         0, -1,  0,
                        -1,     -1,
                         0, -1,  0
                   }
                 ),

                new TextureAdjacentSelectionRule
                (
                   this,
                   "entity.wall-straight",
                   TextureAdjacentSelectionRule.TransformRule.Rotated,
                   new int[]
                   {
                         0, 1,  0,
                        -1,    -1,
                         0, 1,  0
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
                         0, -1,  0,
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
                         -1,  1, -1,
                          1,      1,
                          0, -1,  0
                   }
                 ),

                new TextureAdjacentSelectionRule
                (
                   this,
                   "entity.wall-trijunction-full",
                   TextureAdjacentSelectionRule.TransformRule.RMirrorX,
                   new int[]
                   {
                          1,  1, -1,
                          1,      1,
                         -1, -1, -1
                   }
                 ),

                new TextureAdjacentSelectionRule
                (
                   this,
                   "entity.wall-quadjunction",
                   TextureAdjacentSelectionRule.TransformRule.Rotated,
                   new int[]
                   {
                         -1,  1,  -1,
                          1,       1,
                         -1,  1,  -1
                   }
                 ),

                new TextureAdjacentSelectionRule
                (
                   this,
                   "entity.wall-edgejunction",
                   TextureAdjacentSelectionRule.TransformRule.Rotated,
                   new int[]
                   {
                         -1,  1,  -1,
                          1,       1,
                          1,  1,   1
                   }
                 ),

                new TextureAdjacentSelectionRule
                (
                   this,
                   "entity.wall-corner",
                   TextureAdjacentSelectionRule.TransformRule.Rotated,
                   new int[]
                   {
                          0,  -1,   0,
                          1,       -1,
                         -1,   1,   0
                   }
                 ),

                new TextureAdjacentSelectionRule
                (
                   this,
                   "entity.wall-full-corner",
                   TextureAdjacentSelectionRule.TransformRule.RMirrorX,
                   new int[]
                   {
                          0,  -1,   0,
                          1,       -1,
                          1,   1,   0
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
                DefName,
                RenderLayer.TILE);
            
        }
    }

    public class DirtFloor : EntityBase
    {
        public override string DefName => "Dirt Floor";

        public override IEntityTrait[] Traits { get; }

        public DirtFloor()
        {
            Traits = new IEntityTrait[]
            {
                new Trait_IsTile(DefName)
            };
            EntityGraphicsDef = new EntityGraphics(
                "entity.dirtfloor",
                "simpleNoise",
                DefName,
                RenderLayer.TILE);

        }
    }
}
