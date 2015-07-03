using System;
using System.Diagnostics.Eventing.Reader;
using GPK_RePack.Classes.Interfaces;

namespace GPK_RePack.Classes
{
    class GpkString : IGpkPart
    {
        public string name;
        public long flags;
        public bool used = false;

        public GpkString() {}

        public GpkString(string tmpName, long tmpFlags, bool tmpUsed)
        {
            name = tmpName;
            flags = tmpFlags;
            used = tmpUsed;
        }

        public override string ToString() 
        {
            return name;
        }

        public int GetSize()
        {
            int size = 0;
            size += 4; //len
            size += name.Length + 1; //data
            size += 8; //flags
            return size;
        }
    }
 
}
