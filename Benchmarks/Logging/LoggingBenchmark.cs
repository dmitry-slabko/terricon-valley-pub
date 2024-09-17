using BenchmarkDotNet.Attributes;
using Bogus;
using Microsoft.Extensions.Logging;

namespace Benchmarks.Logging;

[MemoryDiagnoser]
public class LoggingBenchmark {
	record SampleData {
		public required Guid Id { get; init; }
		public required double Value { get; init; }
		public required string Name { get; init; } = null!;
		public required int Index { get; init; }
		public required DateTimeOffset Date { get; init; }
	}

	private ILoggerFactory loggerFactory = null!;
	private ILogger logger = null!;
	private Faker faker = null!;
	private Faker<SampleData> dataFaker = null!;
	private List<Guid> diagnosticData = null!;
	private SampleData sampleData = null!;

	[GlobalSetup]
	public void Setup() {
		loggerFactory = LoggerFactory.Create(builder => {
			builder.AddDebug();
		});
		logger = loggerFactory.CreateLogger<LoggingBenchmark>();
		Randomizer.Seed = new Random(1234);
		faker = new Faker();
		dataFaker = new Faker<SampleData>()
			.StrictMode(true)
			.RuleFor(x => x.Id, f => Guid.NewGuid())
			.RuleFor(x => x.Value, f => f.Random.Double())
			.RuleFor(x => x.Name, f => f.Name.FirstName())
			.RuleFor(x => x.Index, f => f.Random.Int())
			.RuleFor(x => x.Date, f => f.Date.Future());
		diagnosticData = Enumerable.Range(1, 3).Select(_ => faker.Random.Guid()).ToList();
		sampleData = dataFaker.Generate();
	}

	[GlobalCleanup]
	public void Cleanup() {
		loggerFactory.Dispose();
	}

	[Benchmark(Baseline = true)]
	public void StringTypedLogging() {
		var operationId = faker.Random.Guid();
		logger.LogInformation($"Operation {operationId} started on {DateTimeOffset.UtcNow}");
		logger.LogInformation($"The operation data is {sampleData}");
		for (var index = 0; index < diagnosticData.Count; index++) {
			var item = diagnosticData[index];
			logger.LogInformation($"Diagnostic Data@{index} {item}");
		}
		logger.LogInformation($"Operation {operationId} completed on {DateTimeOffset.UtcNow}");
	}

	[Benchmark]
	public void StructuredLogging() {
		var operationId = faker.Random.Guid();
		logger.LogInformation("Operation {operationId} started on {startTime}", operationId, DateTimeOffset.UtcNow);
		logger.LogInformation("The operation data is {data}", sampleData);
		for (var index = 0; index < diagnosticData.Count; index++) {
			var item = diagnosticData[index];
			logger.LogInformation("Diagnostic Data@{index} {item}", index, item);
		}
		logger.LogInformation("Operation {operationId} completed on {startTime}", operationId, DateTimeOffset.UtcNow);
	}
}