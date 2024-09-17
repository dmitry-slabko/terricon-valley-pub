using System.Buffers;
using BenchmarkDotNet.Attributes;

namespace Benchmarks.Memory;

[MemoryDiagnoser]
public class ArrayPoolBenchmark {
	private const int Size = 1024 * 1024 * 2;
	private static readonly ArrayPool<byte> arrayPool = 
		ArrayPool<byte>.Create(Size * 10, 16);

	[Benchmark(Baseline = true)]
	public DummyConsumer UseAllocations() {
		var array = new byte[Size];
		var consumer = new DummyConsumer();
		consumer.ConsumeMemory(array);
		return consumer;
	}

	[Benchmark]
	public DummyConsumer UseArrayPool() {
		var consumer = new DummyConsumer();
		var array = arrayPool.Rent(Size);
		try {
			consumer.ConsumeMemory(array.AsSpan(0, Size));
		} finally {
			arrayPool.Return(array);
		}
		return consumer;
	}
}