using System;

namespace GPK_RePack.Model.Payload
{
    [Serializable]
    class SoundCueObject
    {
        public string objectName;
        public int Unk2;
        public int Unk3;

        public override string ToString()
        {
            return string.Format("Name {0}, Unk2 {1}, Unk3 {2}", objectName, Unk2, Unk3);
        }
    }
}
