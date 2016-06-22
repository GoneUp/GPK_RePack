using System;
using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Windows.Forms;

namespace GPK_RePack.Forms
{
    class NLogConfig
    {
        public static void SetDefaultConfig()
        {
            LogManager.ThrowExceptions = true;
            var config = new LoggingConfiguration();  

           //Targets
            var logfile = new FileTarget(); 
            logfile.FileName = "Terahelper.log";
            logfile.DeleteOldFileOnStartup = true;
            config.AddTarget("logfile", logfile);

            var formTarget = new FormControlTarget();
            formTarget.Layout = "${date:format=HH\\:mm\\:ss} ${logger} # ${message} ${newline}";
            formTarget.Append = true;
            formTarget.ControlName = "boxLog";
            formTarget.FormName = "GUI";
            config.AddTarget("form", formTarget);
     
            //Rules
            var rule1 = new LoggingRule("*", LogLevel.Trace, logfile);
            config.LoggingRules.Add(rule1);

            var rule2 = new LoggingRule("*", LogLevel.Info, formTarget);
            config.LoggingRules.Add(rule2);

            LogManager.Configuration = config;
        }
    }
}
