using System.Threading;
using System.Threading.Tasks;

namespace Ami.Extension
{
	public static class AsyncTaskExtension
	{
		public const float MillisecondInSeconds = 0.001f;

		public static async Task DelaySeconds(float seconds, CancellationToken cancellationToken = default(CancellationToken))
		{
			await Delay(TimeExtension.SecToMs(seconds), cancellationToken);
		}

		public static async Task DelaySeconds(double seconds, CancellationToken cancellationToken = default(CancellationToken))
		{
			await Delay(TimeExtension.SecToMs(seconds), cancellationToken);
		}

		public static async Task Delay(int milliseconds, CancellationToken cancellationToken = default(CancellationToken))
		{
			try
			{
				await Task.Delay(milliseconds, cancellationToken);
			}
			catch (TaskCanceledException)
			{
			}
		}

		public static bool IsCanceled(this CancellationTokenSource source)
		{
			return source?.IsCancellationRequested ?? true;
		}
	}
}
