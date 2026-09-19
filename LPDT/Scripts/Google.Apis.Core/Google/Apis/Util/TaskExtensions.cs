using System.Threading;
using System.Threading.Tasks;

namespace Google.Apis.Util
{
	internal static class TaskExtensions
	{
		internal static Task<T> WithCancellationToken<T>(this Task<T> task, CancellationToken cancellationToken)
		{
			if (!cancellationToken.CanBeCanceled)
			{
				return task;
			}
			return ImplAsync();
			async Task<T> ImplAsync()
			{
				TaskCompletionSource<T> cts = new TaskCompletionSource<T>();
				using (cancellationToken.Register(delegate
				{
					cts.TrySetCanceled();
				}))
				{
					return await (await Task.WhenAny<T>(task, cts.Task).ConfigureAwait(continueOnCapturedContext: false)).ConfigureAwait(continueOnCapturedContext: false);
				}
			}
		}
	}
}
