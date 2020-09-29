using System;
using GPK_RePack.Core.Helper;
using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Targets.Wrappers;
using NLog.Windows.Forms;

namespace GPK_RePack.Core
{
    [Target("CustomEventTarget")]
    public sealed class CustomEventTarget : TargetWithLayout
    {
        public static event Action<string> LogMessageWritten;

        protected override void Write(LogEventInfo logEvent)
        {
            base.Write(logEvent);
            LogMessageWritten?.Invoke(this.RenderLogEvent(this.Layout, logEvent));
        }

    }

    public class NLogConfig
    {
        public enum NlogFormConfig
        {
            WinForms,
            WPF
        }

        private static FileTarget logfile;
        private static LoggingRule fileLoggingRule;
        private static LoggingRule formLoggingRule;

        private static NlogFormConfig _formConfig;


        public static void SetDefaultConfig(NlogFormConfig fc)
        {
            _formConfig = fc;
            LogManager.ThrowExceptions = true;
            var config = new LoggingConfiguration();

            //Targets
            logfile = new FileTarget();
            logfile.FileName = "Terahelper.log";
            logfile.DeleteOldFileOnStartup = true;
            logfile.KeepFileOpen = true;
            AsyncTargetWrapper asyncWrapperLog = new AsyncTargetWrapper(logfile);
            config.AddTarget("logfile", asyncWrapperLog);

            var formTarget = GetFormTarget();
            config.AddTarget("form", formTarget);

            //Rules
            var level = LogLevel.Trace;
            switch (CoreSettings.Default.LogLevel)
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
        //public static void SetCustomConfig(Target customTarget)
        //{
        //    LogManager.ThrowExceptions = true;
        //    var config = new LoggingConfiguration();

        //    //Targets
        //    logfile = new FileTarget();
        //    logfile.FileName = "Terahelper.log";
        //    logfile.DeleteOldFileOnStartup = true;
        //    logfile.KeepFileOpen = true;
        //    AsyncTargetWrapper asyncWrapperLog = new AsyncTargetWrapper(logfile);
        //    config.AddTarget("logfile", asyncWrapperLog);

        //    config.AddTarget("form", customTarget);

        //    //Rules
        //    var level = LogLevel.Trace;
        //    switch (CoreSettings.Default.LogLevel)
        //    {
        //        case "info":
        //            level = LogLevel.Info;
        //            break;
        //        case "debug":
        //            level = LogLevel.Debug;
        //            break;
        //        case "trace":
        //            level = LogLevel.Trace;
        //            break;
        //    }
        //    fileLoggingRule = new LoggingRule("*", level, asyncWrapperLog);
        //    config.LoggingRules.Add(fileLoggingRule);

        //    formLoggingRule = new LoggingRule("*", LogLevel.Info, customTarget);
        //    config.LoggingRules.Add(formLoggingRule);


        //    LogManager.Configuration = config;
        //}

        public static void ReloadFileLoggingRule()
        {
            var level = LogLevel.Trace;
            switch (CoreSettings.Default.LogLevel)
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
            SetDefaultConfig(_formConfig);
        }


        private static AsyncTargetWrapper GetFormTarget()
        {
            const string layout = "${date:format=HH\\:mm\\:ss} ${logger} # ${message}";
            const int maxLines = 1000;
            const bool autoScroll = true;
            Target t;

            switch (_formConfig)
            {
                case NlogFormConfig.WinForms:
                    t = new RichTextBoxTarget
                    {
                        Layout = layout,
                        AutoScroll = autoScroll,
                        ControlName = "boxLog",
                        FormName = "GUI",
                        MaxLines = maxLines
                    };
                    break;
                case NlogFormConfig.WPF:
                    t = new CustomEventTarget
                    {
                        Layout = layout,
                    };
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return new AsyncTargetWrapper(t);
        }

    }
}
