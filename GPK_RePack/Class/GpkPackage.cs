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

        public Dictionary<int, GpkString> NameList;
        public List<GpkImport> ImportList;
        public List<GpkExport> ExportList;
        public GpkDependList DependList;

        public GpkPackage()
        {
            Header = new GpkHeader();

            NameList = new Dictionary<int, GpkString>();
            ImportList = new List<GpkImport>();
            ExportList = new List<GpkExport>();
        }
    }
}
