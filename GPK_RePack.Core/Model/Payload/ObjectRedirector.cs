using System;
using System.IO;
using System.Text;
using GPK_RePack.Core.Model.Interfaces;

namespace GPK_RePack.Core.Model.Payload
{
    [Serializable]
    class ObjectRedirector : IPayload
    {
        public string ObjectName;
        public int ObjectIndex;

     
        public void ReadData( GpkPackage package, GpkExport export)
        {
            BinaryReader reader = new BinaryReader(new MemoryStream(export.Data));

            ObjectIndex = reader.ReadInt32();
            ObjectName = package.GetObjectName(ObjectIndex);
        }

        public void WriteData(BinaryWriter writer, GpkPackage package, GpkExport export)
        {
            writer.Write(Convert.ToInt32(package.GetObjectIndex(ObjectName)));
        }

        public int GetSize()
        {
            return 4;
        }

        public string GetClassIdent()
        {
            return "Core.ObjectRedirector";
        }

        public override string ToString()
        {
            StringBuilder info = new StringBuilder();
            info.AppendLine("Type: " + GetClassIdent());
            info.AppendLine("Redirects to: " + ObjectName);
            return info.ToString();
        }

    }
}
