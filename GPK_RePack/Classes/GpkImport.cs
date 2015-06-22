using System.Text;
using GPK_RePack.Classes.Interfaces;

namespace GPK_RePack.Classes
{
    class GpkImport : IGpkPart
    {
        public string UID;

        public string ClassPackage;
        public string Class;
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
            info.AppendLine("Class: " + Class);
            return info.ToString();

        }

        public int GetSize()
        {
            return 28;

        }
    }
}
