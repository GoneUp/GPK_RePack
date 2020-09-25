using System;
using System.Text;
using GPK_RePack.Core.Model.Interfaces;

namespace GPK_RePack.Core.Model
{
    [Serializable]
    public class GpkImport : IGpkPart
    {
        public string UID;

        public string ClassPackage;
        public string ClassName;
        public int OwnerIndex;
        public string OwnerObject;
        public string ObjectName;

        public void CheckNamePresence(GpkPackage package)
        {
            package.GetStringIndex(ClassPackage);
            package.GetStringIndex(ClassName);
            package.GetStringIndex(ObjectName);
        }


        public override string ToString()
        {
            StringBuilder info = new StringBuilder();
            info.AppendLine("Type: Import");
            info.AppendLine("UID: " + UID);
            info.AppendLine("Object: " + ObjectName);
            info.AppendLine("ClassPackage: " + ClassPackage);
            info.AppendLine("Class: " + ClassName);
            info.AppendLine("OwnerObject: " + OwnerObject);
            return info.ToString();

        }

        public string ToCompactString()
        {
            return String.Format("I;{0};{1};{2};{3}", UID, ObjectName, ClassPackage, ClassName);
        }

        public int GetSize()
        {
            return 28;
        }
    }
}
