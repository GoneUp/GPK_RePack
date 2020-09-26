namespace GPK_RePack.Core.Model
{

    public enum GpkPackageFlags : uint
    {

        AllowDownload = 0x00000001,
        ClientOptional = 0x00000002,
        ServerSideOnly = 0x00000004,
        BrokenLinks = 0x00000008,

        Unsecure = 0x00000010,
        Encrypted = 0x00000020,
        Need = 0x00008000,
        Map = 0x00020000,
        Script = 0x00200000,
        Debug = 0x00400000,
        Compressed = 0x02000000,
        FullyCompressed = 0x04000000,
        NoExportsData = 0x20000000,
        Stripped = 0x40000000,
        Protected = 0x80000000

    }

}
