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
    }

    public static class LoggingUtility
    {
        private static string CreateLog(this ILogger self, string text, LoggingLevel level)
        {
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

        public static void Verbose(this ILogger self, string text, LoggingLevel level = LoggingLevel.Verbose)
        {
            CreateLog(self, text, level);
        }

        public static void VerbseFormat(this ILogger self, string text, params object[] args)
        {
            CreateLog(self, string.Format(text, args), LoggingLevel.Verbose);
        }

        public static void Debug(this ILogger self, string text)
        {
            CreateLog(self, text, LoggingLevel.Debug);
        }

        public static void DebugFormat(this ILogger self, string text, params object[] args)
        {
            CreateLog(self, string.Format(text, args), LoggingLevel.Debug);
        }

        public static void Notice(this ILogger self, string text)
        {
            CreateLog(self, text, LoggingLevel.Notice);
        }

        public static void NoticeFormat(this ILogger self, string text, params object[] args)
        {
            CreateLog(self, string.Format(text, args), LoggingLevel.Notice);
        }

        public static void Warning(this ILogger self, string text)
        {
            CreateLog(self, text, LoggingLevel.Warning);
        }

        public static void WarningFormat(this ILogger self, string text, params object[] args)
        {
            CreateLog(self, string.Format(text, args), LoggingLevel.Warning);
        }

        public static void Error(this ILogger self, string text)
        {
            CreateLog(self, text, LoggingLevel.Error);
        }

        public static void ErrorFormat(this ILogger self, string text, params object[] args)
        {
            CreateLog(self, string.Format(text, args), LoggingLevel.Error);
        }

        public static void Exception(this ILogger self, string text)
        {
            CreateLog(self, text, LoggingLevel.Exception);
        }

        public static void ExceptionFormat(this ILogger self, string text, params object[] args)
        {
            CreateLog(self, string.Format(text, args), LoggingLevel.Exception);
        }
    }
}