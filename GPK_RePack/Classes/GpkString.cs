using GPK_RePack.Classes.Interfaces;

namespace GPK_RePack.Classes
{
    class GpkString : IGpkPart
    {
        public string name;
        public long flags;

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
