using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPK_RePack.Model.Payload
{

    public enum CompressionTypesPackage : uint
    {

        ZLIB = 0x00000001,
        LZO = 0x00000002,
        LZX = 0x00000004,

    }

}
/*
 * 
 * #define COMPRESS_ZLIB		1
#define COMPRESS_LZO		2
#define COMPRESS_LZX		4
*/