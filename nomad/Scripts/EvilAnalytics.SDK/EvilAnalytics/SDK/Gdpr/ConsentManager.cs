using System;
using System.Threading.Tasks;
using EvilAnalytics.SDK.Core;
using EvilAnalytics.SDK.Network;
using EvilAnalytics.Shared.Gdpr;

namespace EvilAnalytics.SDK.Gdpr
{
	public class ConsentManager
	{
		private readonly EvilAnalyticsConfig _config;

		private readonly ApiClient _apiClient;

		private readonly string _deviceId;

		private readonly Action<string>? _logger;

		private ConsentType _currentConsent;

		private bool _consentLoaded;

		private bool _consentExplicitlyGranted;

		public bool IsConsentExplicitlyGranted => _consentExplicitlyGranted;

		public ConsentManager(EvilAnalyticsConfig config, ApiClient apiClient, string deviceId, Action<string>? logger = null)
		{
			_config = config;
			_apiClient = apiClient;
			_deviceId = deviceId;
			_logger = logger;
		}

		public bool HasConsent(ConsentType type)
		{
			if (!_consentExplicitlyGranted)
			{
				return false;
			}
			return (_currentConsent & type) == type;
		}

		public void GrantConsent(ConsentType types)
		{
			_currentConsent = types;
			_consentExplicitlyGranted = true;
			_logger?.Invoke($"[EvilAnalytics] Consent explicitly granted: {types}");
		}

		public void RevokeAllConsent()
		{
			_currentConsent = ConsentType.None;
			_consentExplicitlyGranted = false;
			_logger?.Invoke("[EvilAnalytics] All consent revoked");
		}

		public async Task<bool> LoadConsentAsync()
		{
			try
			{
				ConsentStatus consentStatus = await _apiClient.GetConsentAsync(_deviceId);
				if (consentStatus != null)
				{
					_currentConsent = ConsentType.None;
					if (consentStatus.Analytics)
					{
						_currentConsent |= ConsentType.Analytics;
					}
					if (consentStatus.Hardware)
					{
						_currentConsent |= ConsentType.Hardware;
					}
					if (consentStatus.Performance)
					{
						_currentConsent |= ConsentType.Performance;
					}
					if (consentStatus.CrashReporting)
					{
						_currentConsent |= ConsentType.CrashReporting;
					}
					_consentExplicitlyGranted = _currentConsent != ConsentType.None;
					_consentLoaded = true;
					_logger?.Invoke($"[EvilAnalytics] Consent loaded from server: {_currentConsent}");
					return true;
				}
				_logger?.Invoke("[EvilAnalytics] No consent record found - explicit consent required");
				_consentLoaded = true;
				return true;
			}
			catch (Exception ex)
			{
				_logger?.Invoke("[EvilAnalytics] Failed to load consent: " + ex.Message);
				_currentConsent = ConsentType.None;
				_consentExplicitlyGranted = false;
				_consentLoaded = false;
				return false;
			}
		}

		public async Task SetConsentAsync(ConsentType type, bool granted)
		{
			try
			{
				ConsentUpdateRequest consentUpdateRequest = new ConsentUpdateRequest
				{
					DeviceId = _deviceId
				};
				if ((type & ConsentType.Analytics) != ConsentType.None)
				{
					consentUpdateRequest.Analytics = granted;
					UpdateLocalConsent(ConsentType.Analytics, granted);
				}
				if ((type & ConsentType.Hardware) != ConsentType.None)
				{
					consentUpdateRequest.Hardware = granted;
					UpdateLocalConsent(ConsentType.Hardware, granted);
				}
				if ((type & ConsentType.Performance) != ConsentType.None)
				{
					consentUpdateRequest.Performance = granted;
					UpdateLocalConsent(ConsentType.Performance, granted);
				}
				if ((type & ConsentType.CrashReporting) != ConsentType.None)
				{
					consentUpdateRequest.CrashReporting = granted;
					UpdateLocalConsent(ConsentType.CrashReporting, granted);
				}
				await _apiClient.UpdateConsentAsync(consentUpdateRequest);
				_consentExplicitlyGranted = true;
				_logger?.Invoke($"[EvilAnalytics] Consent updated: {type} = {granted}");
			}
			catch (Exception ex)
			{
				_logger?.Invoke("[EvilAnalytics] Failed to update consent: " + ex.Message);
				throw;
			}
		}

		public async Task SetAllConsentsAsync(bool analytics, bool hardware, bool performance, bool crashReporting, string? consentVersion = null)
		{
			try
			{
				ConsentUpdateRequest request = new ConsentUpdateRequest
				{
					DeviceId = _deviceId,
					Analytics = analytics,
					Hardware = hardware,
					Performance = performance,
					CrashReporting = crashReporting,
					ConsentVersion = consentVersion
				};
				await _apiClient.UpdateConsentAsync(request);
				_currentConsent = ConsentType.None;
				if (analytics)
				{
					_currentConsent |= ConsentType.Analytics;
				}
				if (hardware)
				{
					_currentConsent |= ConsentType.Hardware;
				}
				if (performance)
				{
					_currentConsent |= ConsentType.Performance;
				}
				if (crashReporting)
				{
					_currentConsent |= ConsentType.CrashReporting;
				}
				_consentExplicitlyGranted = true;
				_consentLoaded = true;
				_logger?.Invoke($"[EvilAnalytics] All consents updated: {_currentConsent}");
			}
			catch (Exception ex)
			{
				_logger?.Invoke("[EvilAnalytics] Failed to update consents: " + ex.Message);
				throw;
			}
		}

		public async Task<DataExportResponse?> ExportDataAsync()
		{
			try
			{
				DataExportRequest request = new DataExportRequest
				{
					DeviceId = _deviceId,
					IncludeEvents = true,
					IncludeSessions = true,
					IncludeHardware = true
				};
				DataExportResponse result = await _apiClient.ExportDataAsync(request);
				_logger?.Invoke("[EvilAnalytics] Data export completed");
				return result;
			}
			catch (Exception ex)
			{
				_logger?.Invoke("[EvilAnalytics] Data export failed: " + ex.Message);
				throw;
			}
		}

		public async Task<AnonymizeResponse?> RequestAnonymizationAsync(string? reason = null)
		{
			try
			{
				AnonymizeRequest request = new AnonymizeRequest
				{
					DeviceId = _deviceId,
					Reason = reason
				};
				AnonymizeResponse result = await _apiClient.AnonymizeAsync(request);
				_logger?.Invoke("[EvilAnalytics] Anonymization completed");
				_currentConsent = ConsentType.None;
				_consentLoaded = true;
				return result;
			}
			catch (Exception ex)
			{
				_logger?.Invoke("[EvilAnalytics] Anonymization failed: " + ex.Message);
				throw;
			}
		}

		private void UpdateLocalConsent(ConsentType type, bool granted)
		{
			if (granted)
			{
				_currentConsent |= type;
			}
			else
			{
				_currentConsent &= ~type;
			}
		}
	}
}
