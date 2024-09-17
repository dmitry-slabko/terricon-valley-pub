using Benchmarks.Memory;
using FluentAssertions;

namespace Tests;

public class StringBenchmarkTests {
	[Fact]
	public void AllMethodsProduceEqualResults() {
		var s1 = Guid.NewGuid().ToString();
		var s2 = Guid.NewGuid().ToString();

		var sut = new StringsBenchmark();
		sut.Setup();
		var r1 = sut.ReplaceAndAppend(s1, s2);
		var r2 = sut.ReplaceAndAppendWithStringBuilder(s1, s2);
		var r3 = sut.ReplaceAndAppendWithPreallocatedStringBuilder(s1, s2);
		var r4 = sut.ReplaceAndAppendWithSpans(s1, s2);

		r1.Should().Be(r2);
		r1.Should().Be(r3);
		r1.Should().Be(r4);
	}
}