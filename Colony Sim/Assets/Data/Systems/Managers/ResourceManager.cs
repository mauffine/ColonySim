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
        public LoggingPriority LoggingPriority { get => _loggingPriority; set => _loggingPriority = value; }
        [SerializeField]
        private LoggingPriority _loggingPriority = LoggingPriority.AlwaysShow;
        public bool Stamp { get => _stamp; set => _stamp = value; }
        public string LoggingPrefix => "<color=blue>[RESOURCEMGR]</color>";
        [SerializeField]
        private bool _stamp = false;
        #endregion

        private const string EntityResourcePath = "Entities/";
        private const string EntityMaterialPath = EntityResourcePath + "Materials";
        private const string EntityTexturePath = EntityResourcePath + "Textures";
        private const string EntityShaderPath = EntityResourcePath + "Shaders";

        private const string CreatureResourcePath = "Creatures/";

        private const string CharacterResourcePath = CreatureResourcePath + "Characters/";
        private const string CharacterTexturePath = CharacterResourcePath + "Textures";

        private const string UtilityResourcePath = "Utility/";
        private const string UtilityTexturePath = UtilityResourcePath + "Textures";

        public override void Init()
        {
            this.Notice("> Resource Manager Init..");
            instance = this;
            base.Init();
        }

        public static Texture2D LoadEntityTexture(string textureName)
        {
            return instance.Instance_LoadEntityTexture(textureName);
        }

        private Texture2D Instance_LoadEntityTexture(string textureName)
        {
            this.Verbose($"Retrieving Tile Texture: {textureName}");
            string _path = $"{EntityTexturePath}/{textureName}";
            Texture2D tileTexture = Resources.Load(_path) as Texture2D;
            if (tileTexture != null)
            {
                return tileTexture;
            }
            this.Warning($"Failed to locate Texture: {textureName} at path: {_path}");
            return null;
        }

        public static Material LoadEntityMaterial(string materialName)
        {
            return instance.Instance_LoadEntityMaterial(materialName);
        }

        private Material Instance_LoadEntityMaterial(string textureName)
        {
            this.Verbose($"Retrieving Material: {textureName}");
            string _path = $"{EntityMaterialPath}/{textureName}";
            Material mat = Resources.Load(_path) as Material;
            if (mat != null)
            {
                return mat;
            }
            this.Warning($"Failed to locate Material: {textureName} at path: {_path}");
            return null;
        }

        public static Shader LoadEntityShader(string shaderName)
        {
            return instance.Instance_LoadEntityShader(shaderName);
        }

        private Shader Instance_LoadEntityShader(string shaderName)
        {
            this.Verbose($"Retrieving Shader: {shaderName}");
            string _path = $"{EntityShaderPath}/{shaderName}";
            Shader shader = Resources.Load(_path) as Shader;
            if (shader != null)
            {
                return shader;
            }
            this.Warning($"Failed to locate Material: {shaderName} at path: {_path}");
            return null;
        }

        public static Texture2D LoadCharacterTexture(string textureName)
        {
            return instance.Instance_LoadCharacterTexture(textureName);
        }

        private Texture2D Instance_LoadCharacterTexture(string textureName)
        {
            this.Verbose($"Retrieving Character Texture: {textureName}");
            string _path = $"{CharacterTexturePath}/{textureName}";
            Texture2D tileTexture = Resources.Load(_path) as Texture2D;
            if (tileTexture != null)
            {
                return tileTexture;
            }
            this.Warning($"Failed to locate Texture: {textureName} at path: {_path}");
            return null;
        }

        public static Texture2D LoadUtilityTexture(string textureName)
        {
            return instance.Instance_LoadUtilityTexture(textureName);
        }

        private Texture2D Instance_LoadUtilityTexture(string textureName)
        {
            this.Verbose($"Retrieving Character Texture: {textureName}");
            string _path = $"{UtilityTexturePath}/{textureName}";
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
