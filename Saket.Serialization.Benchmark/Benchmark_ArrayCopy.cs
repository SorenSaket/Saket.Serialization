using BenchmarkDotNet.Attributes;
using Saket.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace Saket.Engine.Benchmark.Serialization;

// My pc doesn't support avx512
// All built in functions seem to be the ceiling of copy speeds. They have a lot of optimimzations.

//|      Method | ItemCount | InterationCount |     Mean |   Error |  StdDev | Ratio | RatioSD |
//|------------ |---------- |---------------- |---------:|--------:|--------:|------:|--------:|
//|      AVX256 |      8192 |               1 | 176.5 ns | 1.63 ns | 1.36 ns |  1.15 |    0.02 |
//|      AVX512 |      8192 |               1 |       NA |      NA |      NA |     ? |       ? |
//|   ArrayCopy |      8192 |               1 | 153.4 ns | 2.29 ns | 2.03 ns |  1.00 |    0.00 |
//|     MemCopy |      8192 |               1 | 154.5 ns | 1.68 ns | 1.32 ns |  1.01 |    0.02 |
//| MarshalCopy |      8192 |               1 | 155.7 ns | 1.68 ns | 1.49 ns |  1.01 |    0.01 |


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
    public void ArrayCopy()
    {
        for (int i = 0; i < InterationCount; i++)
        {
            Array.Copy(dataSource, dataDestination, ItemCount);
        }
    }
    [BenchmarkCategory("Buffer.MemoryCopy()")]
    [Benchmark()]
    public void MemCopy()
    {
        for (int i = 0; i < InterationCount; i++)
        {
            unsafe
            {
                fixed (void* source = dataSource)
                fixed (void* dest = dataDestination)
                {
                    Buffer.MemoryCopy(source, dest, ItemCount, ItemCount);
                }
            }
        }
    }
    [BenchmarkCategory("Marshal.Copy()")]
    [Benchmark()]
    public void MarshalCopy()
    {
        for (int i = 0; i < InterationCount; i++)
        {
            unsafe
            {
                fixed (void* source = dataSource)
                fixed (void* dest = dataDestination)
                {
                    Marshal.Copy(new IntPtr(source), dataDestination, 0, ItemCount);
                }
            }
        }
    }

    [BenchmarkCategory("AVX256")]
    [Benchmark]
    public void AVX256()
    {
        for (int i = 0; i < InterationCount; i++)
        {
            unsafe
            {
                fixed(byte* source = dataSource)
                fixed (byte* dest = dataDestination)
                {
                    for (int x = 0; x < ItemCount; x+=32)
                    {
                        Avx.Store(&dest[x], Avx2.LoadVector256(&source[x]));
                    }
                    
                }
            }
        }
    }

   

    [BenchmarkCategory("AVX512")]
    [Benchmark]
    public void AVX512()
    {
        for (int i = 0; i < InterationCount; i++)
        {
            unsafe
            {
                fixed (byte* source = dataSource)
                fixed (byte* dest = dataDestination)
                {
                    for (int x = 0; x < ItemCount; x+=64)
                    {
                        Avx512F.Store(&dest[x], Avx512F.LoadVector512(&source[x]));
                    }
                }
            }
        }
    }

}
