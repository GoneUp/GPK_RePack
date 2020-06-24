using System;
using GPK_RePack.Properties;
using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Windows.Forms;

namespace GPK_RePack.Forms
{
    class NLogConfig
    {
        private static FileTarget logfile;
        private static FormControlTarget formTarget;


        public static void SetDefaultConfig()
        {
            LogManager.ThrowExceptions = true;
            var config = new LoggingConfiguration();  

           //Targets
            logfile = new FileTarget(); 
            logfile.FileName = "Terahelper.log";
            logfile.DeleteOldFileOnStartup = true;
            config.AddTarget("logfile", logfile);

            formTarget = new FormControlTarget();
            formTarget.Layout = "${date:format=HH\\:mm\\:ss} ${logger} # ${message} ${newline}";
            formTarget.Append = true;
            formTarget.ControlName = "boxLog";
            formTarget.FormName = "GUI";
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
            var rule1 = new LoggingRule("*", level, logfile);
            config.LoggingRules.Add(rule1);

            var rule2 = new LoggingRule("*", LogLevel.Info, formTarget);
            config.LoggingRules.Add(rule2);

            LogManager.Configuration = config;
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
