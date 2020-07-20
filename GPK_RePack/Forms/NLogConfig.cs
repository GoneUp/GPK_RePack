using System;
using System.Windows.Forms;
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
        private static LoggingRule fileLoggingRule;
        private static LoggingRule formLoggingRule;


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

            var formTarget = getFormTarget();
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

            formLoggingRule = new LoggingRule("*", LogLevel.Info, formTarget);
            config.LoggingRules.Add(formLoggingRule);


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
            LogManager.Configuration.LoggingRules.Remove(formLoggingRule);
        }

        public static void EnableFormLogging()
        {
            SetDefaultConfig();
        }


        private static AsyncTargetWrapper getFormTarget()
        {
            var formTargetSync = new RichTextBoxTarget();
            formTargetSync.Layout = "${date:format=HH\\:mm\\:ss} ${logger} # ${message}";
            formTargetSync.AutoScroll = true;
            formTargetSync.ControlName = "boxLog";
            formTargetSync.FormName = "GUI";
            formTargetSync.MaxLines = 1000;
            var formTarget = new AsyncTargetWrapper(formTargetSync);

            return formTarget;
        }

    }
}
