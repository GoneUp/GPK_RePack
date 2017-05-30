using System;
using System.IO;

namespace GPK_RePack.Model.ExportData
{
    [Serializable]
    class DataLoader
    {
        private readonly string FilePath;
        private readonly int Offset;
        public readonly int Length;

        //On demand load of files. 

        /*
        public DataLoader(byte[] data)
        {
            //preload class
            m_data = data;
        }
        */

        public DataLoader(string path, int off, int len)
        {
            //load on demand
            FilePath = path;
            Offset = off;
            Length = len;
        }

        public byte[] LoadData()
        {
            var m_data = new byte[Length];
            FileStream reader = File.OpenRead(FilePath);
            reader.Seek(Offset, SeekOrigin.Begin);
            reader.Read(m_data, 0, Length);
            reader.Close();
            reader.Dispose();

            return m_data;
            //catch errors? 
        }

        public override string ToString()
        {
            return string.Format("[DataLoader] Offset: {0} Length: {1} Path: {2}", Offset, Length, FilePath);
        }
    }
}
