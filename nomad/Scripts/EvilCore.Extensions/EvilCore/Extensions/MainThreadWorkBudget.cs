using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using EvilCore.EvilPack.EvilLogger;
using UnityEngine;

namespace EvilCore.Extensions
{
	public static class MainThreadWorkBudget
	{
		private readonly struct WorkItem
		{
			public readonly Action Work;

			public readonly UniTaskCompletionSource Completion;

			public WorkItem(Action work, UniTaskCompletionSource completion)
			{
				Work = work;
				Completion = completion;
			}
		}

		private static readonly Queue<WorkItem>[] _bands = new Queue<WorkItem>[3]
		{
			new Queue<WorkItem>(),
			new Queue<WorkItem>(),
			new Queue<WorkItem>()
		};

		private static bool _pumpRunning;

		public static float MaxMillisecondsPerFrame { get; set; } = 0.5f;

		public static int PendingCount => _bands[0].Count + _bands[1].Count + _bands[2].Count;

		private static bool HasWork
		{
			get
			{
				if (_bands[0].Count <= 0 && _bands[1].Count <= 0)
				{
					return _bands[2].Count > 0;
				}
				return true;
			}
		}

		public static UniTask Run(Action work, WorkPriority priority = WorkPriority.Normal)
		{
			UniTaskCompletionSource uniTaskCompletionSource = new UniTaskCompletionSource();
			_bands[(int)priority].Enqueue(new WorkItem(work, uniTaskCompletionSource));
			EnsurePump();
			return uniTaskCompletionSource.Task;
		}

		public static async UniTask<GameObject> RunInstantiate(Func<GameObject> instantiate, WorkPriority priority = WorkPriority.Normal)
		{
			GameObject result = null;
			await Run(delegate
			{
				result = instantiate?.Invoke();
			}, priority);
			return result;
		}

		public static void Clear()
		{
			for (int i = 0; i < _bands.Length; i++)
			{
				while (_bands[i].Count > 0)
				{
					_bands[i].Dequeue().Completion.TrySetCanceled();
				}
			}
		}

		private static void EnsurePump()
		{
			if (!_pumpRunning)
			{
				_pumpRunning = true;
				RunPump().Forget();
			}
		}

		private static async UniTaskVoid RunPump()
		{
			try
			{
				while (HasWork)
				{
					float realtimeSinceStartup = Time.realtimeSinceStartup;
					do
					{
						WorkItem workItem = DequeueNext();
						try
						{
							workItem.Work?.Invoke();
							workItem.Completion.TrySetResult();
						}
						catch (Exception ex)
						{
							if (!workItem.Completion.TrySetException(ex))
							{
								EvilLogger.LogError($"[MainThreadWorkBudget] Work unit threw: {ex}", "RunPump", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Extensions\\MainThreadWorkBudget.cs", 137);
							}
						}
					}
					while (HasWork && (Time.realtimeSinceStartup - realtimeSinceStartup) * 1000f < MaxMillisecondsPerFrame);
					await UniTask.Yield();
				}
			}
			finally
			{
				_pumpRunning = false;
			}
		}

		private static WorkItem DequeueNext()
		{
			if (_bands[0].Count > 0)
			{
				return _bands[0].Dequeue();
			}
			if (_bands[1].Count > 0)
			{
				return _bands[1].Dequeue();
			}
			return _bands[2].Dequeue();
		}
	}
}
