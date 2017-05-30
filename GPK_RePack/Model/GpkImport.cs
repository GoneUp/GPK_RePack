using System;
using System.Text;
using GPK_RePack.Model.Interfaces;

namespace GPK_RePack.Model
{
    [Serializable]
    class GpkImport : IGpkPart
    {
        public string UID;

        public string ClassPackage;
        public string ClassName;
        public int PackageRef;
        public string ObjectName;

        public int Unk;

        public override string ToString()
        {
            StringBuilder info = new StringBuilder();
            info.AppendLine("Type: Import");
            info.AppendLine("UID: " + UID);
            info.AppendLine("Object: " + ObjectName);
            info.AppendLine("ClassPackage: " + ClassPackage);
            info.AppendLine("Class: " + ClassName);
            return info.ToString();

        }

        public int GetSize()
        {
            return 28;

        }
    }
}
