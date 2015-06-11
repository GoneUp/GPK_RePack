using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GPK_RePack.Classes;

namespace GPK_RePack.Editors
{
    class DataTools
    {

        public static void ReplaceProperties(GpkExport source, GpkExport destination)
        {
            source.Properties.Clear();
            source.Properties.AddRange(destination.Properties.ToArray());
            source.property_padding = destination.property_padding;
            source.property_size = destination.property_size;
            source.padding_unk = destination.padding_unk;
        }

        public static void ReplaceData(GpkExport source, GpkExport destination)
        {
            source.data_padding = destination.data_padding;
            source.data = destination.data;
            source.payload = destination.payload;
        }

    }
}
