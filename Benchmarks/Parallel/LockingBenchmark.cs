using BenchmarkDotNet.Attributes;
using Bogus;

namespace Benchmarks.Parallel;

[MemoryDiagnoser]
public class LockingBenchmark {
	private readonly object lockObject = new();
	private Faker faker = null!;

	private int value;

	[GlobalSetup]
	public void Setup() {
		Randomizer.Seed = new Random(123);
		faker = new();
	}

	[Benchmark(Baseline = true)]
	public async Task<int> WithLocking() {
		var tasks = Enumerable.Range(1, 10).Select(_ => UpdateWithLocking()).ToArray();
		await Task.WhenAll(tasks);
		return value;
	}

	[Benchmark]
	public async Task<int> WithoutLocking() {
		var tasks = Enumerable.Range(1, 10).Select(_ => UpdateWithoutLocking()).ToArray();
		await Task.WhenAll(tasks);
		return value;
	}

	[Benchmark]
	public async Task<int> WithoutAnySync() {
		var tasks = Enumerable.Range(1, 10).Select(_ => Update()).ToArray();
		await Task.WhenAll(tasks);
		return value;
	}

	private Task UpdateWithLocking() {
		lock (lockObject) {
			value = faker.Random.Int();
		}
		return Task.CompletedTask;
	}

	private Task UpdateWithoutLocking() {
		Interlocked.Exchange(ref value, faker.Random.Int());
		return Task.CompletedTask;
	}

	private Task Update() {
		value = faker.Random.Int();
		return Task.CompletedTask;
	}
}