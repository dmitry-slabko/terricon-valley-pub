using BenchmarkDotNet.Attributes;
using Bogus;

namespace Benchmarks.Algorithms;

[MemoryDiagnoser]
public class AlgorithmBenchmark {
	private List<int> list;
	private List<int> matchList;
	private HashSet<int> matchSet;

	[Benchmark(Baseline = true)]
	public bool LoopInsideLoop() => list.Any(x => matchList.Contains(x));

	[Benchmark]
	public bool SingleLoop() => list.Any(x => matchSet.Contains(x));

	[Benchmark]
	public bool Optimized() => matchSet.Overlaps(list);

	[GlobalSetup]
	public void Setup() {
		Randomizer.Seed = new Random(42);
		var faker = new Faker();

		list = new List<int>(Enumerable.Range(1, 1000).Select(_ => faker.Random.Int()));
		matchList = new List<int>(Enumerable.Range(1, 1000).Select(_ => faker.Random.Int()));
		matchSet = new HashSet<int>(matchList);
	}
}