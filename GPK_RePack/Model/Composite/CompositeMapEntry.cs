using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPK_RePack.Model.Composite
{
    public class CompositeMapEntry
    {
        /*
         * PkgMapper format
Entries are always seperated by |

UID -> Composite UID
S1UI_Chat2.Chat2,c7a706fb_6a349a6f_1d212.Chat2_dup|

        CompositePkgMapper

Files are seperated by !
File name like this !17d87899_3? -> 17d87899_3.gpk
Entries are always seperated by |

Composite UID, Unkown Object ID, Sub-GPK File offset, Sub-GPK File length
c7a706fb_6a349a6f_1d212.Chat2_dup,c7a706fb_6a349a6f_1d212,92291307,821218,|


         * 
         * */
        public string UID;
        public string CompositeUID;
        public string UnknownUID;
        public string SubGPKName;
        public int FileOffset;
        public int FileLength;

        public override string ToString()
        {
            return String.Format("UID: {0}, CompositeUID: {1}", UID, CompositeUID);
        }
    }


}
