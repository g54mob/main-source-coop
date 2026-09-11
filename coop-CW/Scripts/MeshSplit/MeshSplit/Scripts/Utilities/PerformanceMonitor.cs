using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace MeshSplit.Scripts.Utilities
{
	public static class PerformanceMonitor
	{
		private class Entry
		{
			public Stopwatch Stopwatch;

			public float TimeThreshold;
		}

		private static readonly Dictionary<string, Entry> _stopwatches = new Dictionary<string, Entry>();

		public static void Start(string textIdentifier, float timeThreshold = 0f)
		{
			if (_stopwatches.ContainsKey(textIdentifier))
			{
				_stopwatches[textIdentifier].Stopwatch.Restart();
				return;
			}
			Stopwatch stopwatch = new Stopwatch();
			_stopwatches.Add(textIdentifier, new Entry
			{
				Stopwatch = stopwatch,
				TimeThreshold = timeThreshold
			});
			stopwatch.Start();
		}

		public static void Stop(string textIdentifier, string additionalText = null)
		{
			if (_stopwatches.TryGetValue(textIdentifier, out var value))
			{
				Stopwatch stopwatch = value.Stopwatch;
				stopwatch.Stop();
				double totalMilliseconds = stopwatch.Elapsed.TotalMilliseconds;
				if (value.TimeThreshold == 0f || totalMilliseconds >= (double)(value.TimeThreshold * 1000f))
				{
					UnityEngine.Debug.Log($"{textIdentifier} {additionalText}\n\ttime: \t{totalMilliseconds:n2} ms");
				}
				_stopwatches.Remove(textIdentifier);
			}
		}

		public static void Stop(string textIdentifier, out double milliseconds)
		{
			if (_stopwatches.TryGetValue(textIdentifier, out var value))
			{
				Stopwatch stopwatch = value.Stopwatch;
				stopwatch.Stop();
				milliseconds = stopwatch.Elapsed.TotalMilliseconds;
				_stopwatches.Remove(textIdentifier);
			}
			else
			{
				milliseconds = 0.0;
			}
		}
	}
}
