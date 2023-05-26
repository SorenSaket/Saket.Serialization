using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using Saket.Engine.Benchmark.Serialization;
using System.Diagnostics;
using System.Numerics;


public static class Program
{
    [STAThread]
    static void Main()
    {
        var config = ManualConfig.Create(DefaultConfig.Instance).WithOptions(ConfigOptions.DisableOptimizationsValidator);

        BenchmarkRunner.Run(typeof(Benchmark_Stream_Span), config);
        Console.ReadKey();
    }
}

