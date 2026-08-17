using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using EvilAnalytics.Shared.Crashlytics;

namespace EvilAnalytics.SDK.Crashlytics
{
	public class CrashlyticsTracker
	{
		private readonly List<LogEntry> _pendingLogs = new List<LogEntry>();

		private readonly List<LogEntry> _recentLogs = new List<LogEntry>();

		private readonly Dictionary<string, DateTimeOffset> _lastSeenErrors = new Dictionary<string, DateTimeOffset>();

		private readonly object _lock = new object();

		private readonly Action<string> _logger;

		private const int MaxPendingLogs = 100;

		private const int MaxRecentLogs = 50;

		private const int MaxMessageLength = 5000;

		private const int MaxStackTraceLength = 10000;

		private const int MaxTrackedHashes = 1000;

		public bool IsEnabled { get; set; }

		public LogSeverity MinSeverity { get; set; } = LogSeverity.Warning;

		public Func<string> GetCurrentScene { get; set; }

		public Func<float?> GetCurrentFps { get; set; }

		public Func<float?> GetCurrentMemoryMb { get; set; }

		public Action<LogSeverity> OnCriticalError { get; set; }

		public TimeSpan DeduplicationWindow { get; set; } = TimeSpan.FromSeconds(5.0);

		public bool EnableDeduplication { get; set; } = true;

		public bool HasPendingLogs
		{
			get
			{
				lock (_lock)
				{
					return _pendingLogs.Count > 0;
				}
			}
		}

		public int PendingLogCount
		{
			get
			{
				lock (_lock)
				{
					return _pendingLogs.Count;
				}
			}
		}

		public CrashlyticsTracker(Action<string> logger = null)
		{
			_logger = logger;
		}

		public static bool IsCriticalSeverity(LogSeverity severity)
		{
			if (severity != LogSeverity.Error && severity != LogSeverity.Exception)
			{
				return severity == LogSeverity.Assert;
			}
			return true;
		}

		public void RecordLog(string message, string stackTrace, LogSeverity severity)
		{
			if (!IsEnabled || severity < MinSeverity)
			{
				return;
			}
			if (EnableDeduplication)
			{
				string key = ComputeErrorHash(message, stackTrace);
				lock (_lock)
				{
					if (_lastSeenErrors.TryGetValue(key, out var value) && DateTimeOffset.UtcNow - value < DeduplicationWindow)
					{
						_logger?.Invoke("[Crashlytics] Skipped duplicate " + severity.ToString() + ": " + TruncateString(message, 50));
						return;
					}
					_lastSeenErrors[key] = DateTimeOffset.UtcNow;
					if (_lastSeenErrors.Count > 1000)
					{
						CleanupOldHashes();
					}
				}
			}
			LogEntry item = new LogEntry
			{
				Message = TruncateString(message, 5000),
				StackTrace = TruncateString(stackTrace, 10000),
				Severity = severity,
				Timestamp = DateTimeOffset.UtcNow,
				SceneName = GetCurrentScene?.Invoke(),
				CurrentFps = GetCurrentFps?.Invoke(),
				MemoryUsedMb = GetCurrentMemoryMb?.Invoke()
			};
			lock (_lock)
			{
				_pendingLogs.Add(item);
				if (_pendingLogs.Count > 100)
				{
					_pendingLogs.RemoveAt(0);
				}
				_recentLogs.Add(item);
				if (_recentLogs.Count > 50)
				{
					_recentLogs.RemoveAt(0);
				}
			}
			_logger?.Invoke($"[Crashlytics] Captured {severity}: {TruncateString(message, 100)}");
			if (IsCriticalSeverity(severity))
			{
				OnCriticalError?.Invoke(severity);
			}
		}

		public List<LogEntry> GetAndClearPendingLogs()
		{
			lock (_lock)
			{
				List<LogEntry> result = new List<LogEntry>(_pendingLogs);
				_pendingLogs.Clear();
				return result;
			}
		}

		public List<LogEntry> GetRecentLogs()
		{
			lock (_lock)
			{
				return new List<LogEntry>(_recentLogs);
			}
		}

		public void ClearAll()
		{
			lock (_lock)
			{
				_pendingLogs.Clear();
				_recentLogs.Clear();
			}
		}

		private static string TruncateString(string value, int maxLength)
		{
			if (string.IsNullOrEmpty(value))
			{
				return value;
			}
			if (value.Length <= maxLength)
			{
				return value;
			}
			return value.Substring(0, maxLength);
		}

		private static string ComputeErrorHash(string message, string stackTrace)
		{
			string s = message + "|" + stackTrace;
			using SHA256 sHA = SHA256.Create();
			return BitConverter.ToString(sHA.ComputeHash(Encoding.UTF8.GetBytes(s)), 0, 16).Replace("-", "").ToLower();
		}

		private void CleanupOldHashes()
		{
			DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow - DeduplicationWindow - TimeSpan.FromMinutes(1.0);
			List<string> list = new List<string>();
			foreach (KeyValuePair<string, DateTimeOffset> lastSeenError in _lastSeenErrors)
			{
				if (lastSeenError.Value < dateTimeOffset)
				{
					list.Add(lastSeenError.Key);
				}
			}
			foreach (string item in list)
			{
				_lastSeenErrors.Remove(item);
			}
		}
	}
}
