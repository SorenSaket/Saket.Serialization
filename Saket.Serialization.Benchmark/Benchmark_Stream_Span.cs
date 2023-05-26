using BenchmarkDotNet.Attributes;
using Dia2Lib;
using Saket.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saket.Engine.Benchmark.Serialization
{
    [BenchmarkCategory("Stream Span")]
    public class Benchmark_Stream_Span
    {
        [Params(8192)]
        public int ItemCount { get; set; }

        [Params(4)]
        public int InterationCount { get; set; }

        Stream stream;

        byte[] data;
        UInt64[] temp = new UInt64[16];

        [GlobalSetup]
        public void Setup()
        {
            data = new byte[ItemCount];

            Random rnd = new Random();
            rnd.NextBytes(data);

            stream = new MemoryStream(data);
        }

        
        [BenchmarkCategory("BinaryReader")]
        [Benchmark(Baseline = true)]
        public void ReadExactly()
        {
            for (int i = 0; i < InterationCount; i++)
            {
                stream.Position = 0;
                stream.ReadExactly(data, 0, ItemCount);
            }
        }
        
        [BenchmarkCategory("Steam Reader ser")]
        [Benchmark]
        public void ReadExactlySpan()
        {
            for (int i = 0; i < InterationCount; i++)
            {
                stream.Position = 0;
                stream.ReadExactly(data.AsSpan(0, ItemCount));
            }
        }
    }
}
