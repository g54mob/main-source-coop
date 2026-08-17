using System;

namespace EvilAnalytics.SDK.Core
{
	public class EvilAnalyticsConfig
	{
		private string _serverUrl = "https://localhost:5000";

		public string ApiKey { get; set; } = string.Empty;

		public string ServerUrl
		{
			get
			{
				return _serverUrl;
			}
			set
			{
				_serverUrl = value;
				IsHttpExplicitlyConfigured = !string.IsNullOrEmpty(value) && value.StartsWith("http://", StringComparison.OrdinalIgnoreCase);
			}
		}

		internal bool IsHttpExplicitlyConfigured { get; private set; }

		public bool RequireHttps { get; set; } = true;

		public bool AutoCollectHardware { get; set; }

		[Obsolete("Use LogLevel property instead for more granular control")]
		public bool EnableDebugLogging
		{
			get
			{
				return LogLevel >= SdkLogLevel.Debug;
			}
			set
			{
				LogLevel = (value ? SdkLogLevel.Debug : SdkLogLevel.Warning);
			}
		}

		public SdkLogLevel LogLevel { get; set; } = SdkLogLevel.Warning;

		public int MaxRetryAttempts { get; set; } = 3;

		public float RetryBaseDelay { get; set; } = 1f;

		public float RequestTimeout { get; set; } = 30f;

		public bool EnableCompression { get; set; } = true;

		public string? AppVersion { get; set; }

		public bool EnableHeartbeat { get; set; } = true;

		public float HeartbeatInterval { get; set; } = 30f;

		public bool EnablePerformanceTracking { get; set; } = true;

		public bool EnableCrashlytics { get; set; } = true;

		public bool EnableEventBatching { get; set; } = true;

		public int EventBatchSize { get; set; } = 50;

		public int EventBatchFlushIntervalSeconds { get; set; } = 10;

		public string? OfflineQueuePath { get; set; }

		public int MaxOfflineQueueSize { get; set; } = 1000;

		internal void Validate(Action<string> logger)
		{
			if (string.IsNullOrEmpty(ApiKey))
			{
				throw new InvalidOperationException("API key is required");
			}
			if (IsHttpExplicitlyConfigured)
			{
				if (RequireHttps)
				{
					throw new InvalidOperationException("SECURITY: HTTP is not allowed when RequireHttps is true. Set RequireHttps = false for local development only.");
				}
				logger?.Invoke("[EvilAnalytics] SECURITY WARNING: Using HTTP instead of HTTPS. API keys may be exposed in transit. Use HTTPS in production!");
			}
		}
	}
}
