using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GPK_RePack.Editors;
using GPK_RePack.Model;
using GPK_RePack.Model.Interfaces;
using GPK_RePack.Model.Payload;
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


        public static void DumpMassTextures(GpkStore store, String outdir)
        {
            logger.Info("Started dumping textures to " + outdir);
            Directory.CreateDirectory(outdir);

            List<Task> runningTasks = new List<Task>();

            foreach (var file in store.CompositeMap)
            {
                var fileOutPath = string.Format("{0}\\{1}.gpk\\", outdir, file.Key);
                Directory.CreateDirectory(fileOutPath);

                //limit to 5 threads by default
                foreach (var entry in file.Value)
                {
                    Task newTask = new Task(() =>
                    {
                        string path = string.Format("{0}\\{1}.gpk", store.BaseSearchPath, entry.SubGPKName);

                        if (!File.Exists(path))
                        {
                            logger.Warn("GPK to load not found. Searched for: " + path);
                            return;
                        }

                        Reader r = new Reader();
                        var package = r.ReadSubGpkFromComposite(path, entry.UID, entry.FileOffset, entry.FileLength);

                        //extract
                        var exports = package.GetExportsByClass("Core.Texture2D");

                        foreach (var export in exports)
                        {
                            //UID->Composite UID
                            //S1UI_Chat2.Chat2,c7a706fb_6a349a6f_1d212.Chat2_dup |
                            //we use this uid from pkgmapper
                            //var imagePath = string.Format("{0}{1}_{2}.dds", fileOutPath, entry.UID, export.UID);
                            var imagePath = string.Format("{0}{1}---{2}.dds", fileOutPath, entry.UID, export.UID);
                            TextureTools.exportTexture(export, imagePath);

                            logger.Info("Extracted texture {0} to {1}", entry.UID, imagePath);
                        }

                        package = null;
                    });

                    newTask.Start();
                    runningTasks.Add(newTask);
                }
            }

            Task.WaitAll(runningTasks.ToArray());


            logger.Info("Dumping done");
        }
    }
}
