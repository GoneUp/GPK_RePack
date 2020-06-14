using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GPK_RePack.Forms;
using GPK_RePack.IO;
using GPK_RePack.Properties;

namespace GPK_RePack.Model
{
    [Serializable]
    class GpkPackage
    {
        public string Filename;
        public string Path;
        public long OrginalSize;
        public long UncompressedSize;
        public Boolean Changes = false;

        public GpkHeader Header;

        public Dictionary<long, GpkString> NameList;
        public Dictionary<long, GpkImport> ImportList;
        public Dictionary<long, GpkExport> ExportList;

        public Dictionary<String, String> UidList;

        public readonly int datapuffer = 10;
        public bool x64;

        public GpkPackage()
        {
            UidList = new Dictionary<string, string>();
            Header = new GpkHeader();

            NameList = new Dictionary<long, GpkString>();
            ImportList = new Dictionary<long, GpkImport>();
            ExportList = new Dictionary<long, GpkExport>();
        }

        #region string
        public void AddString(string text)
        {
            long maxKey = 0;
            foreach (KeyValuePair<long, GpkString> pair in NameList)
            {
                if (pair.Key > maxKey) maxKey = pair.Key;
                if (pair.Value.name == text) return;
            }

            //flag 1970393556451328 is unk
            GpkString str = new GpkString(text, 1970393556451328, true);
            NameList.Add(maxKey + 1, str);
            Header.NameCount++;
        }

        public string GetString(long index)
        {
            if (NameList.ContainsKey(index))
            {
                NameList[index].used = true;
                return NameList[index].name;
            }

            throw new Exception(string.Format("NameIndex {0} not found!", index));
        }

        public long GetStringIndex(string text)
        {
            foreach (KeyValuePair<long, GpkString> pair in NameList)
            {
                if (pair.Value.name == text) return pair.Key;
            }

            throw new Exception(string.Format("Name {0} not found!", text));
        }

        public int RemoveUnusedStrings()
        {
            int removed = 0;
            int count = 0;

            if (!Settings.Default.Debug) return 0;

            var listCopy = NameList.ToArray();
            foreach (KeyValuePair<long, GpkString> pair in listCopy)
            {
                if (pair.Value.used)
                {
                    //renumbering for the case something was removed before
                    if (pair.Key != count)
                    {
                        NameList.Remove(pair.Key);
                        NameList.Add(count, pair.Value);
                    }

                    count++;
                }
                else
                {
                    NameList.Remove(pair.Key); //test
                    removed++;
                }
            }

            return removed;
        }
        #endregion

        #region objects
        public string GetObjectName(int index, bool returnNull = false)
        {
            //Import, Export added due to diffrent files appear to have the same object on import and export list

            if (index == 0)
            {
                return "none";
            }
            if (index < 0)
            {
                if (!ImportList.ContainsKey((index * -1) - 1))
                {
                    //not found. return null or we get a exception.  
                    if (returnNull) return null;
                }

                return ImportList[((index * -1) - 1)].UID;
            }
            if (index > 0)
            {
                if (!ExportList.ContainsKey(index - 1))
                {
                    //not found. return null or we get a excaption.  
                    if (returnNull) return null;
                }

                GpkExport export = ExportList[index - 1];
                if (export.UID == null)
                {
                    export.ClassName = GetObjectName(export.ClassIndex);
                    export.SuperName = GetObjectName(export.SuperIndex);
                    export.PackageName = GetObjectName(export.PackageIndex);
                    export.UID = Reader.GenerateUID(this, export);
                }
                return export.UID;
            }

            if (returnNull) return null;

            throw new Exception(string.Format("Object {0} not found!", index));
        }

        public long GetObjectIndex(string text)
        {
            if (text == "none") return 0;

            long parallelKey = 1;
            Parallel.ForEach(ImportList, (pair, state) =>
            {
                if (pair.Value.UID == text)
                {
                    parallelKey = (pair.Key + 1) * -1;
                    state.Break();
                }
            });
            if (parallelKey != 1) return parallelKey;

            parallelKey = -1;
            Parallel.ForEach(ExportList, (pair, state) =>
            {
                if (pair.Value.UID == text)
                {
                    parallelKey = pair.Key + 1;
                    state.Break();
                }
            });

            if (parallelKey != -1) return parallelKey;

            throw new Exception(string.Format("Object {0} not found!", text));
        }

        public long GetObjectKeyByUID(string uid)
        {
            foreach (KeyValuePair<long, GpkImport> pair in ImportList)
            {
                if (pair.Value.UID == uid)
                {
                    return pair.Key;
                }
            }

            foreach (KeyValuePair<long, GpkExport> pair in ExportList)
            {
                if (pair.Value.UID == uid)
                {
                    return pair.Key;
                }
            }

            throw new Exception(string.Format("Object {0} not found!", uid));
        }


        public object GetObjectByUID(string uid)
        {
            foreach (KeyValuePair<long, GpkImport> pair in ImportList)
            {
                if (pair.Value.UID == uid)
                {
                    return pair.Value;
                }
            }

            foreach (KeyValuePair<long, GpkExport> pair in ExportList)
            {
                if (pair.Value.UID == uid)
                {
                    return pair.Value;
                }
            }

            throw new Exception(string.Format("Object {0} not found!", uid));
        }
#endregion

        public List<GpkExport> GetExportsByClass(string className)
        {
            List<GpkExport> tmpList = new List<GpkExport>();

            if (className == "#all")
            {
                tmpList = ExportList.Values.ToList();
                return tmpList;
            }

            foreach (KeyValuePair<long, GpkExport> pair in ExportList)
            {
                if (pair.Value.ClassName == className)
                {
                    tmpList.Add(pair.Value);
                }
            }
            return tmpList;
        }

        public void PrepareWriting()
        {
            //set new offsets
            //set coutn values on header
            //remove unused strings
            int count = RemoveUnusedStrings();
            GUI.logger.Debug("Removed {0} unused strings", count);
            Header.RecalculateCounts(this);
            GetSize(true);
        }

        public int GetSize(bool fixOffsets)
        {
            int tmpSize = 0;
            tmpSize += Header.GetSize();

            if (fixOffsets) Header.NameOffset = tmpSize;
            foreach (KeyValuePair<long, GpkString> pair in NameList)
            {
                tmpSize += pair.Value.GetSize();
            }

            if (fixOffsets) Header.ImportOffset = tmpSize;
            tmpSize += ImportList.Count * new GpkImport().GetSize();

            if (fixOffsets) Header.ExportOffset = tmpSize;
            foreach (KeyValuePair<long, GpkExport> pair in ExportList)
            {
                tmpSize += pair.Value.GetSize(); //export list part
            }

            tmpSize += datapuffer; //puffer betwwen exportlist and data

            foreach (KeyValuePair<long, GpkExport> pair in ExportList)
            {
                if (fixOffsets) pair.Value.SerialOffset = tmpSize;
                tmpSize += pair.Value.GetDataSize(); //data part
            }

            return tmpSize;
        }

        public string GetDiffString()
        {
            int computedSize = GetSize(false);
            return String.Format("Computed size {0} bytes, Orignal size {1} bytes. Difference: {2} bytes", computedSize, OrginalSize, computedSize - OrginalSize);
        }

        public override string ToString()
        {
            StringBuilder info = new StringBuilder();
            info.AppendLine("Type: Package");
            info.AppendLine("Name: " + Filename);
            info.AppendLine("Path: " + Path);
            info.AppendLine("Orginal Size: " + OrginalSize);
            info.AppendLine(GetDiffString());
            info.AppendLine("Name count: " + NameList.Count);
            info.AppendLine("Import count: " + ImportList.Count);
            info.AppendLine("Export count: " + ExportList.Count);
            info.AppendLine("UID count: " + UidList.Count);
            return info.ToString();
        }
    }
}
