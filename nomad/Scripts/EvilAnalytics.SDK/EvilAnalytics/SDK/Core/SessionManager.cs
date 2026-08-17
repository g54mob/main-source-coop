using System;
using System.Threading.Tasks;
using EvilAnalytics.SDK.Network;
using EvilAnalytics.Shared.Events;

namespace EvilAnalytics.SDK.Core
{
	public class SessionManager
	{
		private readonly EvilAnalyticsConfig _config;

		private readonly ApiClient _apiClient;

		private readonly Action<string> _logger;

		private DateTimeOffset _lastHeartbeat = DateTimeOffset.MinValue;

		public Guid? CurrentSessionId { get; private set; }

		public Guid? PlayerId { get; private set; }

		public bool IsSessionActive => CurrentSessionId.HasValue;

		public DateTimeOffset? SessionStartTime { get; private set; }

		public SessionManager(EvilAnalyticsConfig config, ApiClient apiClient, Action<string> logger = null)
		{
			_config = config;
			_apiClient = apiClient;
			_logger = logger;
		}

		public async Task<SessionStartResponse> StartSessionAsync(string deviceId, string platform = null)
		{
			if (IsSessionActive)
			{
				if (_logger != null)
				{
					_logger("[EvilAnalytics] Session already active, ending previous session");
				}
				await EndSessionAsync(deviceId);
			}
			SessionStartRequest request = new SessionStartRequest
			{
				DeviceId = deviceId,
				ApiKey = _config.ApiKey,
				SdkVersion = Analytics.SdkVersion,
				AppVersion = _config.AppVersion,
				Platform = platform,
				Timestamp = DateTimeOffset.UtcNow
			};
			try
			{
				SessionStartResponse sessionStartResponse = await _apiClient.StartSessionAsync(request);
				if (sessionStartResponse != null)
				{
					CurrentSessionId = sessionStartResponse.SessionId;
					PlayerId = sessionStartResponse.PlayerId;
					SessionStartTime = DateTimeOffset.UtcNow;
					_lastHeartbeat = DateTimeOffset.UtcNow;
					if (_logger != null)
					{
						_logger($"[EvilAnalytics] Session started: {CurrentSessionId}");
					}
				}
				return sessionStartResponse;
			}
			catch (Exception ex)
			{
				if (_logger != null)
				{
					_logger($"[EvilAnalytics] Failed to start session: {ex.Message}");
				}
				throw;
			}
		}

		public async Task EndSessionAsync(string deviceId, SessionEndReason reason = SessionEndReason.Normal, SessionPerformanceSummary performance = null)
		{
			if (!IsSessionActive)
			{
				if (_logger != null)
				{
					_logger("[EvilAnalytics] No active session to end");
				}
				return;
			}
			SessionEndRequest request = new SessionEndRequest
			{
				SessionId = CurrentSessionId.Value,
				DeviceId = deviceId,
				ApiKey = _config.ApiKey,
				Timestamp = DateTimeOffset.UtcNow,
				EndReason = reason,
				Performance = performance
			};
			try
			{
				await _apiClient.EndSessionAsync(request);
				if (_logger != null)
				{
					_logger($"[EvilAnalytics] Session ended: {CurrentSessionId}");
				}
			}
			catch (Exception ex)
			{
				if (_logger != null)
				{
					_logger($"[EvilAnalytics] Failed to end session: {ex.Message}");
				}
			}
			finally
			{
				CurrentSessionId = null;
				SessionStartTime = null;
			}
		}

		public async Task<bool> TrySendHeartbeatAsync(string deviceId)
		{
			if (!IsSessionActive || !_config.EnableHeartbeat)
			{
				return false;
			}
			if ((DateTimeOffset.UtcNow - _lastHeartbeat).TotalSeconds < (double)_config.HeartbeatInterval)
			{
				return false;
			}
			return await SendHeartbeatAsync(deviceId);
		}

		public async Task<bool> SendHeartbeatAsync(string deviceId)
		{
			if (!IsSessionActive)
			{
				return false;
			}
			HeartbeatRequest request = new HeartbeatRequest
			{
				SessionId = CurrentSessionId.Value,
				DeviceId = deviceId,
				ApiKey = _config.ApiKey,
				Timestamp = DateTimeOffset.UtcNow
			};
			try
			{
				HeartbeatResponse heartbeatResponse = await _apiClient.SendHeartbeatAsync(request);
				if (heartbeatResponse != null && heartbeatResponse.Success)
				{
					_lastHeartbeat = DateTimeOffset.UtcNow;
					if (_logger != null)
					{
						_logger("[EvilAnalytics] Heartbeat sent");
					}
					return true;
				}
			}
			catch (Exception ex)
			{
				if (_logger != null)
				{
					_logger($"[EvilAnalytics] Heartbeat failed: {ex.Message}");
				}
			}
			return false;
		}

		public TimeSpan? GetSessionDuration()
		{
			if (!SessionStartTime.HasValue)
			{
				return null;
			}
			return DateTimeOffset.UtcNow - SessionStartTime.Value;
		}

		public double GetSecondsSinceLastHeartbeat()
		{
			return (DateTimeOffset.UtcNow - _lastHeartbeat).TotalSeconds;
		}
	}
}
