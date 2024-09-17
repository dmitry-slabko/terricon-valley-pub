namespace Benchmarks.Samples;

public class DisposableSample : IDisposable {
	protected IDisposable? disposable;

	public DisposableSample(OtherDisposable disposable) {
		this.disposable = disposable;
	}

	protected virtual void Dispose(bool disposing) {
		if (disposing) {
			var instance = Interlocked.Exchange(ref disposable, null);
			instance?.Dispose();
		}
	}

	public void Dispose() {
		Dispose(true);
	}
}

public class OtherDisposable : IDisposable {
	public void Dispose() { }
}
