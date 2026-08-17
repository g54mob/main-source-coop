using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Den.Tools
{
	public static class Timer
	{
		public struct TimerInstance : IDisposable
		{
			public string name;

			public int calls;

			public long total;

			public long fastest;

			public long slowest;

			public long current;

			public bool logAfter;

			public bool expanded;

			public List<TimerInstance> subTimers;

			public void AddCurrent(long t)
			{
				current += t;
			}

			public void AddTime(long delta)
			{
				total += delta;
				fastest += delta;
				slowest += delta;
				int count = subTimers.Count;
				for (int i = 0; i < count; i++)
				{
					subTimers[i].AddTime(delta);
				}
			}

			public override int GetHashCode()
			{
				return name.GetHashCode();
			}

			public string Log()
			{
				return Log("", 0);
			}

			public string Log(string result, int tab)
			{
				for (int i = 0; i < tab; i++)
				{
					result += "\t";
				}
				result = result + name + " calls:" + calls + " total:" + TicksToMilliseconds(total).ToString("0.000") + " fastest:" + TicksToMilliseconds(fastest).ToString("0.000") + " average:" + TicksToMilliseconds(total / calls).ToString("0.000") + "\n";
				if (subTimers != null)
				{
					for (int j = 0; j < subTimers.Count; j++)
					{
						result += subTimers[j].Log(result, tab + 1);
					}
				}
				return result;
			}

			public int GetExpandedCount()
			{
				if (subTimers == null || !expanded)
				{
					return 0;
				}
				int count = subTimers.Count;
				int num = count;
				for (int i = 0; i < count; i++)
				{
					num += subTimers[i].GetExpandedCount();
				}
				return num;
			}

			public long SelfTime()
			{
				if (subTimers == null)
				{
					return total;
				}
				long num = 0L;
				int count = subTimers.Count;
				for (int i = 0; i < count; i++)
				{
					num += subTimers[i].total;
				}
				return total - num;
			}

			public void Dispose()
			{
				Stop(this);
				if (logAfter)
				{
					Debug.Log(name + " total:" + TicksToMilliseconds(total).ToString("0.000"));
				}
			}
		}

		public static bool enabled;

		public static long startTime;

		public static TimerInstance[] active = new TimerInstance[1000];

		public static int activeCount = 0;

		private const int maxActiveCount = 1000;

		public static List<TimerInstance> history = new List<TimerInstance>();

		public static TimerInstance temp = default(TimerInstance);

		public static Action OnHistoryAdded;

		public static long refinement = 0L;

		public static void PauseActive(long stopTime)
		{
			long num = stopTime - startTime;
			for (int i = 0; i < activeCount; i++)
			{
				active[i].current += num;
			}
		}

		public static TimerInstance Start(string name)
		{
			long num = 0L;
			num = Stopwatch.GetTimestamp();
			return Start(name, num, logAfter: false);
		}

		public static TimerInstance Start(string name, bool logAfter)
		{
			long num = 0L;
			num = Stopwatch.GetTimestamp();
			return Start(name, num, logAfter);
		}

		public static TimerInstance Start(string name, long stopTime, bool logAfter)
		{
			PauseActive(stopTime);
			if (!enabled)
			{
				return temp;
			}
			TimerInstance timerInstance;
			if (activeCount == 0)
			{
				timerInstance = new TimerInstance
				{
					name = name,
					fastest = 9223372036854775807L
				};
			}
			else
			{
				TimerInstance timerInstance2 = active[activeCount - 1];
				if (timerInstance2.subTimers != null)
				{
					timerInstance = timerInstance2.subTimers.Find((TimerInstance t) => t.name == name);
				}
				else
				{
					timerInstance = new TimerInstance
					{
						name = name,
						fastest = 9223372036854775807L
					};
					if (timerInstance2.subTimers == null)
					{
						timerInstance2.subTimers = new List<TimerInstance>();
					}
					timerInstance2.subTimers.Add(timerInstance);
				}
			}
			if (activeCount == 999)
			{
				throw new Exception("Max Timer counter is reached");
			}
			active[activeCount] = timerInstance;
			activeCount++;
			startTime = Stopwatch.GetTimestamp();
			timerInstance.logAfter = logAfter;
			return timerInstance;
		}

		public static void Stop(TimerInstance timer)
		{
			long num = 0L;
			num = Stopwatch.GetTimestamp();
			if (!enabled)
			{
				return;
			}
			PauseActive(num);
			if (activeCount == 0)
			{
				throw new Exception("Trying to stop timer when there are no active timers running");
			}
			activeCount--;
			timer.calls++;
			timer.total += timer.current;
			if (timer.current < timer.fastest)
			{
				timer.fastest = timer.current;
			}
			if (timer.current > timer.slowest)
			{
				timer.slowest = timer.current;
			}
			timer.current = 0L;
			if (enabled && activeCount == 0)
			{
				history.Add(timer);
				if (OnHistoryAdded != null)
				{
					OnHistoryAdded();
				}
			}
			startTime = Stopwatch.GetTimestamp();
		}

		public static double TicksToMilliseconds(long rawTicks)
		{
			long num = 0L;
			num = Stopwatch.Frequency;
			return 1.0 * (double)rawTicks / (double)num * 1000.0;
		}

		public static void Calibrate()
		{
			long num = 9223372036854775807L;
			for (int i = 0; i < 10; i++)
			{
				long num2 = 0L;
				num2 = Stopwatch.GetTimestamp();
				for (int j = 0; j < 10000; j++)
				{
					using (Start("Calibrate"))
					{
					}
					using (Start("Calibrate"))
					{
					}
					using (Start("Calibrate"))
					{
					}
					using (Start("Calibrate"))
					{
					}
					using (Start("Calibrate"))
					{
					}
					using (Start("Calibrate"))
					{
					}
					using (Start("Calibrate"))
					{
					}
					using (Start("Calibrate"))
					{
					}
					using (Start("Calibrate"))
					{
					}
					using (Start("Calibrate"))
					{
					}
				}
				long num3 = Stopwatch.GetTimestamp() - num2;
				if (num3 < num)
				{
					num = num3;
				}
			}
			refinement = num / 100000;
			history.Clear();
			Debug.Log("Timer calibrated with refinement " + refinement);
		}
	}
}
