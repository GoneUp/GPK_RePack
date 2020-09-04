using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using GPK_RePack.Forms;
using GPK_RePack.IO;
using GPK_RePack.Model.Composite;
using GPK_RePack.Model.Interfaces;
using GPK_RePack.Model.Payload;
using GPK_RePack.Properties;

namespace GPK_RePack.Model
{
    [Serializable]
    class GpkPackage
    {
        public string Path;
        public string Filename;

        //Composite
        public bool CompositeGpk;
        public long CompositeStartOffset;
        public long CompressedEndOffset = 0;
        public CompositeMapEntry CompositeEntry;

        //Info
        public long OrginalSize; //raw compressed size
        public long UncompressedSize;

        //Edit stuff
        public Boolean Changes = false;
        public Boolean LowMemMode = false;

        //data structs
        public GpkHeader Header;

        public Dictionary<long, GpkString> NameList;
        public Dictionary<long, GpkImport> ImportList;
        public Dictionary<long, GpkExport> ExportList;

        public Dictionary<String, IGpkPart> UidList;

        public readonly int datapuffer = 0;
        public bool x64;


        public GpkPackage()
        {
            UidList = new Dictionary<string, IGpkPart>();
            Header = new GpkHeader();

            NameList = new Dictionary<long, GpkString>();
            ImportList = new Dictionary<long, GpkImport>();
            ExportList = new Dictionary<long, GpkExport>();
        }

        #region string
        //returns index
        public long AddString(string text)
        {
            long maxKey = 0;
            foreach (KeyValuePair<long, GpkString> pair in NameList)
            {
                if (pair.Key > maxKey) maxKey = pair.Key;
                if (pair.Value.name == text) return pair.Key;
            }

            //flag 1970393556451328 is unk
            var newKey = maxKey + 1;
            GpkString str = new GpkString(text, 1970393556451328, true);
            NameList.Add(newKey, str);
            Header.NameCount++;

            return newKey;
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

            return AddString(text);
        }

        public int RemoveUnusedStrings()
        {
            //bad idea, we should not do this until we understand the FULL upk struct (which we will never do)
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
        public long AddExport(GpkExport export)
        {
            var key = ExportList.Max(x => x.Key) + 1;

            ExportList.Add(key, export);
            Header.ExportCount++;

            Reader.GenerateUID(this, export);
            export.motherPackage = this;
            return key;
        }

        public long AddImport(GpkImport import)
        {
            //check for existing
            foreach (var imp in ImportList)
            {
                if (imp.Value.UID == import.UID) return 0;
            }

            var key = ImportList.Max(x => x.Key) + 1;

            ImportList.Add(key, import);
            Header.ImportCount++;

            Reader.GenerateUID(this, import);
            return key;
        }

        public void CopyObjectFromPackage(string objectname, GpkPackage foreignPackage)
        {
            //recurse it down!
            if (objectname == null || objectname == "none") return;

            object copyObj = foreignPackage.GetObjectByUID(objectname);
            if (copyObj is GpkImport)
            {
                AddImport((GpkImport)copyObj);
                //resolve owners
                string owner = ((GpkImport)copyObj).OwnerObject;
                while (owner != "none")
                {
                    GpkImport ownerObj = (GpkImport)foreignPackage.GetObjectByUID(owner);
                    AddImport(ownerObj);

                    owner = ownerObj.OwnerObject;
                }
            } else {
                var exportObj = (GpkExport)copyObj;
                AddExport(exportObj);

                CopyObjectFromPackage(exportObj.ClassName, foreignPackage);
                CopyObjectFromPackage(exportObj.PackageName, foreignPackage);
                CopyObjectFromPackage(exportObj.SuperName, foreignPackage);
                CopyObjectFromPackage(exportObj.NetIndexName, foreignPackage);
            }

        }

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

        public int GetObjectIndex(string text)
        {
            if (text == "none") return 0;

            var gpkObject = UidList[text];
            if (gpkObject is GpkImport)
            {
                foreach (var pair in ImportList)
                {
                    if (pair.Value == gpkObject)
                    {
                        return (int)(pair.Key + 1) * -1;
                    }
                }
            }
            else if (gpkObject is GpkExport)
            {
                foreach (var pair in ExportList)
                {
                    if (pair.Value == gpkObject)
                    {
                        return (int)pair.Key + 1;
                    }
                }

            }

            throw new Exception(string.Format("Object {0} not found!", text));
        }

        public long GetObjectKeyByUID(string uid)
        {
            if (uid == "none") return 0;

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
        #endregion
        public void PrepareWriting(bool enableCompression)
        {
            //set new offsets
            //set coutn values on header
            //check names
            foreach (var i in ImportList.Values)
            {
                i.CheckNamePresence(this);
            }
            foreach (var i in ExportList.Values)
            {
                i.CheckNamePresence(this);
            }

            GetSize(true);
            Header.RecalculateCounts(this);


            if (enableCompression)
            {
                Header.PackageFlags |= (int)GpkPackageFlags.Compressed;
                Header.CompressionFlags = (int)CompressionTypesPackage.LZO;
            }
            else
            {
                //null out compressflags
                Header.PackageFlags &= ~(int)GpkPackageFlags.Compressed;

                Header.CompressionFlags = 0;
                Header.ChunkHeaders.Clear();
                Header.EstimatedChunkHeaderCount = 0;
            }
        }

        public int EstimateChunkHeaderCount()
        {
            int chunkheadercount = (GetSize(false) / Constants.DEFAULT_CHUNKSIZE) + 1;
            Header.EstimatedChunkHeaderCount = chunkheadercount;
            return chunkheadercount;
        }

        public void GenerateChunkHeaders(int dataSize, int dataStartOffset, byte[] buffer)
        {
            int chunkheadercount = (dataSize / Constants.DEFAULT_CHUNKSIZE) + 1;
            int oldChunkheadercount = Header.ChunkHeaders.Count();

            Header.ChunkHeaders.Clear();

            int bufferOffset = dataStartOffset;
            int endOfData = dataStartOffset + dataSize;
            for (int i = 0; i < chunkheadercount; i++)
            {
                var chunkHeader = new GpkCompressedChunkHeader();
                chunkHeader.UncompressedOffset = bufferOffset;


                var chunkData = new PackageChunkBlock();
                chunkData.signature = Constants.DEFAULT_SIGNATURE;
                chunkData.blocksize = Constants.DEFAULT_BLOCKSIZE;

                if (bufferOffset + Constants.DEFAULT_CHUNKSIZE > endOfData)
                {
                    //ending
                    chunkData.uncompressedSize_chunkheader = endOfData - bufferOffset;
                }
                else
                {
                    chunkData.uncompressedSize_chunkheader = Constants.DEFAULT_CHUNKSIZE;
                }

                //chunks has blocks
                int blockCount = Convert.ToInt32(Math.Ceiling(chunkData.uncompressedSize_chunkheader / (double)chunkData.blocksize));
                byte[] uncompressedChunkData = new byte[chunkData.uncompressedSize_chunkheader];
                Array.ConstrainedCopy(buffer, bufferOffset, uncompressedChunkData, 0, chunkData.uncompressedSize_chunkheader);


                chunkData.Compress(uncompressedChunkData, blockCount, Constants.DEFAULT_BLOCKSIZE, Header.CompressionFlags);

                //set final data
                chunkHeader.writableChunkblock = chunkData;
                chunkHeader.UncompressedSize = chunkData.uncompressedSize_chunkheader;

                Header.ChunkHeaders.Add(chunkHeader);

                //END
                bufferOffset += chunkData.uncompressedSize_chunkheader;
            }
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

            if (fixOffsets) Header.DependsOffset = tmpSize;
            tmpSize += 4 * ExportList.Count;

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
            info.AppendLine("Is x64 GPK: " + x64);
            return info.ToString();
        }
    }
}
