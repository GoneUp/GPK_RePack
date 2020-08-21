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

        public void loadSubGpk(string path, CompositeMapEntry entry)
        {
            var reader = new Reader();
            var gpk = reader.ReadSubGpkFromComposite(path, entry.UID, entry.FileOffset, entry.FileLength);
            if (gpk == null)
                return;

            gpk.CompositeGpk = true;
            gpk.CompositeEntry = entry;
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
                            savepath = String.Format("{0}_{1}_{2}", package.Path, package.Filename, Settings.Default.SaveFileSuffix);
                            //ffe86d35_183.gpk_UID_rebuild
                        }

                        if (!(patchComposite && package.CompositeGpk))
                        {
                            //simple save
                            tmpS.SaveGpkPackage(package, savepath, usePadding);
                        }
                        else
                        {
                            var tmpPath = savepath + "_single";
                            tmpS.SaveGpkPackage(package, tmpPath, usePadding);

                            //ugly and quick, replace with direct memory save
                            //patch, move entries
                            var compositeData = File.ReadAllBytes(package.Path);
                            var patchData = File.ReadAllBytes(tmpPath);
                            var patchDiff = patchData.Length - package.CompositeEntry.FileLength;

                            var compositeSize = compositeData.Length;
                            var compsiteFileEnd = package.CompositeEntry.FileOffset + package.CompositeEntry.FileLength;
                            if (patchDiff >= 0)
                            {
                                //enlarge
                                Array.Resize(ref compositeData, compositeSize + patchDiff);
                            }
                            //move data up/down
                            var upperData = new byte[compositeSize - compsiteFileEnd];
                            Array.ConstrainedCopy(compositeData, compsiteFileEnd, upperData, 0, upperData.Length);

                            //patchdiff could be negative, so data can be moved down
                            Array.ConstrainedCopy(upperData, 0, compositeData, compsiteFileEnd + patchDiff, upperData.Length);

                            //copy it in
                            Array.ConstrainedCopy(patchData, 0, compositeData, package.CompositeEntry.FileOffset, patchData.Length);


                            if (patchDiff < 0)
                            {
                                //shrink
                                Array.Resize(ref compositeData, compositeSize + patchDiff);
                            }


                            File.WriteAllBytes(savepath, compositeData);

                            //patch mappings
                            if (package.CompositeEntry != null && package.CompositeEntry.FileLength != patchData.Length)
                            {

                                //modify entries accordingly
                                foreach (var entry in this.CompositeMap[Path.GetFileNameWithoutExtension(package.Path)])
                                {
                                    if (entry.FileOffset > package.CompositeEntry.FileOffset)
                                    {
                                        entry.FileOffset += patchDiff;
                                    }
                                }

                                //modify our entry
                                package.CompositeEntry.FileLength = patchData.Length;

                                MapperTools.WriteMappings(savepath, this);
                            }
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
