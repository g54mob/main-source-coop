using System;
using Cysharp.Threading.Tasks;
using Epic.OnlineServices;
using Epic.OnlineServices.Connect;
using EpicTransport;
using EvilCore.EvilPack.EvilLogger;
using PlayEveryWare.EpicOnlineServices;
using Steamworks;
using Steamworks.Data;
using UnityEngine;
using VContainer;

namespace EvilCore.Networking
{
	public class EOSAuthManager : MonoBehaviour, IOnlineService, IAuthService
	{
		[Header("Debug")]
		[Tooltip("Skip all platform services (Steam, Xbox, PSN...) and use EOS DeviceId only.")]
		[SerializeField]
		private bool forceDeviceIdLogin;

		[Inject]
		private INetworkTelemetry _telemetry;

		private const int RefreshMaxAttempts = 3;

		private const int RefreshInitialBackoffMs = 2000;

		private ulong _authExpirationNotificationId;

		private bool _isRefreshing;

		private UniTaskCompletionSource<bool> _loginAttemptTcs;

		private string _lastLoginFailReason;

		public bool IsLoggedIn { get; private set; }

		public bool SkipPlatformServices => forceDeviceIdLogin;

		public ProductUserId LocalProductUserId { get; private set; }

		public string DisplayName { get; private set; }

		public event Action OnLoginSuccess;

		public event Action<string> OnLoginFailed;

		private void Awake()
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}

		private async void Start()
		{
			await WaitForPlatformInitAsync();
			await LoginAsync();
		}

		private void OnDestroy()
		{
			UnsubscribeAuthExpiration();
		}

		private async UniTask WaitForPlatformInitAsync()
		{
			while (EOSManager.Instance == null || EOSManager.Instance.GetEOSPlatformInterface() == null)
			{
				await UniTask.Delay(100, ignoreTimeScale: false, PlayerLoopTiming.Update, this.GetCancellationTokenOnDestroy());
			}
		}

		private async UniTask LoginAsync()
		{
			_telemetry?.RecordAttempt("login");
			NetworkRetry.Outcome<bool> outcome = await NetworkRetry.RunAsync(AttemptLoginOnceAsync, (bool success) => success, (bool _) => true, 3, 1000, 8000, 2f, 0.3f, this.GetCancellationTokenOnDestroy());
			if (outcome.Success)
			{
				_telemetry?.RecordSuccess("login", outcome.Attempts);
				return;
			}
			string text = _lastLoginFailReason ?? "EOS Connect login failed";
			EvilLogger.LogError($"[EOSAuth] Login failed after {outcome.Attempts} attempt(s): {text}", "LoginAsync", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Networking\\EOS\\EOSAuthManager.cs", 85);
			_telemetry?.RecordFailure("login", NetworkErrorType.AuthenticationFailed, text, outcome.Attempts, rateLimited: false);
			this.OnLoginFailed?.Invoke(text);
		}

		private async UniTask<bool> AttemptLoginOnceAsync()
		{
			_loginAttemptTcs = new UniTaskCompletionSource<bool>();
			try
			{
				if (forceDeviceIdLogin)
				{
					LoginWithDeviceId();
				}
				else
				{
					for (int i = 0; i < 30; i++)
					{
						if (SteamClient.IsValid)
						{
							break;
						}
						await UniTask.Delay(100, ignoreTimeScale: false, PlayerLoopTiming.Update, this.GetCancellationTokenOnDestroy());
					}
					if (SteamClient.IsValid)
					{
						await LoginWithSteamAsync();
					}
					else
					{
						LoginWithDeviceId();
					}
				}
			}
			catch (Exception ex)
			{
				EvilLogger.LogError("[EOSAuth] Login attempt threw: " + ex.Message, "AttemptLoginOnceAsync", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Networking\\EOS\\EOSAuthManager.cs", 128);
				FailLoginAttempt(ex.Message);
			}
			return await _loginAttemptTcs.Task;
		}

		private void FailLoginAttempt(string reason)
		{
			_lastLoginFailReason = reason;
			_loginAttemptTcs?.TrySetResult(result: false);
		}

		private async UniTask LoginWithSteamAsync()
		{
			await UniTask.Delay(500, ignoreTimeScale: false, PlayerLoopTiming.Update, this.GetCancellationTokenOnDestroy());
			AuthTicket ticket = null;
			try
			{
				ticket = await SteamUser.GetAuthSessionTicketAsync(default(NetIdentity));
			}
			catch (Exception)
			{
			}
			if (ticket == null || ticket.Data == null || ticket.Data.Length == 0)
			{
				FailLoginAttempt("Failed to get Steam session ticket");
				return;
			}
			string text = BitConverter.ToString(ticket.Data).Replace("-", "");
			DisplayName = SteamClient.Name;
			ConnectInterface connectInterface = EOSManager.Instance.GetEOSPlatformInterface().GetConnectInterface();
			LoginOptions options = new LoginOptions
			{
				Credentials = new Credentials
				{
					Type = ExternalCredentialType.SteamSessionTicket,
					Token = text
				}
			};
			connectInterface.Login(ref options, null, delegate(ref LoginCallbackInfo info)
			{
				OnConnectLoginComplete(info);
			});
		}

		private void LoginWithDeviceId()
		{
			DisplayName = "DevUser";
			EOSManager.Instance.StartConnectLoginWithDeviceToken(DisplayName, OnDeviceIdLoginAttempt);
		}

		private void OnDeviceIdLoginAttempt(LoginCallbackInfo info)
		{
			if (info.ResultCode == Epic.OnlineServices.Result.Success)
			{
				HandleLoginSuccess(info.LocalUserId);
			}
			else if (info.ResultCode == Epic.OnlineServices.Result.InvalidUser)
			{
				CreateDeviceIdAndLogin(info.ContinuanceToken);
			}
			else
			{
				CreateDeviceIdAndLogin(null);
			}
		}

		private void CreateDeviceIdAndLogin(ContinuanceToken continuanceToken)
		{
			ConnectInterface connectInterface = EOSManager.Instance.GetEOSPlatformInterface().GetConnectInterface();
			CreateDeviceIdOptions options = new CreateDeviceIdOptions
			{
				DeviceModel = "PC Windows 64bit"
			};
			connectInterface.CreateDeviceId(ref options, null, delegate(ref CreateDeviceIdCallbackInfo callback)
			{
				if (callback.ResultCode != Epic.OnlineServices.Result.Success)
				{
					EvilLogger.LogError($"[EOSAuth] Device ID creation failed: {callback.ResultCode}", "CreateDeviceIdAndLogin", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Networking\\EOS\\EOSAuthManager.cs", 223);
					FailLoginAttempt($"Device ID creation failed: {callback.ResultCode}");
				}
				else if (continuanceToken != null)
				{
					EOSManager.Instance.CreateConnectUserWithContinuanceToken(continuanceToken, OnCreateUserComplete);
				}
				else
				{
					EOSManager.Instance.StartConnectLoginWithDeviceToken(DisplayName, OnConnectLoginComplete);
				}
			});
		}

		private void OnConnectLoginComplete(LoginCallbackInfo loginCallbackInfo)
		{
			if (loginCallbackInfo.ResultCode == Epic.OnlineServices.Result.Success)
			{
				HandleLoginSuccess(loginCallbackInfo.LocalUserId);
				return;
			}
			if (loginCallbackInfo.ResultCode == Epic.OnlineServices.Result.InvalidUser)
			{
				EOSManager.Instance.CreateConnectUserWithContinuanceToken(loginCallbackInfo.ContinuanceToken, OnCreateUserComplete);
				return;
			}
			EvilLogger.LogError($"[EOSAuth] EOS Connect login failed: {loginCallbackInfo.ResultCode}", "OnConnectLoginComplete", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Networking\\EOS\\EOSAuthManager.cs", 254);
			FailLoginAttempt($"EOS Connect login failed: {loginCallbackInfo.ResultCode}");
		}

		private void OnCreateUserComplete(CreateUserCallbackInfo createUserCallbackInfo)
		{
			if (createUserCallbackInfo.ResultCode == Epic.OnlineServices.Result.Success)
			{
				HandleLoginSuccess(createUserCallbackInfo.LocalUserId);
				return;
			}
			EvilLogger.LogError($"[EOSAuth] Create user failed: {createUserCallbackInfo.ResultCode}", "OnCreateUserComplete", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Networking\\EOS\\EOSAuthManager.cs", 267);
			FailLoginAttempt($"Create user failed: {createUserCallbackInfo.ResultCode}");
		}

		private void HandleLoginSuccess(ProductUserId productUserId)
		{
			bool num = !IsLoggedIn;
			IsLoggedIn = true;
			LocalProductUserId = productUserId;
			EOSSDKComponent.DisplayName = DisplayName;
			if (num)
			{
				SubscribeAuthExpiration();
				this.OnLoginSuccess?.Invoke();
			}
			_loginAttemptTcs?.TrySetResult(result: true);
		}

		private void SubscribeAuthExpiration()
		{
			if (_authExpirationNotificationId != 0L)
			{
				return;
			}
			ConnectInterface connectInterface = EOSManager.Instance?.GetEOSPlatformInterface()?.GetConnectInterface();
			if (!(connectInterface == null))
			{
				AddNotifyAuthExpirationOptions options = default(AddNotifyAuthExpirationOptions);
				_authExpirationNotificationId = connectInterface.AddNotifyAuthExpiration(ref options, null, OnAuthExpirationNotification);
				if (_authExpirationNotificationId == 0L)
				{
					EvilLogger.LogError("[EOSAuth] Failed to subscribe to auth expiration notification.", "SubscribeAuthExpiration", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Networking\\EOS\\EOSAuthManager.cs", 314);
				}
			}
		}

		private void UnsubscribeAuthExpiration()
		{
			if (_authExpirationNotificationId != 0L)
			{
				(EOSManager.Instance?.GetEOSPlatformInterface()?.GetConnectInterface())?.RemoveNotifyAuthExpiration(_authExpirationNotificationId);
				_authExpirationNotificationId = 0uL;
			}
		}

		private void OnAuthExpirationNotification(ref AuthExpirationCallbackInfo data)
		{
			RefreshLoginAsync().Forget();
		}

		private async UniTask RefreshLoginAsync()
		{
			if (_isRefreshing)
			{
				return;
			}
			_isRefreshing = true;
			try
			{
				_telemetry?.RecordAttempt("login");
				NetworkRetry.Outcome<bool> outcome = await NetworkRetry.RunAsync(AttemptLoginOnceAsync, (bool success) => success, (bool _) => true, 3, 2000, 8000, 2f, 0.3f, this.GetCancellationTokenOnDestroy());
				if (outcome.Success)
				{
					_telemetry?.RecordSuccess("login", outcome.Attempts);
					return;
				}
				string resultCode = _lastLoginFailReason ?? "EOS Connect token refresh failed";
				EvilLogger.LogError("[EOSAuth] Refresh login failed after max attempts. Auth context will expire.", "RefreshLoginAsync", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Networking\\EOS\\EOSAuthManager.cs", 370);
				IsLoggedIn = false;
				_telemetry?.RecordFailure("login", NetworkErrorType.AuthenticationFailed, resultCode, outcome.Attempts, rateLimited: false);
				this.OnLoginFailed?.Invoke("EOS Connect token refresh failed");
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				EvilLogger.LogError("[EOSAuth] Refresh login threw: " + ex2.Message, "RefreshLoginAsync", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Networking\\EOS\\EOSAuthManager.cs", 381);
			}
			finally
			{
				_isRefreshing = false;
			}
		}

		public void Activate()
		{
			base.gameObject.SetActive(value: true);
		}

		public void Deactivate()
		{
			UnsubscribeAuthExpiration();
			base.gameObject.SetActive(value: false);
		}
	}
}
