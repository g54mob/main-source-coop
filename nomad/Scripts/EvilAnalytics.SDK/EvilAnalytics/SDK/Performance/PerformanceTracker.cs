using System;
using System.Linq;
using EvilAnalytics.Shared.Events;

namespace EvilAnalytics.SDK.Performance
{
	public class PerformanceTracker
	{
		public const int MaxSampleCount = 10000;

		private readonly float[] _fpsSamples = new float[10000];

		private readonly float[] _memorySamples = new float[10000];

		private readonly object _lock = new object();

		private int _fpsWriteIndex;

		private int _fpsCount;

		private int _memoryWriteIndex;

		private int _memoryCount;

		private float _peakMemoryMb;

		private bool _isTracking;

		public bool IsEnabled { get; set; } = true;

		public bool IsTracking => _isTracking;

		public int SampleCount
		{
			get
			{
				lock (_lock)
				{
					return _fpsCount;
				}
			}
		}

		public void StartTracking()
		{
			lock (_lock)
			{
				_fpsWriteIndex = 0;
				_fpsCount = 0;
				_memoryWriteIndex = 0;
				_memoryCount = 0;
				_peakMemoryMb = 0f;
				_isTracking = true;
			}
		}

		public void StopTracking()
		{
			lock (_lock)
			{
				_isTracking = false;
			}
		}

		public void RecordFps(float fps)
		{
			if (!IsEnabled || !_isTracking || fps <= 0f)
			{
				return;
			}
			lock (_lock)
			{
				_fpsSamples[_fpsWriteIndex] = fps;
				_fpsWriteIndex = (_fpsWriteIndex + 1) % 10000;
				if (_fpsCount < 10000)
				{
					_fpsCount++;
				}
			}
		}

		public void RecordMemory(float memoryMb)
		{
			if (!IsEnabled || !_isTracking || memoryMb <= 0f)
			{
				return;
			}
			lock (_lock)
			{
				_memorySamples[_memoryWriteIndex] = memoryMb;
				_memoryWriteIndex = (_memoryWriteIndex + 1) % 10000;
				if (_memoryCount < 10000)
				{
					_memoryCount++;
				}
				if (memoryMb > _peakMemoryMb)
				{
					_peakMemoryMb = memoryMb;
				}
			}
		}

		public SessionPerformanceSummary GetSummary()
		{
			lock (_lock)
			{
				if (_fpsCount == 0)
				{
					return null;
				}
				float[] array = new float[_fpsCount];
				Array.Copy(_fpsSamples, 0, array, 0, _fpsCount);
				float[] array2 = array.OrderBy((float x) => x).ToArray();
				SessionPerformanceSummary sessionPerformanceSummary = new SessionPerformanceSummary
				{
					AvgFps = array.Average(),
					MinFps = array2[0],
					MaxFps = array2[array2.Length - 1],
					P1Fps = CalculatePercentile(array2, 1),
					SampleCount = _fpsCount
				};
				if (_memoryCount > 0)
				{
					float[] array3 = new float[_memoryCount];
					Array.Copy(_memorySamples, 0, array3, 0, _memoryCount);
					sessionPerformanceSummary.AvgMemoryMb = array3.Average();
					sessionPerformanceSummary.PeakMemoryMb = _peakMemoryMb;
				}
				return sessionPerformanceSummary;
			}
		}

		public SessionPerformanceSummary GetSummaryAndReset()
		{
			lock (_lock)
			{
				SessionPerformanceSummary summary = GetSummary();
				_fpsWriteIndex = 0;
				_fpsCount = 0;
				_memoryWriteIndex = 0;
				_memoryCount = 0;
				_peakMemoryMb = 0f;
				_isTracking = false;
				return summary;
			}
		}

		private static float CalculatePercentile(float[] sorted, int percentile)
		{
			if (sorted.Length == 0)
			{
				return 0f;
			}
			if (sorted.Length == 1)
			{
				return sorted[0];
			}
			double num = (double)percentile / 100.0 * (double)(sorted.Length - 1);
			int num2 = (int)Math.Floor(num);
			int num3 = (int)Math.Ceiling(num);
			if (num2 == num3 || num3 >= sorted.Length)
			{
				return sorted[num2];
			}
			double num4 = num - (double)num2;
			return sorted[num2] * (1f - (float)num4) + sorted[num3] * (float)num4;
		}
	}
}
