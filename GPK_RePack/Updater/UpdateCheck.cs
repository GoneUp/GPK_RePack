using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using GPK_RePack.Properties;
using NLog;

namespace GPK_RePack.Updater
{
    class UpdateCheck
    {
        private static Logger logger;

        public static void checkForUpdate(UpdaterCheckCallback callback)
        {

            logger = LogManager.GetLogger("Updater");
            Task newTask = new Task(() =>
            {
                string output;
                Int32 versionCode = 0;
                try
                {
                    WebClient wc = new WebClient();
                    output = wc.DownloadString(Settings.Default.UpdateUrl);
                    versionCode = Int32.Parse(output);
                }
                catch (Exception we)
                {
                    logger.Debug("Update check failed. Exception");
                    logger.Debug(we);
                }

                logger.Debug("Online Versioncode {0}, Appversion {1}", versionCode, Settings.Default.VersionCode);
                if (callback != null)
                {
                    callback.postUpdateResult(versionCode > Settings.Default.VersionCode);
                }
            });
            newTask.Start();
        }
    }
}
