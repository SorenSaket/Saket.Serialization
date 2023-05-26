using BenchmarkDotNet.Attributes;
using Saket.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Saket.Engine.Benchmark.Serialization
{
    [BenchmarkCategory("Writing")]
    public class Benchmark_Writing
    {
        [Params(10000)]
        public int ItemCount { get; set; } = 10000;

        [Params(1)]
        public int InterationCount { get; set; } = 1;

        Stream stream;

        byte[] data;
        UInt64[] temp = new UInt64[16];

        Vector4 testData = new Vector4(123.5325f, 1025.1f, -2356f, -757f);

        [GlobalSetup]
        public void Setup()
        {
            data = new byte[ItemCount* 16];
            Random rnd = new Random();
            rnd.NextBytes(data);
            stream = new MemoryStream(data);
        }

        [BenchmarkCategory("StreamWriter")]
        [Benchmark]
        public void StreamWriter()
        {
            var writer = new Saket.Serialization.StreamWriterLE(stream);

            for (int i = 0; i < InterationCount; i++)
            {
                stream.Position = 0;
                for (int y = 0; y < ItemCount; y++)
                {
                    writer.SerializeVector4(ref testData);
                }
            }
        }

        [BenchmarkCategory("BinaryWriter")]
        [Benchmark(Baseline = true)]
        public void BinaryWriter()
        {
            var writer = new BinaryWriter(stream);
           
            for (int i = 0; i < InterationCount; i++)
            {
                stream.Position = 0;
                for (int y = 0; y < ItemCount; y++)
                {
                    writer.Write(testData.X);
                    writer.Write(testData.Y);
                    writer.Write(testData.Z);
                    writer.Write(testData.W);
                }
            }
        }

     
    }
}
