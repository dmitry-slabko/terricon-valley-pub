using BenchmarkDotNet.Attributes;

namespace Benchmarks.Memory;

[MemoryDiagnoser]
public class CollectionSizeBenchmark {
	[Params(10, 100, 1000)]
	public int Size { get; set; }

	[Benchmark(Baseline = true)]
	public List<Guid> DefaultCollectionSize() {
		var list = new List<Guid>();
		for (int i = 0; i < Size; i++) {
			list.Add(Guid.NewGuid());
		}
		return list;
	}

	[Benchmark]
	public List<Guid> PreallocatedCollection() {
		var list = new List<Guid>(Size);
		for (int i = 0; i < Size; i++) {
			list.Add(Guid.NewGuid());
		}
		return list;
	}
}