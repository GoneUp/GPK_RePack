using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPK_RePack.Class
{
    class GpkPackage
    {
        public GpkHeader Header;

        public List<GpkString> NameList;
        public List<GpkImport> ImportList;
        public GpkExportList ExportList;
        public GpkDependList DependList;

        public GpkPackage()
        {
            NameList = new List<GpkString>();
            ImportList = new List<GpkImport>();
        }
    }
}
