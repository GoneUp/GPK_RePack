using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GPK_RePack.Parser;

namespace GPK_RePack.Classes.Interfaces
{
    interface IProperty
    {
        void WriteData(BinaryWriter writer, GpkPackage package);
        void ReadData(BinaryReader reader, GpkPackage package);
        int RecalculateSize();

        bool ValidateValue(string input);
        bool SetValue(string input);
    }
}
