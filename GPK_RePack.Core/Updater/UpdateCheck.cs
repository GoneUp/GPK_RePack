using System;
using System.Net;
using System.Threading.Tasks;
using NLog;

namespace GPK_RePack.Core.Updater
{
    public class UpdateCheck
    {
        //bump version on new release
        private static Logger logger;

        public static void checkForUpdate(IUpdaterCheckCallback callback)
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
                    callback.PostUpdateResult(onlineVersionCode > Constants.APP_VERSION);
                }
            });
            newTask.Start();
        }
    }
}
