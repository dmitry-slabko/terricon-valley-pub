using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnostics.Windows.Configs;
using Bogus;

namespace Benchmarks.TypeSystem;

[MemoryDiagnoser]
public class TypesBenchmark {
	private int[] array = null!;
	private List<int> list = null!;

	[GlobalSetup]
	public void Setup() {
		Randomizer.Seed = new Random(12345);
		var faker = new Faker();
		array = Enumerable.Range(0, 1000).Select(_ => faker.Random.Int()).ToArray();
		list = Enumerable.Range(0, 1000).Select(_ => faker.Random.Int()).ToList();
	}

	[Benchmark(Baseline = true)]
	public int BenchmarkAsIList() => AsIList(array) + AsIList(list);

	[Benchmark]
	public int BenchmarkAsConcreteTypes() => AsArray(array) + AsList(list);

	private static int AsIList(IList<int> source) {
		int result = 0;
		for (int i = 0; i < source.Count; i++) {
			result += source[i] + (source[i] >> 2);
		}
		return result;
	}

	private static int AsList(List<int> source) {
		int result = 0;
		for (int i = 0; i < source.Count; i++) {
			result += source[i] + (source[i] >> 2);
		}
		return result;
	}

	private static int AsArray(int[] source) {
		int result = 0;
		for (int i = 0; i < source.Length; i++) {
			result += source[i] + (source[i] >> 2);
		}
		return result;
	}
}