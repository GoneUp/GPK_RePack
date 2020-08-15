using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using GPK_RePack.IO;
using GPK_RePack.Model.Composite;
using GPK_RePack.Model.Interfaces;
using GPK_RePack.Properties;
using NLog;

namespace GPK_RePack.Model
{
    class GpkStore
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public Dictionary<String, List<CompositeMapEntry>> CompositeMap;
        public string BaseSearchPath;
        public List<GpkPackage> LoadedGpkPackages { get; } = new List<GpkPackage>();

        //events
        public delegate void GpkListChangeHandler();
        public event GpkListChangeHandler PackagesChanged;

        public GpkStore()
        {
            CompositeMap = new Dictionary<string, List<CompositeMapEntry>>();
        }

        public void loadGpk(string path, Reader reader = null, bool triggerEvent = true)
        {
            if (reader == null)
                reader = new Reader();

            var gpks = reader.ReadGpk(path, false);
            if (gpks == null)
                return;

            LoadedGpkPackages.AddRange(gpks);

            if (triggerEvent)
                PackagesChanged();
        }

        public void loadSubGpk(string path, string fileID, int fileOffset, int dataLength)
        {
            var reader = new Reader();
            var gpk = reader.ReadSubGpkFromComposite(path, fileID, fileOffset, dataLength);
            if (gpk == null)
                return;

            LoadedGpkPackages.Add(gpk);

            PackagesChanged();
        }

        public void SaveGpkListToFiles(List<GpkPackage> saveList, bool usePadding, bool patchComposite, List<IProgress> runningSavers, List<Task> runningTasks)
        {
            //todo: detect if we have modified multiplie gpks from composite and patch them all to a signle output

            foreach (GpkPackage package in saveList)
            {
                try
                {
                    Writer tmpS = new Writer();
                    Task newTask = new Task(() =>
                    {
                        string savepath = null;
                        if (!package.CompositeGpk)
                        {
                            savepath = package.Path + Settings.Default.SaveFileSuffix;
                        }
                        else
                        {
                            savepath = String.Format("{0}\\{1}{2}", Path.GetDirectoryName(package.Path), package.Filename, Settings.Default.SaveFileSuffix);
                        }

                        if (!patchComposite)
                        {
                            //simple save
                            tmpS.SaveGpkPackage(package, savepath, usePadding);
                        }
                        else
                        {
                            var tmpPath = savepath + "_tmp";
                            tmpS.SaveGpkPackage(package, tmpPath, usePadding);

                            //ugly and quick, replace with direct memory save
                            var compositeData = File.ReadAllBytes(package.Path);
                            var patchData = File.ReadAllBytes(tmpPath);
                            Array.ConstrainedCopy(patchData, 0, compositeData, (int)package.CompositeStartOffset, patchData.Length);

                            File.WriteAllBytes(savepath, compositeData);
                        }


                    });
                    newTask.Start();
                    runningTasks.Add(newTask);
                    runningSavers.Add(tmpS);
                }
                catch (Exception ex)
                {
                    logger.Fatal(ex, "Save failure!");
                }

            }
        }

        public void DeleteGpk(GpkPackage package)
        {
            LoadedGpkPackages.Remove(package);

            PackagesChanged();
        }
        public void clearGpkList()
        {
            LoadedGpkPackages.Clear();

            PackagesChanged();
        }

        public void clearCompositeMap()
        {
            CompositeMap = new Dictionary<string, List<CompositeMapEntry>>();
            BaseSearchPath = "";
        }


    }
}
