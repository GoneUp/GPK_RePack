﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
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
        //bump version on new release
        private static Logger logger;

        public static void checkForUpdate(UpdaterCheckCallback callback)
        {           

            logger = LogManager.GetLogger("Updater");
            Task newTask = new Task(() =>
            {
                string output;
                Int32 onlineVersionCode = 0;
                try
                {
                    WebClient wc = new WebClient();
                    output = wc.DownloadString(Constants.UPDATE_URL);
                    onlineVersionCode = Int32.Parse(output);
                }
                catch (Exception we)
                {
                    logger.Debug("Update check failed. Exception");
                    logger.Debug(we);
                }

                
                logger.Debug("Online Versioncode {0}, Appversion {1}", onlineVersionCode, Constants.APP_VERSION);
                if (callback != null)
                {
                    callback.postUpdateResult(onlineVersionCode > Constants.APP_VERSION);
                }
            });
            newTask.Start();
        }
    }
}
