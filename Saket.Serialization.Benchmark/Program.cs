using BenchmarkDotNet.Running;
using Saket.Engine.Benchmark.Serialization;
using System.Diagnostics;
using System.Numerics;


public static class Program
{
    [STAThread]
    static void Main()
    {
       
        BenchmarkRunner.Run(typeof(Benchmark_ArrayCopy));
        Console.ReadKey();
    }
}

