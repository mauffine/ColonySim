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
        public bool[] ReadFromNeighbours;
        public bool MirrorX;

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
                for (int i = 0; i < TextureRules.Length; i++)
                {
                    var textureRule = TextureRules[i];
                    UnityEngine.Debug.Log($"CHECKING RULE::{i}");
                    if (textureRule.Match(TileData, out EntityTextureSettings Settings))
                    {
                        UnityEngine.Debug.Log($"CHECKING RULE::{i}::PASS");
                        return Settings;
                    }
                    else
                    {
                        UnityEngine.Debug.Log($"CHECKING RULE::{i}::FAIL");
                    }
                }
            }
            else
            {
                UnityEngine.Debug.Log("DEFAULT TEXTURE");
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
        public enum TransformRule { Fixed, Rotated, MirrorX, MirrorY, RMirrorX, RMirrorY }
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
            string _transformLog = Transform == TransformRule.Fixed ? "FIXED" : "ROTATED";
            UnityEngine.Debug.Log($"TILE RULE::{_transformLog}::{this.TextureID}");
            if (adjacentTileData == null) adjacentTileData = TileManager.Get.GetAdjacentTiles(Data);
            Settings = new EntityTextureSettings()
            {
                TextureID = this.TextureID,
                Angle = 0,
                ReadFromNeighbours = Enumerable.Repeat(true, 8).ToArray()
            };

            bool allRulesPass = true;
            switch (Transform)
            {
                case TransformRule.Fixed:
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
                    return allRulesPass;
                case TransformRule.Rotated:
                    if (RotationMatch(Data, out float Angle))
                    {
                        Settings = new EntityTextureSettings()
                        {
                            TextureID = this.TextureID,
                            Angle = Angle,
                            ReadFromNeighbours = Enumerable.Repeat(true, 8).ToArray()
                        };
                        return true;
                    }
                    return false;
                case TransformRule.RMirrorX:
                    if (RotatedMirrorMatch(Data, out float mirroredAngle))
                    {
                        Settings = new EntityTextureSettings()
                        {
                            TextureID = this.TextureID,
                            Angle = mirroredAngle,
                            ReadFromNeighbours = Enumerable.Repeat(true, 8).ToArray(),
                            MirrorX = true
                        };
                        return true;
                    }
                    return false;
                default:
                    return false;
            }
        }

        private bool MatchingNeighbour(ITileData Neighbour)
        {
            if (Neighbour == null) return false;
            IEntity Match = Neighbour.Container.GetEntity(Entity.DefName);
            string _matchLog = Match != null ? "EXISTS" : "NOT";
            UnityEngine.Debug.Log($"{Entity.DefName}::{Neighbour.Coordinates}::{_matchLog}");
            return Match != null;
        }

        private bool RotationMatch(ITileData Neighbour, out float Angle)
        {
            for (int angle = 0; angle <= 270; angle += 90)
            {
                UnityEngine.Debug.Log($"@ANGLE::{angle}");
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
                    UnityEngine.Debug.Log($"@ANGLE::{angle}::PASS");
                    Angle = angle;
                    return true;
                }
                else
                {
                    UnityEngine.Debug.Log($"@ANGLE::{angle}::FAIL");
                }
            }
            Angle = 0;
            return false;
        }

        private bool RotatedMirrorMatch(ITileData Neighbour, out float Angle)
        {
            for (int angle = 0; angle <= 270; angle += 90)
            {
                UnityEngine.Debug.Log($"@ANGLE::{angle}");
                bool angleRulesPass = true;
                for (int i = 0; i < 8; i++)
                {
                    int rotatedIndex = angle == 0 ? i : GetRotatedMirroredIndex(i, angle);

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
                    UnityEngine.Debug.Log($"@ANGLE::{angle}::PASS");
                    Angle = angle;
                    return true;
                }
                else
                {
                    UnityEngine.Debug.Log($"@ANGLE::{angle}::FAIL");
                }
            }
            Angle = 0;
            return false;
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

        private int GetRotatedMirroredIndex(int original, int rotation)
        {
            return RotatedOrMirroredIndexes[3, GetRotatedIndex(original, rotation)];

        }
    }
}
