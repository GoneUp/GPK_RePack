using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GPK_RePack.Model;
using GPK_RePack.Model.Interfaces;
using NLog;

/*
 * Use powershell to mass dump all gpk names out of the tera folder 
 * ls -r -ea silentlycontinue -fo -inc "*.gpk" | % { $_.fullname } > filelist.txt
 * (Maps use *.gmp!)
 * */

namespace GPK_RePack.IO
{
    class MassDumper
    {

        private static Logger logger = LogManager.GetLogger("MassDumper");
        public static void DumpMassHeaders(String outfile, String[] gpkFiles)
        {
            try
            {


                if (gpkFiles.Length == 0) return;

                DateTime start = DateTime.Now;
                List<IProgress> runningReaders = new List<IProgress>();
                List<Task> runningTasks = new List<Task>();
                List<GpkPackage> loadedGpkPackages = new List<GpkPackage>();

                if (gpkFiles.Length == 1 && gpkFiles[0].EndsWith(".txt"))
                {
                    var listpath = gpkFiles[0];
                    gpkFiles = File.ReadAllLines(listpath);

                    logger.Info("Read {0} of gpk files from list", gpkFiles.Length);
                }


                logger.Debug("start");
                foreach (var path in gpkFiles)
                {
                    if (File.Exists(path))
                    {
                        Task newTask = new Task(() =>
                        {
                            Reader reader = new Reader();
                            runningReaders.Add(reader);
                            var tmpPack = reader.ReadGpk(path, true);
                            if (tmpPack != null)
                            {
                                loadedGpkPackages.AddRange(tmpPack);
                            }
                        });
                        newTask.Start();
                        runningTasks.Add(newTask);
                    }
                }


                Task.WaitAll(runningTasks.ToArray());

                logger.Debug("loading done");
                using (StreamWriter file = new StreamWriter(outfile))
                {
                    file.WriteLine("Terahelper GPK dump");

                    foreach (var gpk in loadedGpkPackages)
                    {
                        file.WriteLine("### {0} ###", gpk.Path);
                        foreach (var import in gpk.ImportList)
                        {
                            file.WriteLine("{0};{1}", gpk.Filename, import.Value.ToCompactString());
                        }

                        foreach (var export in gpk.ExportList)
                        {
                            file.WriteLine("{0};{1}", gpk.Filename, export.Value.ToCompactString());
                        }

                    }
                }
                logger.Debug("done");

                //filename
                //import
                //exports
            }
            catch (Exception ex)
            {
                logger.Fatal(ex);
            }
        }
    }
}
