﻿using System;
using System.IO;
using System.Text;
using System.Windows.Documents;
using GPK_RePack.Model.Interfaces;
using GPK_RePack.Model.Prop;

namespace GPK_RePack.Model.Payload
{
    [Serializable]
    class ObjectReferencer : IPayload
    {
        public int count;
        public string[] referencedObjects;


        public void ReadData(GpkPackage package, GpkExport export)
        {
            IProperty arrayProp = export.Properties.Find(t => ((GpkBaseProperty)t).name == "ReferencedObjects");
            var data = ((GpkArrayProperty)arrayProp).value;

            BinaryReader reader = new BinaryReader(new MemoryStream(data));

            count = reader.ReadInt32();
            referencedObjects = new string[count];
            for (int i = 0; i < count; i++)
            {
                int objIndex = reader.ReadInt32();
                referencedObjects[i] = package.GetObjectName(objIndex);
            }

        }

        public void WriteData(BinaryWriter writer, GpkPackage package, GpkExport export)
        {
            /*
             * useless code atm, payload is written after props, so rewrite here does not make any sense
             * 
            GpkArrayProperty arrayProp = (GpkArrayProperty)export.Properties.Find(t => ((GpkBaseProperty)t).name == "ReferencedObjects");
            var data = new byte[count * 4 + 4];

            BinaryWriter writerProp = new BinaryWriter(new MemoryStream(data));
            writerProp.Write(count);
            for (int i = 0; i < count; i++)
            {
                writerProp.Write(Convert.ToInt32(package.GetObjectIndex(referencedObjects[i])));
            }

            arrayProp.value = data;
            arrayProp.RecalculateSize();
            */
            
        }

        public int GetSize()
        {
            return 0;
        }

        public string GetClassIdent()
        {
            return "Core.ObjectReferencer";
        }

        public override string ToString()
        {
            StringBuilder info = new StringBuilder();
            info.AppendLine("Type: " + GetClassIdent());
            info.AppendLine("References to: ");
            foreach (var obj in referencedObjects)
            {
                info.Append(obj + ", ");
            }
            return info.ToString();
        }

    }
}
