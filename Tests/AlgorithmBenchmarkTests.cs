using Benchmarks.Algorithms;
using FluentAssertions;

namespace Tests;

public class AlgorithmBenchmarkTests {
	[Fact]
	public void AlgorithmBenchmarkTest() {
		var sut = new AlgorithmBenchmark();
		sut.Setup();

		bool result = sut.LoopInsideLoop();
		sut.Optimized().Should().Be(result);
		sut.SingleLoop().Should().Be(result);
	}
}