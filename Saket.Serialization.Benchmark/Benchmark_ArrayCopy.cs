using BenchmarkDotNet.Attributes;
using Saket.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace Saket.Engine.Benchmark.Serialization
{
    [BenchmarkCategory("ArrayCopy")]
    public class Benchmark_ArrayCopy
    {
        [Params(10000)]
        public int ItemCount { get; set; } = 4096;

        [Params(1)]
        public int InterationCount { get; set; } = 1;

        byte[] dataSource;

        byte[] dataDestination;


        [GlobalSetup]
        public void Setup()
        {
            dataSource = new byte[ItemCount * 16];
            dataDestination = new byte[ItemCount * 16];

            //Random rnd = new Random();
            //rnd.NextBytes(dataSource);
        }

        [BenchmarkCategory("Array.Copy()")]
        [Benchmark(Baseline = true)]
        public void BuildIn()
        {
            for (int i = 0; i < InterationCount; i++)
            {
                Array.Copy(dataSource, dataDestination, ItemCount);
            }
        }

        [BenchmarkCategory("Custom")]
        [Benchmark]
        public void Custom()
        {
            for (int i = 0; i < InterationCount; i++)
            {
                unsafe
                {
                    fixed(byte* source = dataSource)
                    {
                        fixed(byte* dest = dataDestination)
                        {
                            for (int x = 0; x < ItemCount / 32; x++)
                            {
                                Avx.Store(&dest[x * 32], Avx.LoadVector256(&source[x * 32]));
                            }
                        }
                    }
                }
             
            }
        }

     
    }
}
