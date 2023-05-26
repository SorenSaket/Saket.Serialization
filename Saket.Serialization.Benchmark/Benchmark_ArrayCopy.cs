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
        [Params(8192)]
        public int ItemCount { get; set; }

        [Params(1)]
        public int InterationCount { get; set; }

        byte[] dataSource;

        byte[] dataDestination;


        [GlobalSetup]
        public void Setup()
        {
            dataSource = new byte[ItemCount];
            dataDestination = new byte[ItemCount];
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

        [BenchmarkCategory("AVX")]
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
                                Avx.Store(&dest[x * 32], Avx2.LoadVector256(&source[x * 32]));
                            }
                        }
                    }
                }
            }
        }

     
    }
}
