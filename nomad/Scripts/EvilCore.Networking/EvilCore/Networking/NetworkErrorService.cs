using System;
using EvilCore.EvilPack.EvilLogger;
using UnityEngine;

namespace EvilCore.Networking
{
	public class NetworkErrorService : INetworkErrorService, IDisposable
	{
		private const float PendingStalenessSeconds = 120f;

		private readonly IAuthService _authService;

		private readonly INetworkTelemetry _telemetry;

		private readonly ISceneFlowManager _sceneFlowManager;

		private NetworkErrorInfo? _pending;

		private float _pendingTime;

		public event Action<NetworkErrorInfo> OnNetworkError;

		public NetworkErrorService(IAuthService authService, INetworkTelemetry telemetry = null, ISceneFlowManager sceneFlowManager = null)
		{
			_authService = authService;
			_telemetry = telemetry;
			_sceneFlowManager = sceneFlowManager;
			if (_authService != null)
			{
				_authService.OnLoginFailed += HandleLoginFailed;
			}
		}

		public void Report(NetworkErrorType type, string technicalDetail = null)
		{
			Report(new NetworkErrorInfo(type, technicalDetail));
		}

		public void Report(NetworkErrorInfo info)
		{
			_telemetry?.RecordReportedError(info.Type, info.TechnicalDetail);
			string arg = (string.IsNullOrEmpty(info.TechnicalDetail) ? "" : (" — " + info.TechnicalDetail));
			Action<NetworkErrorInfo> action = this.OnNetworkError;
			if (action != null)
			{
				EvilLogger.LogError($"[NetworkError] {info.Type}{arg} (shown)", LogCategory.Network, "Report", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Networking\\_Core\\NetworkErrorService.cs", 55);
				action(info);
				return;
			}
			_pending = info;
			_pendingTime = Time.realtimeSinceStartup;
			ISceneFlowManager sceneFlowManager = _sceneFlowManager;
			if (sceneFlowManager != null && sceneFlowManager.IsMainMenuSceneLoaded)
			{
				EvilLogger.LogError($"[NetworkError] {info.Type}{arg} (buffered — MainMenu is loaded but no listener subscribed; verify NetworkErrorPopup exists in the MainMenu scene, its ROOT is active, and is assigned on MainMenuUIInstaller)", LogCategory.Network, "Report", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Networking\\_Core\\NetworkErrorService.cs", 69);
			}
		}

		public NetworkErrorInfo? ConsumePending()
		{
			if (!_pending.HasValue)
			{
				return null;
			}
			NetworkErrorInfo? pending = _pending;
			_pending = null;
			if (Time.realtimeSinceStartup - _pendingTime > 120f)
			{
				return null;
			}
			return pending;
		}

		private void HandleLoginFailed(string reason)
		{
			_telemetry?.RecordReportedError(NetworkErrorType.AuthenticationFailed, reason);
		}

		public void Dispose()
		{
			if (_authService != null)
			{
				_authService.OnLoginFailed -= HandleLoginFailed;
			}
		}
	}
}
