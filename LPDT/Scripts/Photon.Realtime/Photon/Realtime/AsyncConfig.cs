using System.Threading;
using System.Threading.Tasks;

namespace Photon.Realtime
{
	public class AsyncConfig
	{
		public static AsyncConfig Global = new AsyncConfig();

		public bool CreateServiceTask { get; set; } = true;

		public int ServiceIntervalMs { get; set; } = 10;

		public float OperationTimeoutSec { get; set; } = 15f;

		public TaskFactory TaskFactory { get; set; } = Task.Factory;

		public TaskScheduler TaskScheduler
		{
			get
			{
				if (TaskFactory?.Scheduler != null)
				{
					return TaskFactory.Scheduler;
				}
				return TaskScheduler.Default;
			}
		}

		public CancellationToken CancellationToken { get; set; }

		public bool IsCancellationRequested => CancellationToken.IsCancellationRequested;

		public static void InitForUnity()
		{
			Global = CreateUnityAsyncConfig();
		}

		public static AsyncConfig CreateUnityAsyncConfig()
		{
			return new AsyncConfig
			{
				TaskFactory = CreateUnityTaskFactory()
			};
		}

		public static TaskFactory CreateUnityTaskFactory()
		{
			return new TaskFactory(CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskContinuationOptions.DenyChildAttach | TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.FromCurrentSynchronizationContext());
		}
	}
}
