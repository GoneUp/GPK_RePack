using System;

namespace GPK_RePack.Core.Model.Compression
{
    [Serializable]
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