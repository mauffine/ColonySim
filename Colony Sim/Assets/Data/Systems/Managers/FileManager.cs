using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;  
using ColonySim.LoggingUtility;
using ILogger = ColonySim.LoggingUtility.ILogger;
using System.IO;
using System;
using ColonySim.Entities;

namespace ColonySim.Systems
{
    public interface IJsonSaveUtility
    {
        string SaveToJson();
        string LoadFromJson(string JSON);
    }

    public class FileManager : System, ILogger
    {
        #region Static
        private static FileManager instance;
        public static FileManager Get => instance;
        public List<string> Logs { get; set; } = new List<string>();
        public LoggingLevel LoggingLevel { get => _loggingLevel; set => _loggingLevel = value; }
        [SerializeField]
        private LoggingLevel _loggingLevel = LoggingLevel.Warning;
        public LoggingPriority LoggingPriority { get => _loggingPriority; set => _loggingPriority = value; }
        [SerializeField]
        private LoggingPriority _loggingPriority = LoggingPriority.AlwaysShow;
        public bool Stamp { get => _stamp; set => _stamp = value; }
        public string LoggingPrefix => "<color=navy>[FILEMANAGER]</color>";
        [SerializeField]
        private bool _stamp = false;
        #endregion

        public static string JSONFileDirectory => ApplicationController.DataPath + "/JSON/";

        public override void Init()
        {
            this.Notice("<color=blue>[File Manager Init]</color>");
            instance = this;
            Initialized = true;
            base.Init();
        }

        public bool WriteToFile(string file, string dir, string fileName, string extension, bool overwrite)
        {
            string path = dir + fileName + extension;
            Stream Stream = new FileStream(dir, FileMode.CreateNew, FileAccess.Write, FileShare.Write);
            this.Notice($"Saving {fileName} at {path}...");

            if (File.Exists(path))
            {
                if (overwrite)
                {
                    this.Notice("<color=blue>File Already Exists! Overwritting...</color>");
                    File.Delete(path);
                }
                else
                {
                    this.Warning("<color=red>File Already Exists! Aborting</color>");
                    return false;
                }
            }

            try
            {
                using (StreamWriter writer = new StreamWriter(Stream))
                {
                    writer.Write(file);
                }
                return true;
            }
            catch (Exception)
            {
                throw new IOException("Failed to Serialize Path::" + path);
            }
        }

        public bool ReadFromFile(string dir, string fileName, string extension, out string JSON)
        {
            string path = dir + fileName + extension;
            this.Notice($"<color=blue>Deserializing file at {path}</color>");
            if (!File.Exists(path))
            {
                this.Warning($"Invalid path::{path}");
                JSON = null;
                return false;
            }

            Stream Stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            try
            {
                using(StreamReader reader = new StreamReader(Stream))
                {
                    JSON = reader.ReadToEnd();
                }
                return true;
            }
            catch (Exception)
            {
                throw new FileLoadException("Could not load object at::" + path);
            }
        }

        public string[] GetFilesAtDir(string path, string[] extensions)
        {
            List<string> fileNames = new List<string>();
            if (Directory.Exists(path))
            {
                var info = new DirectoryInfo(path);
                foreach (var file in info.EnumerateFiles("*", SearchOption.AllDirectories))
                {
                    if (IsValidFile(file.Name, extensions))
                    {
                        fileNames.Add(Path.GetFileNameWithoutExtension(file.Name));
                    }
                }
            }
            else
            {
                this.Error("Event Directory Missing - " + path);
            }
            return fileNames.ToArray();
        }

        private bool IsValidFile(string file, string[] extensions)
        {
            foreach (var extension in extensions)
            {
                Debug.Log("Checking Extension::" + extension);
                if (extension == Path.GetExtension(file))
                {
                    return true;
                }
            }
            return false;
        }

        public bool DeleteFile(string dir, string fileName, string extension)
        {
            string path = dir + fileName + extension;
            if (File.Exists(path))
            {
                this.Notice("Deleting file::" + path);
                File.Delete(path);
                return true;
            }
            return false;
        }

        public string JSONSerialize(object graph)
        {
            this.Notice($"<color=blue>Serializing {graph}...</color>");
            string JSON = JsonUtility.ToJson(graph);
            return JSON;
        }
    }
}
