using System.IO;

namespace GPK_RePack.Model.Interfaces
{
    interface IProperty
    {
        void WriteData(BinaryWriter writer, GpkPackage package);
        void ReadData(BinaryReader reader, GpkPackage package);
        int RecalculateSize();

        void CheckAndAddNames(GpkPackage package);
        bool ValidateValue(string input);
        bool SetValue(string input);
    }
}
