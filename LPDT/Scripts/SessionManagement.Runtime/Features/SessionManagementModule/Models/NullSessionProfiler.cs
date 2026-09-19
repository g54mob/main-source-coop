using System;
using System.Threading.Tasks;

namespace Features.SessionManagementModule.Models
{
	public sealed class NullSessionProfiler : ISessionProfiler
	{
		private sealed class NoOpScope : IAsyncDisposable
		{
			public ValueTask DisposeAsync()
			{
				return default(ValueTask);
			}
		}

		private static readonly IAsyncDisposable _scope = new NoOpScope();

		public IAsyncDisposable Sample(SessionPhase phase)
		{
			return _scope;
		}
	}
}
