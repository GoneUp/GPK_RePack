using System.Text;

namespace GPK_RePack.Classes
{
    class GpkImport
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
    }
}
