using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPK_RePack
{
    class Constants
    {
        public static string UPDATE_URL = "https://raw.githubusercontent.com/GoneUp/GPK_RePack/master/versioncode";
        public static int APP_VERSION = 16;

        public static int DEFAULT_BLOCKSIZE = 131072;
        public static int DEFAULT_BLOCKCOUNT = 256;
        public static int DEFAULT_CHUNKSIZE = DEFAULT_BLOCKSIZE * DEFAULT_BLOCKCOUNT;

        public static int DEFAULT_SIGNATURE = -1641380927; //0x9e2a83c1;
    }
}
