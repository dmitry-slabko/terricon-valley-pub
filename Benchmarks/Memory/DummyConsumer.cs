namespace Benchmarks.Memory;

public class DummyConsumer {
	public int ConsumptionResult { get; private set; }

	public void ConsumeMemory(Span<byte> memory) {
		ConsumptionResult = memory.Length + memory[memory.Length / 2];
	}
}