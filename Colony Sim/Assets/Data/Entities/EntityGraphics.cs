using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ColonySim.Rendering;
using ColonySim.World;
using ColonySim.World.Tiles;
using System.Linq;
using ColonySim.LoggingUtility;
using ILoggerSlave = ColonySim.LoggingUtility.ILoggerSlave;

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
        private readonly string DefName;

        public EntityGraphics(string TextureID, string MaterialID, string DefName, RenderLayer Layer = RenderLayer.BASE)
        { this.TextureID = TextureID; this.MaterialID = MaterialID; this.Layer = Layer; this.DefName = DefName; }

        public EntityTextureSettings GetTexture(ITileData TileData)
        {
            AdjacentTileData adjacentTileData = TileManager.AdjacencyData(TileData);
            if (TextureRules != null)
            {
                for (int i = 0; i < TextureRules.Length; i++)
                {
                    var textureRule = TextureRules[i];
                    int matchingNeighbours = 0;
                    foreach (var neighbour in adjacentTileData)
                    {
                        if (neighbour != null && neighbour.Container.GetEntity(DefName) != null)
                        {
                            matchingNeighbours++;
                        }
                    }
                    if (textureRule.Match(TileData, adjacentTileData, matchingNeighbours, out EntityTextureSettings Settings))
                    {
                        return Settings;
                    }
                }
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
        bool Match(ITileData Data, AdjacentTileData adjacencyData, int matchingNeighbours, out EntityTextureSettings TextureSettings);
    }

    public enum NeighbourRule { DontCare = 0, Exists = 1, Not = -1 }

    public class TextureAdjacentSelectionRule : ITextureRule, ILoggerSlave
    {
        public LoggingUtility.ILogger Master => WorldRenderer.Get;
        public string LoggingPrefix => $"<color=magenta>[TXTADJRULE]</color>";

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

        public bool Match(ITileData Data, AdjacentTileData adjacencyData, int matchingNeighbours, out EntityTextureSettings Settings)
        {
            string _transformLog = Transform == TransformRule.Fixed ? "FIXED" : "ROTATED";
            adjacentTileData = adjacencyData;          
            Settings = new EntityTextureSettings()
            {
                TextureID = this.TextureID,
                Angle = 0,
                ReadFromNeighbours = Enumerable.Repeat(true, 8).ToArray()
            };

            this.Debug($"{adjacentTileData.Origin}::CHECKING::{this.TextureID}::{_transformLog}::{matchingNeighbours}", LoggingPriority.Low);

            int maxNeighbourCount = 8;
            int minNeighbourCount = 0;
            for (int i = 0; i < 8; i++)
            {
                if(NeighbourRules[i] == NeighbourRule.Not)
                {
                    maxNeighbourCount--;
                }else if(NeighbourRules[i] == NeighbourRule.Exists)
                {
                    minNeighbourCount++;
                }
            }

            if (matchingNeighbours > maxNeighbourCount || matchingNeighbours < minNeighbourCount)
            {               
                this.Debug($"{this.TextureID}::NEIGHBOURCHECK::<color=red>FAIL</color>", LoggingPriority.Low);
                return false;
            }

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
                    if (allRulesPass)
                    {
                        this.Debug($"{this.TextureID}::<color=green>PASS</color>", LoggingPriority.Low);
                    }
                    else
                    {
                        this.Debug($"{this.TextureID}::<color=red>FAIL</color>", LoggingPriority.Low);
                    }
                    
                    return allRulesPass;
                case TransformRule.Rotated:
                    if (RotationMatch(Data, out float Angle))
                    {
                        this.Verbose($"{this.TextureID}::<color=green>PASS</color>", LoggingPriority.Low);
                        Settings = new EntityTextureSettings()
                        {
                            TextureID = this.TextureID,
                            Angle = Angle,
                            ReadFromNeighbours = Enumerable.Repeat(true, 8).ToArray()
                        };
                        return true;
                    }
                    this.Debug($"{this.TextureID}::<color=red>FAIL</color>", LoggingPriority.Low);
                    return false;
                case TransformRule.RMirrorX:
                    if (RotationMatch(Data, out float _Angle))
                    {
                        this.Verbose($"{this.TextureID}::<color=green>PASS</color>", LoggingPriority.Low);
                        Settings = new EntityTextureSettings()
                        {
                            TextureID = this.TextureID,
                            Angle = _Angle,
                            ReadFromNeighbours = Enumerable.Repeat(true, 8).ToArray()
                        };
                        return true;
                    }
                    if (RotatedMirrorMatch(Data, out float mirroredAngle))
                    {
                        this.Verbose($"{this.TextureID}::<color=green>MIRROR PASS</color>", LoggingPriority.Low);
                        Settings = new EntityTextureSettings()
                        {
                            TextureID = this.TextureID,
                            Angle = mirroredAngle,
                            ReadFromNeighbours = Enumerable.Repeat(true, 8).ToArray(),
                            MirrorX = true
                        };
                        return true;
                    }
                    this.Debug($"{this.TextureID}::<color=red>FAIL</color>", LoggingPriority.Low);
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
            return Match != null;
        }

        private bool RotationMatch(ITileData Neighbour, out float Angle)
        {
            // Check every 90 degree angle
            for (int angle = 0; angle <= 270; angle += 90)
            {
                this.Debug($"@{angle}::", LoggingPriority.Low);
                NeighbourRule[] RotatedRules = new NeighbourRule[8];
                for (int i = 0; i < 8; i++)
                {
                    NeighbourRule rule = NeighbourRules[GetRotatedIndex(i, angle)];
                    RotatedRules[i] = rule;
                    //this.Debug($"RotatedRules[{i}:{AdjacentTileData.IndexToString[i]}] = {rule}", LoggingPriority.Low);
                }

                bool angleRulesPass = true;
                // Check each neighbour
                for (int i = 0; i < 8; i++)
                {
                    switch (RotatedRules[i])
                    {
                        case NeighbourRule.Exists:
                            angleRulesPass = angleRulesPass && MatchingNeighbour(adjacentTileData.AdjacentTiles[i]);
                            string passLog = angleRulesPass ? "<color=green>PASS</color>" : "<color=red>FAIL</color>";
                            this.Debug($"{AdjacentTileData.IndexToString[i]} - Exists? = {passLog}", LoggingPriority.Low);
                            break;
                        case NeighbourRule.Not:
                            angleRulesPass = angleRulesPass && !MatchingNeighbour(adjacentTileData.AdjacentTiles[i]);
                            string _passLog = angleRulesPass ? "<color=green>PASS</color>" : "<color=red>FAIL</color>";
                            this.Debug($"{AdjacentTileData.IndexToString[i]} - Not? = {_passLog}", LoggingPriority.Low);
                            break;
                        default:
                            break;
                    }
                    if (!angleRulesPass) break;
                }
                if (angleRulesPass)
                {
                    Angle = angle;
                    return true;
                }
            }
            Angle = 0;
            return false;
        }

        private bool RotatedMirrorMatch(ITileData Neighbour, out float Angle)
        {
            // Check every 90 degree angle
            for (int angle = 0; angle <= 270; angle += 90)
            {
                this.Verbose($"@{angle}::");
                NeighbourRule[] RotatedRules = new NeighbourRule[8];
                for (int i = 0; i < 8; i++)
                {
                    NeighbourRule rule = NeighbourRules[GetRotatedMirroredIndex(i, angle)];
                    RotatedRules[i] = rule;
                    this.Debug($"RotatedMirrorRules[{i}:{AdjacentTileData.IndexToString[i]}] = {rule}", LoggingPriority.Low);
                }

                bool angleRulesPass = true;
                // Check each neighbour
                for (int i = 0; i < 8; i++)
                {
                    switch (RotatedRules[i])
                    {
                        case NeighbourRule.Exists:
                            angleRulesPass = angleRulesPass && MatchingNeighbour(adjacentTileData.AdjacentTiles[i]);
                            string passLog = angleRulesPass ? "<color=green>PASS</color>" : "<color=red>FAIL</color>";
                            this.Debug($"{AdjacentTileData.IndexToString[i]} - Exists? = {passLog}", LoggingPriority.Low);
                            break;
                        case NeighbourRule.Not:
                            angleRulesPass = angleRulesPass && !MatchingNeighbour(adjacentTileData.AdjacentTiles[i]);
                            string _passLog = angleRulesPass ? "<color=green>PASS</color>" : "<color=red>FAIL</color>";
                            this.Debug($"{AdjacentTileData.IndexToString[i]} - Not? = {_passLog}", LoggingPriority.Low);
                            break;
                        default:
                            break;
                    }
                    if (!angleRulesPass) break;
                }
                if (angleRulesPass)
                {
                    Angle = angle;
                    return true;
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
