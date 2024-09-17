// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using Benchmarks;
using Benchmarks.Algorithms;
using Benchmarks.Logging;
using Benchmarks.Memory;
using Benchmarks.Parallel;
using Benchmarks.TypeSystem;

// BenchmarkRunner.Run<StringsBenchmark>();
// BenchmarkRunner.Run<LoggingBenchmark>();
// BenchmarkRunner.Run<CollectionSizeBenchmark>();
// BenchmarkRunner.Run<ArrayPoolBenchmark>();
// BenchmarkRunner.Run<AlgorithmBenchmark>();
// BenchmarkRunner.Run<TypesBenchmark>();
BenchmarkRunner.Run<LockingBenchmark>();

// BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);