using System.Text;
using BenchmarkDotNet.Attributes;
using Bogus;

namespace Benchmarks.Memory;

[MemoryDiagnoser]
public class StringsBenchmark {
	private Faker faker = null!;
	private string original = null!, text = null!;
	private char c1, c2, c3, c4;

	[Params(0, 1, 4)]
	public int MatchCount { get; set; }

	[GlobalSetup]
	public void Setup() {
		Randomizer.Seed = new Random(12345678);
		faker = new Faker();

		original = faker.Random.String(100, 'A', 'z');
		text = faker.Random.String(32, 'A', 'z');

		c1 = c2 = c3 = c4 = '0';
		if (MatchCount > 0) {
			int maxIndex = MatchCount - 2;
			c1 = original[0];
			int index = 0;
			Func<char, bool> comparer = x => x != c1;
			foreach (char c in original) {
				if (comparer(c)) {
					switch (index) {
						case 0:
							c2 = c;
							comparer = x => x != c1 && x != c2;

							break;

						case 1:
							c3 = c;
							comparer = x => x != c1 && x != c2 && x != c3;

							break;

						case 2:
							c4 = c;

							break;
					}

					if (++index > maxIndex) {
						break;
					}
				}
			}
		}
	}

	[Benchmark(Baseline = true)]
	public string OperationsOnStrings() => ReplaceAndAppend(original, text);

	[Benchmark]
	public string UsingStringBuilder() => ReplaceAndAppendWithStringBuilder(original, text);

	[Benchmark]
	public string PreallocatedStringBuilder() => ReplaceAndAppendWithPreallocatedStringBuilder(original, text);

	[Benchmark]
	public string UsingStackAllocatedSpan() => ReplaceAndAppendWithSpans(original, text);

	public string ReplaceAndAppendWithStringBuilder(string source, string more) =>
		new StringBuilder()
			.Append(source)
			.Replace(c1, '0')
			.Replace(c2, '1')
			.Replace(c3, '2')
			.Replace(c4, '3')
			.Append(more)
			.ToString();

	public string ReplaceAndAppend(string source, string more) =>
		source
			.Replace(c1, '0')
			.Replace(c2, '1')
			.Replace(c3, '2')
			.Replace(c4, '3') +
		more;

	public string ReplaceAndAppendWithPreallocatedStringBuilder(string source, string more) =>
		new StringBuilder(source.Length + more.Length)
			.Append(source)
			.Replace(c1, '0')
			.Replace(c2, '1')
			.Replace(c3, '2')
			.Replace(c4, '3')
			.Append(more)
			.ToString();

	public string ReplaceAndAppendWithSpans(ReadOnlySpan<char> source, ReadOnlySpan<char> more) {
		Span<char> output = stackalloc char[source.Length + more.Length];
		source.CopyTo(output);

		if (source.IndexOfAny(c1, c2) >= 0 || source.IndexOfAny(c3, c4) >= 0) {
			var slice = output.Slice(0, source.Length);
			slice.Replace(c1, '0');
			slice.Replace(c2, '1');
			slice.Replace(c3, '2');
			slice.Replace(c4, '3');
		}

		more.CopyTo(output.Slice(source.Length));

		return new string(output);
	}
}