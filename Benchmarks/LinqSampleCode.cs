using System.Security;
using System.Text.Json;

namespace Benchmarks;

public class LinqSampleCode {
	public IEnumerable<int> SomeNumbers {
		get {
			var rnd = new Random();
			yield return rnd.Next();
			yield return rnd.Next();
			yield return rnd.Next();
		}
	}

	public void DoSomething<T>(IEnumerable<string> parameters) where T : class {
		var results = parameters.Select(GetDataWithSideEffect<T>);

		int count = results.Count();
		T?[] array = new T?[count];
		int index = 0;
		foreach (var result in results) {
			if (result == default(T)) {
				array[index++] = default;
			} else {
				array[index++] = PostProcess(result);
			}
		}
	}

	private T? GetDataWithSideEffect<T>(string param) {
		// execute the side effect
		return default(T);
	}

	private T PostProcess<T>(T data) {
		var x = DoSomethingSync();
		// do something
		return data;
	}

	Guid DoSomethingSync() {
		var task = DoSomethingAsync();
		return task.GetAwaiter().GetResult();
	}

	Task<Guid> DoSomethingAsync() {
		Sample(() => true, CancellationToken.None);
		return Task.FromResult(Guid.NewGuid());
	}

	async Task Sample(Func<bool> loopCondition, CancellationToken ct) {
		while (loopCondition()) {
			// do something
			if (ct.IsCancellationRequested) {
				break;
			}
		}

		try {
			await AnotherAction(ct);
			await SendRequests(ct);
			await SendRequestsImproved(ct);
		} catch (OperationCanceledException) {
			// some cleanup
			throw;
		}
	}

	Task AnotherAction(CancellationToken ct) {
		// do something
		ct.ThrowIfCancellationRequested();
		// do something more
		return Task.CompletedTask;
	}
}