using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPK_RePack.Class
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
