using System;
using System.Collections.Generic;
using System.Text;
using GPK_RePack.Properties;
using NLog.LayoutRenderers;

namespace GPK_RePack.Classes
{
    class GpkPackage
    {
        public string Filename;
        public string Path;
        public long OrginalSize;
        public Boolean Changes = false;

        public GpkHeader Header;

        public Dictionary<long, GpkString> NameList;
        public Dictionary<long, GpkImport> ImportList;
        public Dictionary<long, GpkExport> ExportList;

        public List<String> UidList;

        public GpkPackage()
        {
            UidList = new List<string>();
            Header = new GpkHeader();

            NameList = new Dictionary<long, GpkString>();
            ImportList = new Dictionary<long, GpkImport>();
            ExportList = new Dictionary<long, GpkExport>();
        }

        public void AddString(string text)
        {
            long maxKey = 0;
            foreach (KeyValuePair<long, GpkString> pair in NameList)
            {
                if (pair.Key > maxKey) maxKey = pair.Key;
                if (pair.Value.name == text) return;
            }

            GpkString str = new GpkString();
            str.name = text;
            NameList.Add(maxKey + 1, str);
        }

        public long GetStringIndex(string text)
        {
            foreach (KeyValuePair<long, GpkString> pair in NameList)
            {
                if (pair.Value.name == text) return pair.Key;
            }

            throw new Exception(string.Format("Name {0} not found!", text));
        }

        public long GetObjectIndex(string text)
        {
            if (text == "none") return 0;

            foreach (KeyValuePair<long, GpkImport> pair in ImportList)
            {
                if (pair.Value.UID == text)
                {
                    long tmpKey = (pair.Key + 1) * -1; //0 -> 1 --> -1 ## 1 --> 2 --> -2 ##
                    return tmpKey;
                }
            }

            foreach (KeyValuePair<long, GpkExport> pair in ExportList)
            {
                if (pair.Value.UID == text)
                {
                    long tmpKey = (pair.Key + 1); //0 -> 1 --> -1 ## 1 --> 2 --> -2 ##
                    return tmpKey;
                }
            }

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

        public List<GpkExport> GetExportsByClass(string className)
        {
            List<GpkExport> tmpList = new List<GpkExport>();
            foreach (KeyValuePair<long, GpkExport> pair in ExportList)
            {
                if (pair.Value.ClassName == className)
                {
                    tmpList.Add(pair.Value);
                }
            }
            return tmpList;
        }

        public int GetActualSize()
        {
            int tmpSize = 0;
            tmpSize += Header.GetSize();
            
            foreach (KeyValuePair<long, GpkString> pair in NameList)
            {
                tmpSize += pair.Value.GetSize();
            }

            tmpSize += ImportList.Count * new GpkImport().GetSize();

            foreach (KeyValuePair<long, GpkExport> pair in ExportList)
            {
                tmpSize += pair.Value.GetSize(); //export list part
               // tmpSize += pair.Value.RecalculateSize(); //data part
            }

            foreach (KeyValuePair<long, GpkExport> pair in ExportList)
            {
                //tmpSize += pair.Value.GetSize(); //export list part
                tmpSize += pair.Value.GetDataSize(); //data part
            }

            tmpSize += 10; //puffer betwwen exportlist and data
            return tmpSize;
        }

        public string GetDiffString()
        {
            int computedSize = GetActualSize();
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
