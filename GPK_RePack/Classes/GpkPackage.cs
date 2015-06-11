using System;
using System.Collections.Generic;

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
        public GpkDependList DependList;

        public GpkPackage()
        {
            Header = new GpkHeader();

            NameList = new Dictionary<long, GpkString>();
            ImportList = new Dictionary<long, GpkImport>();
            ExportList = new Dictionary<long, GpkExport>();
        }

        public long GetStringIndex(string text)
        {
            foreach (KeyValuePair<long, GpkString> pair in NameList)
            {
                if (pair.Value.name == text) return pair.Key;
            }

            throw new Exception(string.Format("ObjectName {0} not found!", text));
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
    }
}
