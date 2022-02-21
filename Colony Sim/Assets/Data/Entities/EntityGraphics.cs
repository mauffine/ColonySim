using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ColonySim.Rendering;
using ColonySim.World;
using ColonySim.World.Tiles;
using System.Linq;

namespace ColonySim.Entities
{
    public interface IEntityGraphics
    {
        string MaterialID { get; }
        float DrawPriority { get; }
        RenderLayer Layer { get; }

        EntityTextureSettings GetTexture(ITileData TileData);
        void AddTextureRules(ITextureRule[] textureAdjacentSelectionRules);
    }

    public class EntityTextureSettings
    {
        public string TextureID;
        public float Angle;
        public bool[] ReadFromNeighbours; // MUST BE SORTED

        public override string ToString()
        {
            return $"TextureSettings::ID[{TextureID}] Angle[{Angle}]";
        }
    }

    public class EntityGraphics : IEntityGraphics
    {
        public string TextureID { get; protected set; }
        public string MaterialID { get; protected set; } = "tilables";
        public float DrawPriority { get; protected set; } = 0f;
        public RenderLayer Layer { get; protected set; }

        public bool Instanced = true;
        private ITextureRule[] TextureRules;

        public EntityGraphics(string TextureID, string MaterialID, RenderLayer Layer = RenderLayer.BASE)
        { this.TextureID = TextureID; this.MaterialID = MaterialID; this.Layer = Layer; }

        public EntityTextureSettings GetTexture(ITileData TileData)
        {
            if (TextureRules != null)
            {
                foreach (var textureRule in TextureRules)
                {
                    if (textureRule.Match(TileData, out EntityTextureSettings Settings))
                    {
                        return Settings;
                    }
                }
            }
            else
            {
                UnityEngine.Debug.Log("NO RULES; DEFAULT");
            }

            return new EntityTextureSettings()
            {
                TextureID = TextureID,
                Angle = 0,
                ReadFromNeighbours = null
            };
        }

        public void AddTextureRules(ITextureRule[] Rules)
        {
            UnityEngine.Debug.Log("Adding Texture Rule...");
            TextureRules = Rules;
        }
    }

    public interface ITextureRule
    {
        bool Match(ITileData Data, out EntityTextureSettings TextureSettings);
    }

    public enum NeighbourRule { DontCare = 0, Exists = 1, Not = -1 }

    public class TextureAdjacentSelectionRule : ITextureRule
    {
        public IEntity Entity;
        public string TextureID { get; protected set; }
        public enum TransformRule { Fixed, Rotated, MirrorX, MirrorY }
        public TransformRule Transform;
        //[0] [1] [2] 
        //[3] [-] [4]
        //[5] [6] [7]
        public NeighbourRule[] NeighbourRules;

        public enum MatchRule { ExactType }
        public MatchRule TypeMatch;

        private AdjacentTileData adjacentTileData;

        public TextureAdjacentSelectionRule(IEntity Entity, string TextureID, TransformRule Transform, int[] Rules)
        {
            this.Entity = Entity;
            this.TextureID = TextureID;
            this.Transform = Transform;
            NeighbourRules = new NeighbourRule[8];
            for (int i = 0; i < 8; i++)
            {
                NeighbourRules[i] = (NeighbourRule)Rules[i];
            }
        }

        public bool Match(ITileData Data, out EntityTextureSettings Settings)
        {
            UnityEngine.Debug.Log("Checking Tile Adjacency Rule..");
            if(adjacentTileData == null) adjacentTileData = TileManager.Get.GetAdjacentTiles(Data);
            Settings = new EntityTextureSettings()
            {
                TextureID = this.TextureID,
                Angle = 0,
                ReadFromNeighbours = Enumerable.Repeat(true, 7).ToArray()
            };

            bool allRulesPass = true;
            switch (Transform)
            {
                case TransformRule.Fixed:
                    UnityEngine.Debug.Log("Checking for Fixed Adjacency...");
                    for (int i = 0; i < 8; i++)
                    {
                        switch (NeighbourRules[i])
                        {
                            case NeighbourRule.Exists:
                                allRulesPass = allRulesPass && MatchingNeighbour(adjacentTileData.AdjacentTiles[i]);
                                break;
                            case NeighbourRule.Not:
                                allRulesPass = allRulesPass && !MatchingNeighbour(adjacentTileData.AdjacentTiles[i]);
                                break;
                            default:
                                break;
                        }
                    }
                    UnityEngine.Debug.Log($"Fixed Adjacency Pass::{allRulesPass}");
                    return allRulesPass;                
                case TransformRule.Rotated:
                    UnityEngine.Debug.Log("Checking For Rotated Adjacency..");
                    for (int angle = 0; angle <= 270; angle += 90)
                    {
                        UnityEngine.Debug.Log($"Checking angle {angle}..");
                        bool angleRulesPass = true;
                        for (int i = 0; i < 8; i++)
                        {                           
                            int rotatedIndex = angle == 0 ? i : GetRotatedIndex(i, angle);

                            switch (NeighbourRules[i])
                            {
                                case NeighbourRule.Exists:
                                    angleRulesPass = angleRulesPass && MatchingNeighbour(adjacentTileData.AdjacentTiles[rotatedIndex]);
                                    break;
                                case NeighbourRule.Not:
                                    angleRulesPass = angleRulesPass && !MatchingNeighbour(adjacentTileData.AdjacentTiles[rotatedIndex]);
                                    break;
                                default:
                                    break;
                            }
                            if (!angleRulesPass) break;
                        }
                        if (angleRulesPass)
                        {
                            UnityEngine.Debug.Log($"All Rules Pass at angle {angle}");
                            Settings = new EntityTextureSettings()
                            {
                                TextureID = this.TextureID,
                                Angle = angle
                            };
                            return true;
                        }
                    }
                    UnityEngine.Debug.Log("No Rotated Adjacency Rule Matches");
                    return false;
                default:
                    return false;
            }          
        }

        private bool MatchingNeighbour(ITileData Neighbour)
        {
            if (Neighbour == null) return false;
            IEntity Match = Neighbour.Container.GetEntity(Entity.DefName);
            UnityEngine.Debug.Log($"{Entity.DefName} Exists at {Neighbour.Coordinates}:{Match != null}");
            return Match != null;
        }

        private static readonly int[,] RotatedOrMirroredIndexes =
{
            {5, 3, 0, 6, 1, 7, 4, 2}, // 90
            {7, 6, 5, 4, 3, 2, 1, 0}, // 180, XY
            {2, 4, 7, 1, 6, 0, 3, 5}, // 270
            {2, 1, 0, 4, 3, 7, 6, 5}, // X
            {5, 6, 7, 3, 4, 0, 1, 2}, // Y
        };

        private int GetRotatedIndex(int original, int rotation)
        {
            switch (rotation)
            {
                case 0:
                    return original;
                case 90:
                    return RotatedOrMirroredIndexes[0, original];
                case 180:
                    return RotatedOrMirroredIndexes[1, original];
                case 270:
                    return RotatedOrMirroredIndexes[2, original];
                default:
                    break;
            }
            return original;
        }

    }
}
