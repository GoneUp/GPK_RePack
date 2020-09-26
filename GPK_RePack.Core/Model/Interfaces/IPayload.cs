using System.IO;

namespace GPK_RePack.Core.Model.Interfaces
{
    public interface IPayload
    {
        string GetClassIdent();
        void WriteData(BinaryWriter writer, GpkPackage package, GpkExport export);
        void ReadData(GpkPackage package, GpkExport export);
        int GetSize();
    }
}
