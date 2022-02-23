using System;
using System.Collections.Generic;
using UnityEngine;

namespace ColonySim.LoggingUtility
{
    public enum LoggingLevel
    {
        NeverShow = 0,
        Exception = 1,
        Error = 2,
        Warning = 3,
        Notice = 4,
        Verbose = 5,
        Debug = 6,
        AlwaysShow = 7
    }

    public interface ILogger
    {
        LoggingLevel LoggingLevel { get; set; }
        List<string> Logs { get; set; }
        bool Stamp { get; }
        string LoggingPrefix { get; }
    }

    public interface ILoggerSlave
    {
        ILogger Master { get; }
        string LoggingPrefix { get; }
    }

    public static class LoggingUtility
    {
        private static string CreateLog(this ILogger self, string text, LoggingLevel level)
        {
            if (!String.IsNullOrEmpty(self.LoggingPrefix))
            {
                text = text.Insert(0, $"{self.LoggingPrefix}");
            }
            if (self.Stamp)
            {
                text = text.Insert(0, "(" + level.ToString().ToUpper() + ":" + Time.time + ")");
            }

#if DEBUG

            if ((int)level <= (int)self.LoggingLevel)
            {
                if ((int)level <= 2)
                {
                    text = text.Insert(0, "!!!\n");
                    text = text.Insert(text.Length, "\n!!!");
                }
                else if ((int)level == 3)
                {
                    UnityEngine.Debug.LogWarning(text);
                }
                else
                {
                    UnityEngine.Debug.Log(text);
                }
            }
#endif
            self.Logs.Add(text);
            return text;
        }

        private static string CreateLog(this ILoggerSlave self, string text, LoggingLevel level)
        {
            if (!String.IsNullOrEmpty(self.LoggingPrefix))
            {
                text = text.Insert(0, $"{self.LoggingPrefix}");
            }

            if (self.Master.Stamp)
            {
                text = text.Insert(0, "(" + level.ToString().ToUpper() + ":" + Time.time + ")");
            }

#if DEBUG

            if ((int)level <= (int)self.Master.LoggingLevel)
            {
                if ((int)level <= 2)
                {
                    text = text.Insert(0, "!!!\n");
                    text = text.Insert(text.Length, "\n!!!");
                }
                else if ((int)level == 3)
                {
                    UnityEngine.Debug.LogWarning(text);
                }
                else
                {
                    UnityEngine.Debug.Log(text);
                }
            }
#endif
            self.Master.Logs.Add(text);
            return text;
        }

        public static void Verbose(this ILogger self, string text, LoggingLevel level = LoggingLevel.Verbose)
        {
            CreateLog(self, text, level);
        }

        public static void Debug(this ILogger self, string text)
        {
            CreateLog(self, text, LoggingLevel.Debug);
        }

        public static void Notice(this ILogger self, string text)
        {
            CreateLog(self, text, LoggingLevel.Notice);
        }

        public static void Warning(this ILogger self, string text)
        {
            CreateLog(self, text, LoggingLevel.Warning);
        }

        public static void Error(this ILogger self, string text)
        {
            CreateLog(self, text, LoggingLevel.Error);
        }

        public static void Exception(this ILogger self, string text)
        {
            CreateLog(self, text, LoggingLevel.Exception);
        }

        // Slaves

        public static void Verbose(this ILoggerSlave self, string text, LoggingLevel level = LoggingLevel.Verbose)
        {
            CreateLog(self, text, level);
        }

        public static void Debug(this ILoggerSlave self, string text)
        {
            CreateLog(self, text, LoggingLevel.Debug);
        }

        public static void Notice(this ILoggerSlave self, string text)
        {
            CreateLog(self, text, LoggingLevel.Notice);
        }

        public static void Warning(this ILoggerSlave self, string text)
        {
            CreateLog(self, text, LoggingLevel.Warning);
        }

        public static void Error(this ILoggerSlave self, string text)
        {
            CreateLog(self, text, LoggingLevel.Error);
        }

        public static void Exception(this ILoggerSlave self, string text)
        {
            CreateLog(self, text, LoggingLevel.Exception);
        }
    }
}