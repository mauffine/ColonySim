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

        private const string EntityResourcePath = "Entities/";
        private const string EntityMaterialPath = EntityResourcePath + "Materials";
        private const string EntityTexturePath = EntityResourcePath + "Textures";
        private const string EntityShaderPath = EntityResourcePath + "Shaders";

        public override void Init()
        {
            this.Verbose("<color=blue>[Resource Manager Init]</color>");
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
    }
}
