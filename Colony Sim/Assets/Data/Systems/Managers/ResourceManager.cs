using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ColonySim.World;
using ColonySim.Entities;
using ColonySim.LoggingUtility;
using ILogger = ColonySim.LoggingUtility.ILogger;

namespace ColonySim.Systems
{
    public class ResourceManager : System, ILogger
    {
        #region Static
        private static ResourceManager instance;
        public static ResourceManager Get => instance;
        public List<string> Logs { get; set; } = new List<string>();
        public LoggingLevel LoggingLevel { get => _loggingLevel; set => _loggingLevel = value; }
        [SerializeField]
        private LoggingLevel _loggingLevel = LoggingLevel.Warning;
        public bool Stamp { get => _stamp; set => _stamp = value; }
        [SerializeField]
        private bool _stamp = false;
        #endregion

        private readonly string EntityResourcePath = "Images/Entities";

        public override void Init()
        {
            this.Verbose("<color=blue>[Resource Manager Init]</color>");
            instance = this;
            base.Init();
        }

        public static Texture2D GetEntityTileSprite(string textureName)
        {
            return instance.Instance_GetEntityTileSprite(textureName);
        }

        private Texture2D Instance_GetEntityTileSprite(string textureName)
        {
            this.Verbose($"Retrieving Tile Texture: {textureName}");
            string _path = $"{EntityResourcePath}/Tiles/{textureName}";
            Texture2D tileTexture = Resources.Load(_path) as Texture2D;
            if (tileTexture != null)
            {
                return tileTexture;
            }
            this.Warning($"Failed to locate Texture: {textureName} at path: {_path}");
            return null;
        }
    }
}
