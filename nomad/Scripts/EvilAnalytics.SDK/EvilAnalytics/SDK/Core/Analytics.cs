using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EvilAnalytics.SDK.Crashlytics;
using EvilAnalytics.SDK.Gdpr;
using EvilAnalytics.SDK.Hardware;
using EvilAnalytics.SDK.Leaderboards;
using EvilAnalytics.SDK.Network;
using EvilAnalytics.SDK.Performance;
using EvilAnalytics.SDK.RemoteConfig;
using EvilAnalytics.Shared.BugReports;
using EvilAnalytics.Shared.Common;
using EvilAnalytics.Shared.Crashlytics;
using EvilAnalytics.Shared.Events;
using EvilAnalytics.Shared.Gdpr;

namespace EvilAnalytics.SDK.Core
{
	public static class Analytics
	{
		private static EvilAnalyticsConfig? _config;

		private static ApiClient? _apiClient;

		private static SessionManager? _sessionManager;

		private static ConsentManager? _consentManager;

		private static HardwareCollector? _hardwareCollector;

		private static PerformanceTracker? _performanceTracker;

		private static CrashlyticsTracker? _crashlyticsTracker;

		private static EventBatcher? _eventBatcher;

		private static OfflineQueue? _offlineQueue;

		private static RemoteConfigManager? _remoteConfig;

		private static LeaderboardManager? _leaderboards;

		private static string? _deviceId;

		private static bool _isInitialized;

		private static Action<string>? _logger;

		private static Action<string>? _customLogger;

		public static string SdkVersion => "1.0.0";

		public static bool IsInitialized => _isInitialized;

		public static Guid? CurrentSessionId => _sessionManager?.CurrentSessionId;

		public static Guid? PlayerId => _sessionManager?.PlayerId;

		public static bool IsBatchingEnabled => _eventBatcher != null;

		public static int PendingEventCount => (_eventBatcher?.QueuedCount ?? 0) + (_offlineQueue?.Count ?? 0);

		public static RemoteConfigManager RemoteConfig => _remoteConfig;

		public static LeaderboardManager Leaderboards => _leaderboards;

		public static bool HasAnalyticsConsent => _consentManager?.HasConsent(ConsentType.Analytics) ?? false;

		public static void SetLogger(Action<string> logger)
		{
			_customLogger = logger;
		}

		public static Task InitializeAsync(EvilAnalyticsConfig config, string deviceId)
		{
			if (_isInitialized)
			{
				Log("SDK already initialized");
				return Task.CompletedTask;
			}
			_config = config ?? throw new ArgumentNullException("config");
			_deviceId = deviceId ?? throw new ArgumentNullException("deviceId");
			if (config.EnableDebugLogging)
			{
				_logger = _customLogger ?? ((Action<string>)delegate(string msg)
				{
					Console.WriteLine(msg);
				});
			}
			config.Validate(_logger);
			Log("Initializing EvilAnalytics SDK...");
			_apiClient = new ApiClient(config, _logger);
			_sessionManager = new SessionManager(config, _apiClient, _logger);
			_consentManager = new ConsentManager(config, _apiClient, deviceId, _logger);
			_hardwareCollector = new HardwareCollector();
			_performanceTracker = new PerformanceTracker
			{
				IsEnabled = config.EnablePerformanceTracking
			};
			_crashlyticsTracker = new CrashlyticsTracker(_logger)
			{
				IsEnabled = config.EnableCrashlytics
			};
			_remoteConfig = new RemoteConfigManager(_apiClient);
			_leaderboards = new LeaderboardManager(_apiClient, deviceId);
			if (config.EnableEventBatching)
			{
				_eventBatcher = new EventBatcher(_apiClient, deviceId, _logger, config.EventBatchSize, config.EventBatchFlushIntervalSeconds);
				if (!string.IsNullOrEmpty(config.OfflineQueuePath))
				{
					_offlineQueue = new OfflineQueue(config.OfflineQueuePath, _logger, config.MaxOfflineQueueSize);
					_eventBatcher.OnSendFailed = delegate(List<GameEvent> events)
					{
						_offlineQueue.Enqueue(events);
					};
				}
				Log($"Event batching enabled (batch size: {config.EventBatchSize}, flush interval: {config.EventBatchFlushIntervalSeconds}s)");
			}
			_isInitialized = true;
			Log("SDK initialized successfully");
			return Task.CompletedTask;
		}

		public static void GrantConsent(ConsentType types = ConsentType.All)
		{
			EnsureInitialized();
			_consentManager.GrantConsent(types);
		}

		public static void RevokeConsent()
		{
			EnsureInitialized();
			_consentManager.RevokeAllConsent();
		}

		public static async Task<bool> StartSessionAsync(string? platform = null)
		{
			EnsureInitialized();
			if (!_consentManager.HasConsent(ConsentType.Analytics))
			{
				Log("Analytics consent not granted, session not started");
				return false;
			}
			try
			{
				if (await _sessionManager.StartSessionAsync(_deviceId, platform) != null)
				{
					_performanceTracker?.StartTracking();
					return true;
				}
				return false;
			}
			catch
			{
				return false;
			}
		}

		public static async Task EndSessionAsync(SessionEndReason reason = SessionEndReason.Normal)
		{
			EnsureInitialized();
			SessionPerformanceSummary sessionPerformanceSummary = null;
			if (_performanceTracker != null && _performanceTracker.IsTracking)
			{
				sessionPerformanceSummary = _performanceTracker.GetSummaryAndReset();
				if (sessionPerformanceSummary != null)
				{
					Log($"Session performance: Avg FPS={sessionPerformanceSummary.AvgFps:F1}, Min={sessionPerformanceSummary.MinFps:F1}, Max={sessionPerformanceSummary.MaxFps:F1}, Samples={sessionPerformanceSummary.SampleCount}");
				}
			}
			await _sessionManager.EndSessionAsync(_deviceId, reason, sessionPerformanceSummary);
		}

		public static async Task<bool> TrySendHeartbeatAsync()
		{
			if (!_isInitialized || !_sessionManager.IsSessionActive)
			{
				return false;
			}
			return await _sessionManager.TrySendHeartbeatAsync(_deviceId);
		}

		public static async Task<bool> SendHeartbeatAsync()
		{
			EnsureInitialized();
			return await _sessionManager.SendHeartbeatAsync(_deviceId);
		}

		public static async Task TrackEventAsync(string eventName, Dictionary<string, object>? properties = null)
		{
			await TrackEventAsync(eventName, EventCategory.Custom, properties);
		}

		public static async Task TrackEventAsync(string eventName, EventCategory category, Dictionary<string, object>? properties = null)
		{
			EnsureInitialized();
			if (_consentManager.HasConsent(ConsentType.Analytics))
			{
				if (!_sessionManager.IsSessionActive)
				{
					Log("No active session, event not tracked");
					return;
				}
				await SendEventAsync(new GameEvent
				{
					EventName = eventName,
					Category = category,
					SessionId = _sessionManager.CurrentSessionId.Value,
					Timestamp = DateTimeOffset.UtcNow,
					Properties = properties
				});
			}
		}

		public static async Task TrackEventWithValueAsync(string eventName, decimal value, EventCategory category = EventCategory.Custom, Dictionary<string, object>? properties = null)
		{
			EnsureInitialized();
			if (_consentManager.HasConsent(ConsentType.Analytics))
			{
				if (!_sessionManager.IsSessionActive)
				{
					Log("No active session, event not tracked");
					return;
				}
				await SendEventAsync(new GameEvent
				{
					EventName = eventName,
					Category = category,
					SessionId = _sessionManager.CurrentSessionId.Value,
					Timestamp = DateTimeOffset.UtcNow,
					Properties = properties,
					Value = value
				});
			}
		}

		public static async Task TrackLevelEventAsync(string eventName, int levelId, Dictionary<string, object>? properties = null)
		{
			EnsureInitialized();
			if (_consentManager.HasConsent(ConsentType.Analytics))
			{
				if (!_sessionManager.IsSessionActive)
				{
					Log("No active session, event not tracked");
					return;
				}
				await SendEventAsync(new GameEvent
				{
					EventName = eventName,
					Category = EventCategory.Progression,
					SessionId = _sessionManager.CurrentSessionId.Value,
					Timestamp = DateTimeOffset.UtcNow,
					Properties = properties,
					LevelId = levelId
				});
			}
		}

		private static async Task SendEventAsync(GameEvent evt)
		{
			if (_eventBatcher != null)
			{
				_eventBatcher.QueueEvent(evt);
				Log("Event queued: " + evt.EventName);
				return;
			}
			try
			{
				EventBatch batch = new EventBatch
				{
					DeviceId = _deviceId,
					SdkVersion = "1.0.0",
					Events = new List<GameEvent> { evt },
					SentAt = DateTimeOffset.UtcNow
				};
				Log("Sending event: " + evt.EventName);
				EventBatchResponse eventBatchResponse = await _apiClient.SendEventBatchAsync(batch);
				if (eventBatchResponse != null)
				{
					Log($"Event sent: {eventBatchResponse.Accepted} accepted, {eventBatchResponse.Rejected} rejected");
				}
				else if (_offlineQueue != null)
				{
					_offlineQueue.Enqueue(evt);
					Log("Event queued offline: " + evt.EventName);
				}
			}
			catch (Exception ex)
			{
				Log("Failed to send event: " + ex.Message);
				if (_offlineQueue != null)
				{
					_offlineQueue.Enqueue(evt);
					Log("Event queued offline after failure: " + evt.EventName);
				}
			}
		}

		public static async Task FlushEventsAsync()
		{
			if (!_isInitialized)
			{
				return;
			}
			if (_eventBatcher != null)
			{
				await _eventBatcher.FlushAsync();
			}
			if (_offlineQueue != null && _offlineQueue.HasEvents)
			{
				await _offlineQueue.TryFlushAsync(async delegate(List<GameEvent> events)
				{
					EventBatch batch = new EventBatch
					{
						DeviceId = _deviceId,
						SdkVersion = "1.0.0",
						Events = events,
						SentAt = DateTimeOffset.UtcNow
					};
					return await _apiClient.SendEventBatchAsync(batch) != null;
				});
			}
		}

		public static async Task CollectHardwareInfoAsync()
		{
			EnsureInitialized();
			if (!_consentManager.HasConsent(ConsentType.Hardware))
			{
				Log("Hardware consent not granted");
				return;
			}
			HardwareInfo info = _hardwareCollector.Collect(_deviceId);
			await _apiClient.SendHardwareInfoAsync(info);
		}

		public static async Task SendHardwareInfoAsync(HardwareInfo info)
		{
			EnsureInitialized();
			if (!_consentManager.HasConsent(ConsentType.Hardware))
			{
				Log("Hardware consent not granted");
				return;
			}
			info.DeviceId = _deviceId;
			if (info.CollectedAt == default(DateTimeOffset))
			{
				info.CollectedAt = DateTimeOffset.UtcNow;
			}
			await _apiClient.SendHardwareInfoAsync(info);
			Log("Hardware info sent");
		}

		public static async Task SetConsentAsync(ConsentType type, bool granted)
		{
			EnsureInitialized();
			await _consentManager.SetConsentAsync(type, granted);
		}

		public static bool HasConsent(ConsentType type)
		{
			EnsureInitialized();
			return _consentManager.HasConsent(type);
		}

		public static async Task<DataExportResponse?> ExportDataAsync()
		{
			EnsureInitialized();
			return await _consentManager.ExportDataAsync();
		}

		public static async Task<AnonymizeResponse?> RequestAnonymizationAsync(string? reason = null)
		{
			EnsureInitialized();
			return await _consentManager.RequestAnonymizationAsync(reason);
		}

		public static async Task<BugReportResponse> SubmitBugReportAsync(string message, byte[] screenshot = null, string sceneName = null, string gameVersion = null)
		{
			return await SubmitBugReportWithExtendedDataAsync(message, screenshot, sceneName, gameVersion);
		}

		public static async Task<BugReportResponse> SubmitBugReportWithExtendedDataAsync(string message, byte[] screenshot = null, string sceneName = null, string gameVersion = null, SystemResourceSnapshot systemResources = null, List<LogEntry> recentLogs = null, string fullReportJson = null)
		{
			EnsureInitialized();
			if (!_sessionManager.IsSessionActive)
			{
				Log("No active session, bug report not submitted");
				return null;
			}
			if (string.IsNullOrWhiteSpace(message))
			{
				Log("Bug report message is required");
				return null;
			}
			try
			{
				SessionPerformanceSummary sessionPerformanceSummary = _performanceTracker?.GetSummary();
				if (recentLogs == null && _crashlyticsTracker != null && _crashlyticsTracker.IsEnabled)
				{
					recentLogs = _crashlyticsTracker.GetRecentLogs();
				}
				BugReportRequest request = new BugReportRequest
				{
					DeviceId = _deviceId,
					SessionId = _sessionManager.CurrentSessionId.Value,
					Message = message,
					CurrentFps = sessionPerformanceSummary?.AvgFps,
					AvgFps = sessionPerformanceSummary?.AvgFps,
					MinFps = sessionPerformanceSummary?.MinFps,
					MemoryUsedMb = sessionPerformanceSummary?.AvgMemoryMb,
					SceneName = sceneName,
					GameVersion = (gameVersion ?? _config?.AppVersion),
					Timestamp = DateTimeOffset.UtcNow,
					SystemResources = systemResources,
					RecentLogs = recentLogs,
					FullReportJson = fullReportJson
				};
				Log(string.Format("Submitting bug report{0}...", (screenshot != null) ? " with screenshot" : ""));
				BugReportResponse bugReportResponse = await _apiClient.SubmitBugReportAsync(request, screenshot);
				if (bugReportResponse != null && bugReportResponse.Success)
				{
					Log($"Bug report submitted: {bugReportResponse.BugReportId}");
				}
				else
				{
					Log("Bug report submission failed");
				}
				return bugReportResponse;
			}
			catch (Exception ex)
			{
				Log($"Failed to submit bug report: {ex.Message}");
				return null;
			}
		}

		public static void RecordFps(float fps)
		{
			if (_isInitialized)
			{
				_performanceTracker?.RecordFps(fps);
			}
		}

		public static void UpdateMemoryUsage(float memoryMb, float gpuMemoryMb = 0f)
		{
			if (_isInitialized)
			{
				_performanceTracker?.RecordMemory(memoryMb);
			}
		}

		public static SessionPerformanceSummary GetCurrentPerformanceSummary()
		{
			if (!_isInitialized)
			{
				return null;
			}
			return _performanceTracker?.GetSummary();
		}

		public static void RecordLog(string message, string stackTrace, LogSeverity severity)
		{
			if (_isInitialized)
			{
				_crashlyticsTracker?.RecordLog(message, stackTrace, severity);
			}
		}

		public static void SetCrashlyticsContextCallbacks(Func<string> getScene, Func<float?> getFps, Func<float?> getMemory)
		{
			if (_crashlyticsTracker != null)
			{
				_crashlyticsTracker.GetCurrentScene = getScene;
				_crashlyticsTracker.GetCurrentFps = getFps;
				_crashlyticsTracker.GetCurrentMemoryMb = getMemory;
			}
		}

		public static void SetCrashlyticsEnabled(bool enabled)
		{
			if (_crashlyticsTracker != null)
			{
				_crashlyticsTracker.IsEnabled = enabled;
			}
		}

		public static void SetCrashlyticsMinSeverity(LogSeverity minSeverity)
		{
			if (_crashlyticsTracker != null)
			{
				_crashlyticsTracker.MinSeverity = minSeverity;
			}
		}

		public static void SetOnCriticalErrorCallback(Action<LogSeverity> callback)
		{
			if (_crashlyticsTracker != null)
			{
				_crashlyticsTracker.OnCriticalError = callback;
			}
		}

		public static void SetCrashlyticsDeduplication(bool enabled, float windowSeconds = 5f)
		{
			if (_crashlyticsTracker != null)
			{
				_crashlyticsTracker.EnableDeduplication = enabled;
				_crashlyticsTracker.DeduplicationWindow = TimeSpan.FromSeconds(windowSeconds);
			}
		}

		public static bool IsCriticalSeverity(LogSeverity severity)
		{
			return CrashlyticsTracker.IsCriticalSeverity(severity);
		}

		public static async Task<bool> FlushCrashLogsAsync(string platform = null, string gameVersion = null)
		{
			if (!_isInitialized || !_sessionManager.IsSessionActive)
			{
				return false;
			}
			if (_crashlyticsTracker == null || !_crashlyticsTracker.HasPendingLogs)
			{
				return false;
			}
			try
			{
				List<LogEntry> andClearPendingLogs = _crashlyticsTracker.GetAndClearPendingLogs();
				if (andClearPendingLogs.Count == 0)
				{
					return true;
				}
				LogBatchRequest request = new LogBatchRequest
				{
					DeviceId = _deviceId,
					SessionId = _sessionManager.CurrentSessionId.Value,
					GameVersion = (gameVersion ?? _config?.AppVersion),
					Platform = platform,
					Entries = andClearPendingLogs
				};
				Log($"Sending {andClearPendingLogs.Count} crash logs...");
				LogBatchResponse logBatchResponse = await _apiClient.SendLogBatchAsync(request);
				if (logBatchResponse != null)
				{
					Log($"Crash logs sent: {logBatchResponse.Accepted} accepted, {logBatchResponse.Rejected} rejected");
					return true;
				}
				return false;
			}
			catch (Exception ex)
			{
				Log("Failed to send crash logs: " + ex.Message);
				return false;
			}
		}

		public static List<LogEntry> GetRecentCrashLogs()
		{
			return _crashlyticsTracker?.GetRecentLogs() ?? new List<LogEntry>();
		}

		public static SystemResourceSnapshot CollectSystemResources(long systemMemoryTotalMb, long systemMemoryUsedMb, long unityAllocatedMb, long unityReservedMb, long gcTotalMb, int screenWidth, int screenHeight, int refreshRate, bool isFullScreen, string qualityLevel, string graphicsDevice, int graphicsMemoryMb, string processorType, int processorCount, int processorFrequency)
		{
			float systemMemoryUsagePercent = ((systemMemoryTotalMb > 0) ? ((float)systemMemoryUsedMb * 100f / (float)systemMemoryTotalMb) : 0f);
			return new SystemResourceSnapshot
			{
				SystemMemoryTotalMb = systemMemoryTotalMb,
				SystemMemoryUsedMb = systemMemoryUsedMb,
				SystemMemoryUsagePercent = systemMemoryUsagePercent,
				UnityAllocatedMemoryMb = unityAllocatedMb,
				UnityReservedMemoryMb = unityReservedMb,
				GcTotalMemoryMb = gcTotalMb,
				ScreenWidth = screenWidth,
				ScreenHeight = screenHeight,
				RefreshRate = refreshRate,
				IsFullScreen = isFullScreen,
				QualityLevel = (qualityLevel ?? string.Empty),
				GraphicsDeviceName = (graphicsDevice ?? string.Empty),
				GraphicsMemoryMb = graphicsMemoryMb,
				ProcessorType = (processorType ?? string.Empty),
				ProcessorCount = processorCount,
				ProcessorFrequencyMhz = processorFrequency
			};
		}

		public static async Task ShutdownAsync()
		{
			if (_isInitialized)
			{
				Log("Shutting down SDK...");
				await FlushEventsAsync();
				if (_sessionManager?.IsSessionActive ?? false)
				{
					await EndSessionAsync();
				}
				_eventBatcher?.Dispose();
				_offlineQueue?.Dispose();
				_isInitialized = false;
				_config = null;
				_apiClient = null;
				_sessionManager = null;
				_consentManager = null;
				_hardwareCollector = null;
				_performanceTracker = null;
				_crashlyticsTracker = null;
				_eventBatcher = null;
				_offlineQueue = null;
				_remoteConfig = null;
				_leaderboards = null;
				Log("SDK shutdown complete");
			}
		}

		private static void EnsureInitialized()
		{
			if (!_isInitialized)
			{
				throw new InvalidOperationException("EvilAnalytics SDK not initialized. Call InitializeAsync first.");
			}
		}

		private static void Log(string message)
		{
			_logger?.Invoke("[EvilAnalytics] " + message);
		}
	}
}
