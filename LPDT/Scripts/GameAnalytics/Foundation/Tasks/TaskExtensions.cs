using System;
using System.Collections;
using UnityEngine;

namespace Foundation.Tasks
{
	public static class TaskExtensions
	{
		public static T ThrowIfFaulted<T>(this T self) where T : AsyncTask
		{
			if (self.IsFaulted)
			{
				throw self.Exception;
			}
			return self;
		}

		public static T ContinueWith<T>(this T self, Action<T> continuation) where T : AsyncTask
		{
			if (self.IsCompleted)
			{
				continuation(self);
			}
			else
			{
				self.AddContinue(continuation);
			}
			return self;
		}

		public static T AddTimeout<T>(this T self, int seconds, Action<AsyncTask> onTimeout = null) where T : AsyncTask
		{
			TaskManager.StartRoutine(TimeOutAsync(self, seconds, onTimeout));
			return self;
		}

		private static IEnumerator TimeOutAsync(AsyncTask task, int seconds, Action<AsyncTask> onTimeout = null)
		{
			yield return new WaitForSeconds(seconds);
			if (task.IsRunning)
			{
				onTimeout?.Invoke(task);
				task.Complete(new Exception("Timeout"));
			}
		}
	}
}
