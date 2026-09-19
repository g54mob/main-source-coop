using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Photon.Realtime
{
	public class AsyncOperationHandler : IDisposable
	{
		private TaskCompletionSource<short> _result;

		private CancellationTokenSource _cancellation;

		public Task<short> Task => _result.Task;

		public TaskCompletionSource<short> CompletionSource => _result;

		public CancellationToken Token
		{
			get
			{
				if (_cancellation != null)
				{
					return _cancellation.Token;
				}
				return CancellationToken.None;
			}
		}

		public bool IsCancellationRequested
		{
			get
			{
				if (_cancellation != null)
				{
					return _cancellation.IsCancellationRequested;
				}
				return false;
			}
		}

		public ConcurrentQueue<IDisposable> Disposables { get; private set; }

		public string Name { get; set; }

		public AsyncOperationHandler(float operationTimeoutSec)
		{
			_result = new TaskCompletionSource<short>();
			_cancellation = new CancellationTokenSource(TimeSpan.FromSeconds(operationTimeoutSec));
			_cancellation.Token.Register(delegate
			{
				SetException(new OperationTimeoutException("Operation timed out " + Name));
			});
			Disposables = new ConcurrentQueue<IDisposable>();
		}

		public AsyncOperationHandler()
		{
			_result = new TaskCompletionSource<short>();
			Disposables = new ConcurrentQueue<IDisposable>();
		}

		public void SetResult(short result)
		{
			if (_result.TrySetResult(result))
			{
				if (_cancellation != null && !_cancellation.IsCancellationRequested)
				{
					_cancellation.Cancel();
				}
				Dispose();
			}
		}

		public void SetException(Exception e)
		{
			if (_result.TrySetException(e))
			{
				if (_cancellation != null && !_cancellation.IsCancellationRequested)
				{
					_cancellation.Cancel();
				}
				Dispose();
			}
		}

		public void Dispose()
		{
			_cancellation?.Dispose();
			_cancellation = null;
			IDisposable result;
			while (Disposables.TryDequeue(out result))
			{
				result.Dispose();
			}
		}
	}
}
