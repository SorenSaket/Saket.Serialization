using BenchmarkDotNet.Attributes;
using Saket.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saket.Engine.Benchmark.Serialization
{
    [BenchmarkCategory("Ser")]
    public class Benchmark_Serialization
    {
        [Params(512)]
        public int ItemCount { get; set; }

        [Params(4)]
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
        /*
        [BenchmarkCategory("Steam Reader ser")]
        [Benchmark]
        public void ASaketSer()
        {
            var reader = new Saket.Serialization.StreamReaderLE(stream);

            for (int i = 0; i < InterationCount; i++)
            {
                stream.Position = 0;
                reader.LoadBytes(ItemCount * 8);
                for (int y = 0; y < ItemCount; y++)
                {
                    reader.Position = 0;
                    reader.SerializeUInt64(ref temp[0]);
                }
            }
        }*/
    }
}
