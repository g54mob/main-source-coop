using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace NomadDrive.Features.EvilRoads
{
	public static class RoadBuildScheduler
	{
		private readonly struct QueuedJob
		{
			public readonly Func<UniTask> Work;

			public readonly UniTaskCompletionSource Completion;

			public QueuedJob(Func<UniTask> work, UniTaskCompletionSource completion)
			{
				Work = work;
				Completion = completion;
			}
		}

		private static readonly Queue<QueuedJob> _queue = new Queue<QueuedJob>();

		private static bool _workerRunning;

		public static int PendingCount => _queue.Count;

		public static UniTask Enqueue(Func<UniTask> work)
		{
			UniTaskCompletionSource uniTaskCompletionSource = new UniTaskCompletionSource();
			_queue.Enqueue(new QueuedJob(work, uniTaskCompletionSource));
			if (!_workerRunning)
			{
				RunWorker().Forget();
			}
			return uniTaskCompletionSource.Task;
		}

		private static async UniTaskVoid RunWorker()
		{
			_workerRunning = true;
			try
			{
				while (_queue.Count > 0)
				{
					QueuedJob job = _queue.Dequeue();
					try
					{
						if (job.Work != null)
						{
							await job.Work();
						}
						job.Completion.TrySetResult();
					}
					catch (Exception exception)
					{
						job.Completion.TrySetException(exception);
					}
				}
			}
			finally
			{
				_workerRunning = false;
			}
		}
	}
}
