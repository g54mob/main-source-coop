using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;

namespace Fusion
{
	public abstract class LogStream : IDisposable
	{
		internal record LogTracker(ulong Threshold, bool Repeat)
		{
			public ulong Threshold { get; } = Threshold;

			public bool Repeat { get; } = Repeat;

			internal Dictionary<ulong, ulong> Tracker { get; } = new Dictionary<ulong, ulong>();

			public static LogTracker Create(ulong threshold, bool repeat)
			{
				return new LogTracker(threshold, repeat);
			}

			public override string ToString()
			{
				return string.Format("[LogTracker: {0}={1}, {2}={3}]", "Threshold", Threshold, "Repeat", Repeat);
			}

			internal void ResetTracker(int dynamicUniqueId)
			{
				Tracker.Remove((ulong)dynamicUniqueId);
			}

			internal void ResetAllTrackers()
			{
				Tracker.Clear();
			}
		}

		[HideInCallstack]
		public virtual void Log(ILogSource source, string message)
		{
			Log(message);
		}

		[HideInCallstack]
		public abstract void Log(string message);

		[HideInCallstack]
		public virtual void Log(ILogSource source, string message, Exception error)
		{
			Log(error);
		}

		[HideInCallstack]
		public virtual void Log(ILogSource source, Exception error)
		{
			Log(error);
		}

		[HideInCallstack]
		public virtual void Log(string message, Exception error)
		{
			Log(error);
		}

		[HideInCallstack]
		public abstract void Log(Exception error);

		[CanBeNull]
		public LogStream If(bool condition)
		{
			if (!condition)
			{
				return null;
			}
			return this;
		}

		[CanBeNull]
		public LogStream Once(ref bool flag)
		{
			if (!flag)
			{
				flag = true;
				return this;
			}
			return null;
		}

		public virtual void Dispose()
		{
		}

		[CanBeNull]
		internal LogStream Limit(LogTracker tracker, int dynamicUniqueId, out ulong logCount, out bool allowLogging)
		{
			if (tracker == null)
			{
				Log("LogTracker is null, not limiting logs.");
				logCount = 0uL;
				allowLogging = true;
				return this;
			}
			ulong key = (ulong)dynamicUniqueId;
			if (tracker.Tracker.TryGetValue(key, out var value))
			{
				tracker.Tracker[key] = (logCount = ++value);
			}
			else
			{
				tracker.Tracker.Add(key, logCount = 1uL);
			}
			allowLogging = logCount == 1 || (tracker.Repeat ? (logCount % tracker.Threshold == 0) : (logCount <= tracker.Threshold));
			if (!allowLogging)
			{
				return null;
			}
			return this;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[CanBeNull]
		internal LogStream Limit(LogTracker tracker, int dynamicUniqueId, out ulong logCount)
		{
			bool allowLogging;
			return Limit(tracker, dynamicUniqueId, out logCount, out allowLogging);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[CanBeNull]
		internal LogStream Limit(LogTracker tracker, int dynamicUniqueId)
		{
			ulong logCount;
			bool allowLogging;
			return Limit(tracker, dynamicUniqueId, out logCount, out allowLogging);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Conditional("DEBUG")]
		internal void Log(object message)
		{
			Log($"{message}");
		}
	}
}
