using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saket.Engine.Benchmark.Serialization
{
    [BenchmarkCategory("Reading")]
    public class Benchmark_Serialization_Read_UInt64
    {
        [Params(8192)]
        public int ItemCount { get; set; }

        [Params(1)]
        public int InterationCount { get; set; }

        Stream stream;

        byte[] data;
        UInt64[] temp = new UInt64[16];

        [GlobalSetup]
        public void Setup()
        {
            data = new byte[ItemCount*8];
            Random rnd = new Random();
            rnd.NextBytes(data);
            stream = new MemoryStream(data);
        }

        
        [BenchmarkCategory("BinaryReader")]
        [Benchmark(Baseline = true)]
        public void BinaryReader()
        {
            var reader = new BinaryReader(stream);

            for (int i = 0; i < InterationCount; i++)
            {
                stream.Position = 0;
                for (int y = 0; y < ItemCount; y++)
                {
                    temp[0] = reader.ReadUInt64();
                }
            }
        }
        

    }
}
