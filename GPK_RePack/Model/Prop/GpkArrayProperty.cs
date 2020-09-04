using System;
using System.IO;
using GPK_RePack.Model.Interfaces;

namespace GPK_RePack.Model.Prop
{
    [Serializable]
    class GpkArrayProperty : GpkBaseProperty, IProperty
    {
        public byte[] value;

        public GpkArrayProperty()
        {
            RecalculateSize();
        }
        public GpkArrayProperty(GpkBaseProperty bp)
        {
            name = bp.name;
            type = bp.type;
            size = bp.size;
            arrayIndex = bp.arrayIndex;
        }

        public override string ToString()
        {
            return string.Format("ObjectName: {0} Type: {1} Length: {2}", name, type, size);
        }

        public void WriteData(BinaryWriter writer, GpkPackage package)
        {
            writer.Write(value);
        }

        public void ReadData(BinaryReader reader, GpkPackage package)
        {
            value = new byte[size];
            value = reader.ReadBytes(size);

            //real array parsing experiment
            /*
            BinaryReader internalReader = new BinaryReader(new MemoryStream(value));
            List<byte[]> bList = new List<byte[]>();
            int pointer = 0;
            do
            {
                int elementSize = internalReader.ReadInt32();
                byte[] element = internalReader.ReadBytes(elementSize);
                bList.Add(element);
                pointer += elementSize;
            } while (pointer < size);

            if (bList.Count > 1)
            {
                Debug.Print(bList.Count.ToString());
            }
           */
            RecalculateSize();
        }

        public int RecalculateSize()
        {
            int tmpSize = 0;
            if (value != null)
            {
                tmpSize += value.Length;
            }
            size = tmpSize;
            return size;
        }

        public bool ValidateValue(string input)
        {
            return false;
        }

        public bool SetValue(string input)
        {
            return false;
        }

        public string GetValueHex()
        {
            string hex = "";
            if(value != null) hex = value.ToHex();
            return hex;
        }

        public void CheckAndAddNames(GpkPackage package)
        {
   
        }
    }

}
