using System;

namespace WaveFormRendererLib
{
    public class RmsPeakProvider : PeakProvider
    {
        private readonly int blockSize;

        public RmsPeakProvider(int blockSize)
        {
            this.blockSize = blockSize;
        }

        public override PeakInfo GetNextPeak()
        {
            var samplesRead = Provider.Read(ReadBuffer, 0, ReadBuffer.Length % 2 == 0 ? ReadBuffer.Length : ReadBuffer.Length-1);

            var max = 0.0f;
            for (int x = 0; x < samplesRead; x += blockSize)
            {
                double total = 0.0;
                for (int y = 0; y < blockSize && x + y < samplesRead; y++)
                {
                    total += ReadBuffer[x + y] * ReadBuffer[x + y];
                }
                var rms = (float) Math.Sqrt(total/blockSize);

                max = Math.Max(max, rms);
            }

            return new PeakInfo(0 -max, max);
        }
    }
}