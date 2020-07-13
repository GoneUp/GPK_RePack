using System;
using GPK_RePack.Properties;
using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Targets.Wrappers;
using NLog.Windows.Forms;

namespace GPK_RePack.Forms
{
    class NLogConfig
    {
        private static FileTarget logfile;
        private static AsyncTargetWrapper formTarget;
        private static LoggingRule fileLoggingRule;


        public static void SetDefaultConfig()
        {
            LogManager.ThrowExceptions = true;
            var config = new LoggingConfiguration();  
            

           //Targets
            logfile = new FileTarget(); 
            logfile.FileName = "Terahelper.log";
            logfile.DeleteOldFileOnStartup = true;
            logfile.KeepFileOpen = true;
            AsyncTargetWrapper asyncWrapperLog = new AsyncTargetWrapper(logfile);
            config.AddTarget("logfile", asyncWrapperLog);

            var formTargetSync = new FormControlTarget();
            formTargetSync.Layout = "${date:format=HH\\:mm\\:ss} ${logger} # ${message} ${newline}";
            formTargetSync.Append = true;
            formTargetSync.ControlName = "boxLog";
            formTargetSync.FormName = "GUI";
            config.AddTarget("form", formTargetSync);
            formTarget = new AsyncTargetWrapper(formTargetSync);
            config.AddTarget("form", formTarget);

            //Rules
            var level = LogLevel.Trace;
            switch (Settings.Default.LogLevel)
            {
                case "info":
                    level = LogLevel.Info;
                    break;
                case "debug":
                    level = LogLevel.Debug;
                    break;
                case "trace":
                    level = LogLevel.Trace;
                    break;
            }
            fileLoggingRule = new LoggingRule("*", level, asyncWrapperLog);
            config.LoggingRules.Add(fileLoggingRule);

            var rule2 = new LoggingRule("*", LogLevel.Info, formTarget);
            config.LoggingRules.Add(rule2);

            
            LogManager.Configuration = config;
        }

        public static void ReloadFileLoggingRule()
        {
            var level = LogLevel.Trace;
            switch (Settings.Default.LogLevel)
            {
                case "info":
                    level = LogLevel.Info;
                    break;
                case "debug":
                    level = LogLevel.Debug;
                    break;
                case "trace":
                    level = LogLevel.Trace;
                    break;
            }

            LogManager.Configuration.LoggingRules.Remove(fileLoggingRule);
            fileLoggingRule.SetLoggingLevels(level, LogLevel.Fatal);
            LogManager.Configuration.LoggingRules.Add(fileLoggingRule);

        }

        public static void DisableFormLogging()
        {
            LogManager.Configuration.RemoveTarget("form");
        }

        public static void EnableFormLogging()
        {
            LogManager.Configuration.AddTarget("form", formTarget);
        }
    }
}
