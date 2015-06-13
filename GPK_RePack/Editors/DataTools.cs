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
            destination.Properties.Clear();
            destination.Properties.AddRange(source.Properties.ToArray());
            destination.property_padding = source.property_padding;
            destination.property_size = source.property_size;
            destination.padding_unk = source.padding_unk;
        }

        public static void ReplaceData(GpkExport source, GpkExport destination)
        {
            destination.data_padding = source.data_padding;
            destination.data = source.data;
            destination.payload = source.payload;
        }

    }
}
