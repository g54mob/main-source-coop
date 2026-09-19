using System;
using System.Diagnostics;
using UnityEngine.Profiling;

namespace Features.NetworkTelemetry.Scripts.Internal
{
	internal static class ProcessCpuAndRamSampler
	{
		private static Process _process;

		private static bool _havePriorCpuSample;

		private static TimeSpan _priorTotalProcessorTime;

		private static DateTime _priorUtc;

		internal static void Read(out float normalizedCpuPercentOfAllLogicalProcessors, out long workingSetBytes)
		{
			normalizedCpuPercentOfAllLogicalProcessors = 0f;
			workingSetBytes = 0L;
			try
			{
				if (_process == null)
				{
					_process = Process.GetCurrentProcess();
				}
				_process.Refresh();
				workingSetBytes = Profiler.GetTotalReservedMemoryLong();
				DateTime utcNow = DateTime.UtcNow;
				TimeSpan totalProcessorTime = _process.TotalProcessorTime;
				if (!_havePriorCpuSample)
				{
					_priorTotalProcessorTime = totalProcessorTime;
					_priorUtc = utcNow;
					_havePriorCpuSample = true;
					normalizedCpuPercentOfAllLogicalProcessors = 0f;
					return;
				}
				double totalMilliseconds = (utcNow - _priorUtc).TotalMilliseconds;
				double totalMilliseconds2 = (totalProcessorTime - _priorTotalProcessorTime).TotalMilliseconds;
				_priorTotalProcessorTime = totalProcessorTime;
				_priorUtc = utcNow;
				if (totalMilliseconds <= 1.0)
				{
					normalizedCpuPercentOfAllLogicalProcessors = 0f;
					return;
				}
				int num = Environment.ProcessorCount;
				if (num < 1)
				{
					num = 1;
				}
				normalizedCpuPercentOfAllLogicalProcessors = (float)(100.0 * totalMilliseconds2 / (totalMilliseconds * (double)num));
				if (!float.IsFinite(normalizedCpuPercentOfAllLogicalProcessors))
				{
					normalizedCpuPercentOfAllLogicalProcessors = 0f;
				}
			}
			catch (Exception)
			{
				normalizedCpuPercentOfAllLogicalProcessors = 0f;
				workingSetBytes = 0L;
			}
		}
	}
}
