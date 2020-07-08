using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using GPK_RePack.IO;
using GPK_RePack.Model.Composite;

namespace GPK_RePack.Model
{
    class GpkStore
    {
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

            LoadedGpkPackages.AddRange(gpks);

            if (triggerEvent) 
                PackagesChanged();
        }


        public void loadSubGpk(string path, string fileID, int fileOffset, int dataLength)
        {
            var reader = new Reader();
            var gpk = reader.ReadSubGpkFromComposite(path, fileID, fileOffset, dataLength);

            LoadedGpkPackages.Add(gpk);

            PackagesChanged();
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



    }
}
