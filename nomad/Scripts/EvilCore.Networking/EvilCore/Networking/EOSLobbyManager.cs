using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Cysharp.Threading.Tasks;
using Epic.OnlineServices;
using Epic.OnlineServices.Lobby;
using EpicTransport;
using EvilCore.EvilPack.EvilLogger;
using Steamworks;
using UnityEngine;
using VContainer;

namespace EvilCore.Networking
{
	public class EOSLobbyManager : MonoBehaviour, IEOSLobbyManager, IOnlineService
	{
		private string _currentLobbyId;

		private bool _playerInLobby;

		[Header("Mirror Networking")]
		public bool autoStartMirrorHost = true;

		[Header("Lobby Configuration")]
		[SerializeField]
		private string lobbyBucketId = "default";

		private bool _isOwner;

		private string _hostPuid;

		private string _joinCode;

		private bool _wasKicked;

		private readonly HashSet<string> _bannedMembers = new HashSet<string>();

		private ulong _lobbyMemberStatusNotifyId;

		private ulong _lobbyUpdateNotifyId;

		private ulong _lobbyInviteReceivedNotifyId;

		private ulong _lobbyInviteAcceptedNotifyId;

		private ulong _joinLobbyAcceptedNotifyId;

		private readonly Dictionary<string, string> _members = new Dictionary<string, string>();

		private LobbyCreateOptions _pendingOptions;

		[Inject]
		private IAuthService _authService;

		[Inject]
		private ISceneFlowManager _sceneFlowManager;

		[Inject]
		private INetworkErrorService _errorService;

		[Inject]
		private INetworkTelemetry _telemetry;

		[Inject]
		private IRegionService _regionService;

		[Inject]
		private IObjectResolver _container;

		private INetworkManager _networkManager;

		public int PendingWorldSeed => _pendingOptions.Seed;

		private INetworkManager NetworkManager => _networkManager ?? (_networkManager = _container.Resolve<INetworkManager>());

		string IEOSLobbyManager.CurrentLobbyId => _currentLobbyId;

		public bool IsInLobby => _playerInLobby;

		bool IEOSLobbyManager.PlayerInLobby => _playerInLobby;

		public bool IsOwner => _isOwner;

		string IEOSLobbyManager.JoinCode => _joinCode;

		bool IEOSLobbyManager.WasKicked => _wasKicked;

		string IEOSLobbyManager.LocalProductUserId => EOSSDKComponent.LocalUserProductIdString;

		public IReadOnlyDictionary<string, string> Members => _members;

		public event Action OnLobbyCreated;

		public event Action OnLobbyJoined;

		public event Action OnLobbyLeft;

		public event Action OnLobbyClosed;

		public event Action<string, string> OnMemberJoined;

		public event Action<string> OnMemberLeft;

		public void SetPendingWorldSeed(int seed)
		{
			_pendingOptions.Seed = seed;
		}

		void IEOSLobbyManager.ConsumeKickedFlag()
		{
			_wasKicked = false;
		}

		private LobbyInterface GetLobbyInterface()
		{
			return EOSSDKComponent.GetLobbyInterface();
		}

		private void Awake()
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}

		private void OnEnable()
		{
			SubscribeToNotifications();
		}

		private void OnDisable()
		{
			UnsubscribeFromNotifications();
		}

		private void SubscribeToNotifications()
		{
			LobbyInterface lobbyInterface = GetLobbyInterface();
			if (!(lobbyInterface == null))
			{
				AddNotifyLobbyMemberStatusReceivedOptions options = default(AddNotifyLobbyMemberStatusReceivedOptions);
				_lobbyMemberStatusNotifyId = lobbyInterface.AddNotifyLobbyMemberStatusReceived(ref options, null, OnLobbyMemberStatusReceived);
				AddNotifyLobbyUpdateReceivedOptions options2 = default(AddNotifyLobbyUpdateReceivedOptions);
				_lobbyUpdateNotifyId = lobbyInterface.AddNotifyLobbyUpdateReceived(ref options2, null, OnLobbyUpdateReceived);
				AddNotifyLobbyInviteReceivedOptions options3 = default(AddNotifyLobbyInviteReceivedOptions);
				_lobbyInviteReceivedNotifyId = lobbyInterface.AddNotifyLobbyInviteReceived(ref options3, null, OnLobbyInviteReceived);
				AddNotifyLobbyInviteAcceptedOptions options4 = default(AddNotifyLobbyInviteAcceptedOptions);
				_lobbyInviteAcceptedNotifyId = lobbyInterface.AddNotifyLobbyInviteAccepted(ref options4, null, OnLobbyInviteAccepted);
				AddNotifyJoinLobbyAcceptedOptions options5 = default(AddNotifyJoinLobbyAcceptedOptions);
				_joinLobbyAcceptedNotifyId = lobbyInterface.AddNotifyJoinLobbyAccepted(ref options5, null, OnJoinLobbyAccepted);
			}
		}

		private void UnsubscribeFromNotifications()
		{
			LobbyInterface lobbyInterface = GetLobbyInterface();
			if (!(lobbyInterface == null))
			{
				if (_lobbyMemberStatusNotifyId != 0L)
				{
					lobbyInterface.RemoveNotifyLobbyMemberStatusReceived(_lobbyMemberStatusNotifyId);
				}
				if (_lobbyUpdateNotifyId != 0L)
				{
					lobbyInterface.RemoveNotifyLobbyUpdateReceived(_lobbyUpdateNotifyId);
				}
				if (_lobbyInviteReceivedNotifyId != 0L)
				{
					lobbyInterface.RemoveNotifyLobbyInviteReceived(_lobbyInviteReceivedNotifyId);
				}
				if (_lobbyInviteAcceptedNotifyId != 0L)
				{
					lobbyInterface.RemoveNotifyLobbyInviteAccepted(_lobbyInviteAcceptedNotifyId);
				}
				if (_joinLobbyAcceptedNotifyId != 0L)
				{
					lobbyInterface.RemoveNotifyJoinLobbyAccepted(_joinLobbyAcceptedNotifyId);
				}
			}
		}

		private void OnLobbyMemberStatusReceived(ref LobbyMemberStatusReceivedCallbackInfo data)
		{
			string text = data.TargetUserId?.ToString() ?? "unknown";
			switch (data.CurrentStatus)
			{
			case LobbyMemberStatus.Joined:
				if (_isOwner && IsBanned(text))
				{
					KickMember(text);
				}
				else
				{
					AddMember(text, ResolveMemberDisplayName(text));
				}
				break;
			case LobbyMemberStatus.Left:
				RemoveMember(text);
				break;
			case LobbyMemberStatus.Disconnected:
				RemoveMember(text);
				break;
			case LobbyMemberStatus.Kicked:
			{
				string localUserProductIdString = EOSSDKComponent.LocalUserProductIdString;
				if (text == localUserProductIdString)
				{
					_wasKicked = true;
					this.OnLobbyLeft?.Invoke();
					_playerInLobby = false;
					_currentLobbyId = null;
					NetworkManager.Disconnect();
				}
				RemoveMember(text);
				break;
			}
			case LobbyMemberStatus.Closed:
			{
				bool flag = _playerInLobby && !_isOwner;
				ClearMembers();
				_playerInLobby = false;
				_currentLobbyId = null;
				if (!_isOwner)
				{
					if (flag)
					{
						_errorService?.Report(NetworkErrorType.ConnectionLost, "Host closed the lobby");
					}
					NetworkManager.Disconnect();
				}
				this.OnLobbyClosed?.Invoke();
				break;
			}
			case LobbyMemberStatus.Promoted:
				break;
			}
		}

		private void OnLobbyUpdateReceived(ref LobbyUpdateReceivedCallbackInfo data)
		{
		}

		private void OnLobbyInviteReceived(ref LobbyInviteReceivedCallbackInfo data)
		{
		}

		private void OnLobbyInviteAccepted(ref LobbyInviteAcceptedCallbackInfo data)
		{
		}

		private void OnJoinLobbyAccepted(ref JoinLobbyAcceptedCallbackInfo data)
		{
		}

		private void CreateLobbyAsync(LobbyCreateOptions options)
		{
			if (!EOSSDKComponent.Initialized)
			{
				EvilLogger.LogError("[EOSLobby] EOS not initialized, cannot create lobby.", "CreateLobbyAsync", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Networking\\EOS\\EOSLobbyManager.cs", 219);
				_errorService?.Report(NetworkErrorType.Unknown, "EOS not initialized");
			}
			else
			{
				CreateLobbyWithRetryAsync(options).Forget();
			}
		}

		private async UniTaskVoid CreateLobbyWithRetryAsync(LobbyCreateOptions options)
		{
			_telemetry?.RecordAttempt("lobby_create");
			await _regionService.ResolveLocalRegionAsync();
			CreateLobbyOptions createOptions = new CreateLobbyOptions
			{
				LocalUserId = EOSSDKComponent.LocalUserProductId,
				MaxLobbyMembers = options.MaxPlayers,
				PermissionLevel = LobbyPermissionLevel.Publicadvertised,
				BucketId = lobbyBucketId,
				PresenceEnabled = true,
				AllowInvites = true,
				EnableRTCRoom = true
			};
			NetworkRetry.Outcome<CreateLobbyCallbackInfo> outcome = await NetworkRetry.RunAsync(() => CreateLobbyOnceAsync(createOptions), (CreateLobbyCallbackInfo info) => info.ResultCode == Epic.OnlineServices.Result.Success, (CreateLobbyCallbackInfo info) => IsTransientEosResult(info.ResultCode), 3, 600, 5000, 2f, 0.3f, this.GetCancellationTokenOnDestroy());
			if (!outcome.Success)
			{
				Epic.OnlineServices.Result resultCode = outcome.Result.ResultCode;
				bool flag = IsRateLimitEosResult(resultCode);
				NetworkErrorType type = (flag ? NetworkErrorType.RateLimited : NetworkErrorType.LobbyCreateFailed);
				EvilLogger.LogError($"[EOSLobby] Failed to create lobby after {outcome.Attempts} attempt(s): {resultCode}", "CreateLobbyWithRetryAsync", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Networking\\EOS\\EOSLobbyManager.cs", 267);
				_telemetry?.RecordFailure("lobby_create", type, resultCode.ToString(), outcome.Attempts, flag);
				_errorService?.Report(type, resultCode.ToString());
			}
			else
			{
				_telemetry?.RecordSuccess("lobby_create", outcome.Attempts);
				HandleCreateLobbySuccess(outcome.Result);
			}
		}

		private UniTask<CreateLobbyCallbackInfo> CreateLobbyOnceAsync(CreateLobbyOptions createOptions)
		{
			UniTaskCompletionSource<CreateLobbyCallbackInfo> tcs = new UniTaskCompletionSource<CreateLobbyCallbackInfo>();
			GetLobbyInterface().CreateLobby(ref createOptions, null, delegate(ref CreateLobbyCallbackInfo info)
			{
				tcs.TrySetResult(info);
			});
			return tcs.Task;
		}

		private void HandleCreateLobbySuccess(CreateLobbyCallbackInfo data)
		{
			try
			{
				_currentLobbyId = data.LobbyId;
				_playerInLobby = true;
				_isOwner = true;
				_joinCode = GenerateJoinCode();
				SetLobbyAttributes(_currentLobbyId);
				SetMemberDisplayName(_currentLobbyId);
				RefreshMembers(_currentLobbyId);
				this.OnLobbyCreated?.Invoke();
				if (autoStartMirrorHost)
				{
					StartMirrorHostAsync().Forget();
				}
			}
			catch (Exception ex)
			{
				EvilLogger.LogError("[EOSLobby] Exception while completing lobby creation: " + ex.Message, "HandleCreateLobbySuccess", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Networking\\EOS\\EOSLobbyManager.cs", 309);
				_errorService?.Report(NetworkErrorType.Unknown, ex.Message);
			}
		}

		private static bool IsTransientEosResult(Epic.OnlineServices.Result r)
		{
			if (r != Epic.OnlineServices.Result.TimedOut && r != Epic.OnlineServices.Result.NoConnection && r != Epic.OnlineServices.Result.TooManyRequests)
			{
				return r == Epic.OnlineServices.Result.ServiceFailure;
			}
			return true;
		}

		private static bool IsRateLimitEosResult(Epic.OnlineServices.Result r)
		{
			return r == Epic.OnlineServices.Result.TooManyRequests;
		}

		private void SetLobbyAttributes(string lobbyId)
		{
			UpdateLobbyModificationOptions options = new UpdateLobbyModificationOptions
			{
				LobbyId = lobbyId,
				LocalUserId = EOSSDKComponent.LocalUserProductId
			};
			LobbyModification outLobbyModificationHandle;
			Epic.OnlineServices.Result result = GetLobbyInterface().UpdateLobbyModification(ref options, out outLobbyModificationHandle);
			if (result != Epic.OnlineServices.Result.Success)
			{
				EvilLogger.LogError($"[EOSLobby] Failed to get lobby modification handle: {result}", "SetLobbyAttributes", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Networking\\EOS\\EOSLobbyManager.cs", 335);
				return;
			}
			AddAttribute(outLobbyModificationHandle, "host_puid", EOSSDKComponent.LocalUserProductIdString);
			AddAttribute(outLobbyModificationHandle, "version", Application.version);
			AddAttribute(outLobbyModificationHandle, "game_mode", "default");
			AddAttribute(outLobbyModificationHandle, "region", _regionService.ToAttribute(_regionService.LocalRegion));
			AddAttribute(outLobbyModificationHandle, "is_public", _pendingOptions.IsPublic ? "1" : "0");
			AddAttribute(outLobbyModificationHandle, "lobby_name", _pendingOptions.LobbyName ?? "Unnamed Game");
			AddAttribute(outLobbyModificationHandle, "has_password", string.IsNullOrEmpty(_pendingOptions.Password) ? "0" : "1");
			if (!string.IsNullOrEmpty(_joinCode))
			{
				AddAttribute(outLobbyModificationHandle, "join_code", _joinCode);
			}
			if (!string.IsNullOrEmpty(_pendingOptions.Password))
			{
				string value = ComputeSHA256(_pendingOptions.Password);
				AddAttribute(outLobbyModificationHandle, "password_hash", value);
			}
			UpdateLobbyOptions options2 = new UpdateLobbyOptions
			{
				LobbyModificationHandle = outLobbyModificationHandle
			};
			GetLobbyInterface().UpdateLobby(ref options2, null, delegate(ref UpdateLobbyCallbackInfo updateCallback)
			{
				if (updateCallback.ResultCode != Epic.OnlineServices.Result.Success)
				{
					EvilLogger.LogError($"[EOSLobby] Failed to update lobby attributes: {updateCallback.ResultCode}", "SetLobbyAttributes", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Networking\\EOS\\EOSLobbyManager.cs", 368);
				}
			});
			outLobbyModificationHandle.Release();
		}

		private void SetMemberDisplayName(string lobbyId)
		{
			string text = _authService?.DisplayName ?? "Player";
			UpdateLobbyModificationOptions options = new UpdateLobbyModificationOptions
			{
				LobbyId = lobbyId,
				LocalUserId = EOSSDKComponent.LocalUserProductId
			};
			if (GetLobbyInterface().UpdateLobbyModification(ref options, out var outLobbyModificationHandle) != Epic.OnlineServices.Result.Success)
			{
				return;
			}
			AttributeData value = new AttributeData
			{
				Key = "display_name",
				Value = new AttributeDataValue
				{
					AsUtf8 = text
				}
			};
			LobbyModificationAddMemberAttributeOptions options2 = new LobbyModificationAddMemberAttributeOptions
			{
				Attribute = value,
				Visibility = LobbyAttributeVisibility.Public
			};
			outLobbyModificationHandle.AddMemberAttribute(ref options2);
			UpdateLobbyOptions options3 = new UpdateLobbyOptions
			{
				LobbyModificationHandle = outLobbyModificationHandle
			};
			GetLobbyInterface().UpdateLobby(ref options3, null, delegate(ref UpdateLobbyCallbackInfo cb)
			{
				if (cb.ResultCode != Epic.OnlineServices.Result.Success)
				{
					EvilLogger.LogError($"[EOSLobby] Failed to set member display name: {cb.ResultCode}", "SetMemberDisplayName", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Networking\\EOS\\EOSLobbyManager.cs", 410);
				}
			});
			outLobbyModificationHandle.Release();
		}

		private void RefreshMembers(string lobbyId)
		{
			ClearMembers();
			CopyLobbyDetailsHandleOptions options = new CopyLobbyDetailsHandleOptions
			{
				LobbyId = lobbyId,
				LocalUserId = EOSSDKComponent.LocalUserProductId
			};
			if (GetLobbyInterface().CopyLobbyDetailsHandle(ref options, out var outLobbyDetailsHandle) != Epic.OnlineServices.Result.Success || outLobbyDetailsHandle == null)
			{
				return;
			}
			LobbyDetailsGetMemberCountOptions options2 = default(LobbyDetailsGetMemberCountOptions);
			uint memberCount = outLobbyDetailsHandle.GetMemberCount(ref options2);
			for (uint num = 0u; num < memberCount; num++)
			{
				LobbyDetailsGetMemberByIndexOptions options3 = new LobbyDetailsGetMemberByIndexOptions
				{
					MemberIndex = num
				};
				ProductUserId memberByIndex = outLobbyDetailsHandle.GetMemberByIndex(ref options3);
				if (!(memberByIndex == null))
				{
					string memberId = memberByIndex.ToString();
					string memberDisplayName = GetMemberDisplayName(outLobbyDetailsHandle, memberByIndex);
					AddMember(memberId, memberDisplayName);
				}
			}
			outLobbyDetailsHandle.Release();
		}

		private string GetMemberDisplayName(LobbyDetails details, ProductUserId memberId)
		{
			LobbyDetailsCopyMemberAttributeByKeyOptions options = new LobbyDetailsCopyMemberAttributeByKeyOptions
			{
				TargetUserId = memberId,
				AttrKey = "display_name"
			};
			if (details.CopyMemberAttributeByKey(ref options, out var outAttribute) == Epic.OnlineServices.Result.Success && outAttribute.HasValue)
			{
				return outAttribute.Value.Data?.Value.AsUtf8 ?? ((Utf8String)memberId.ToString());
			}
			if (SteamClient.IsValid)
			{
				string localUserProductIdString = EOSSDKComponent.LocalUserProductIdString;
				if (memberId.ToString() == localUserProductIdString)
				{
					return SteamClient.Name;
				}
			}
			return memberId.ToString();
		}

		private string ResolveMemberDisplayName(string memberId)
		{
			if (string.IsNullOrEmpty(_currentLobbyId))
			{
				return memberId;
			}
			CopyLobbyDetailsHandleOptions options = new CopyLobbyDetailsHandleOptions
			{
				LobbyId = _currentLobbyId,
				LocalUserId = EOSSDKComponent.LocalUserProductId
			};
			if (GetLobbyInterface().CopyLobbyDetailsHandle(ref options, out var outLobbyDetailsHandle) != Epic.OnlineServices.Result.Success || outLobbyDetailsHandle == null)
			{
				return memberId;
			}
			ProductUserId productUserId = ProductUserId.FromString(memberId);
			string result = ((productUserId != null) ? GetMemberDisplayName(outLobbyDetailsHandle, productUserId) : memberId);
			outLobbyDetailsHandle.Release();
			return result;
		}

		private void AddMember(string memberId, string displayName = null)
		{
			if (!_members.ContainsKey(memberId))
			{
				string text = displayName ?? memberId;
				_members[memberId] = text;
				this.OnMemberJoined?.Invoke(memberId, text);
			}
		}

		private void RemoveMember(string memberId)
		{
			if (_members.Remove(memberId))
			{
				this.OnMemberLeft?.Invoke(memberId);
			}
		}

		private void ClearMembers()
		{
			_members.Clear();
		}

		private void AddAttribute(LobbyModification modHandle, string key, string value)
		{
			AttributeData value2 = new AttributeData
			{
				Key = key,
				Value = new AttributeDataValue
				{
					AsUtf8 = value
				}
			};
			LobbyModificationAddAttributeOptions options = new LobbyModificationAddAttributeOptions
			{
				Attribute = value2,
				Visibility = LobbyAttributeVisibility.Public
			};
			Epic.OnlineServices.Result result = modHandle.AddAttribute(ref options);
			if (result != Epic.OnlineServices.Result.Success)
			{
				EvilLogger.LogError($"[EOSLobby] Failed to add attribute '{key}': {result}", "AddAttribute", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Networking\\EOS\\EOSLobbyManager.cs", 528);
			}
		}

		private async UniTaskVoid StartMirrorHostAsync()
		{
			await _sceneFlowManager.TransitionToGameAsync();
			await UniTask.Delay(500, ignoreTimeScale: false, PlayerLoopTiming.Update, this.GetCancellationTokenOnDestroy());
			NetworkManager.StartHost();
		}

		public void CreateGameWithLobby()
		{
			CreateGameWithLobby(new LobbyCreateOptions
			{
				LobbyName = "Default Game",
				MaxPlayers = 4u,
				IsPublic = true,
				Password = null
			});
		}

		public void CreateGameWithLobby(LobbyCreateOptions options)
		{
			_pendingOptions = options;
			_wasKicked = false;
			CreateLobbyAsync(options);
		}

		public void JoinLobbyById(string lobbyId)
		{
			if (!EOSSDKComponent.Initialized)
			{
				EvilLogger.LogError("[EOSLobby] EOS not initialized, cannot join lobby.", "JoinLobbyById", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Networking\\EOS\\EOSLobbyManager.cs", 564);
				_errorService?.Report(NetworkErrorType.Unknown, "EOS not initialized");
			}
			else
			{
				JoinLobbyByIdWithRetryAsync(lobbyId).Forget();
			}
		}

		private async UniTaskVoid JoinLobbyByIdWithRetryAsync(string lobbyId)
		{
			_wasKicked = false;
			_telemetry?.RecordAttempt("lobby_join");
			NetworkRetry.Outcome<JoinLobbyByIdCallbackInfo> outcome = await NetworkRetry.RunAsync(() => JoinLobbyOnceAsync(lobbyId), (JoinLobbyByIdCallbackInfo info) => info.ResultCode == Epic.OnlineServices.Result.Success, (JoinLobbyByIdCallbackInfo info) => IsTransientEosResult(info.ResultCode), 3, 600, 5000, 2f, 0.3f, this.GetCancellationTokenOnDestroy());
			if (!outcome.Success)
			{
				Epic.OnlineServices.Result resultCode = outcome.Result.ResultCode;
				bool flag = IsRateLimitEosResult(resultCode);
				NetworkErrorType type = (flag ? NetworkErrorType.RateLimited : ClassifyJoinResult(resultCode));
				EvilLogger.LogError($"[EOSLobby] Failed to join lobby after {outcome.Attempts} attempt(s): {resultCode}", "JoinLobbyByIdWithRetryAsync", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Networking\\EOS\\EOSLobbyManager.cs", 592);
				_telemetry?.RecordFailure("lobby_join", type, resultCode.ToString(), outcome.Attempts, flag);
				_errorService?.Report(type, resultCode.ToString());
			}
			else
			{
				_telemetry?.RecordSuccess("lobby_join", outcome.Attempts);
				HandleJoinLobbySuccess(outcome.Result);
			}
		}

		private UniTask<JoinLobbyByIdCallbackInfo> JoinLobbyOnceAsync(string lobbyId)
		{
			JoinLobbyByIdOptions options = new JoinLobbyByIdOptions
			{
				LobbyId = lobbyId,
				LocalUserId = EOSSDKComponent.LocalUserProductId,
				PresenceEnabled = true
			};
			UniTaskCompletionSource<JoinLobbyByIdCallbackInfo> tcs = new UniTaskCompletionSource<JoinLobbyByIdCallbackInfo>();
			GetLobbyInterface().JoinLobbyById(ref options, null, delegate(ref JoinLobbyByIdCallbackInfo info)
			{
				tcs.TrySetResult(info);
			});
			return tcs.Task;
		}

		private void HandleJoinLobbySuccess(JoinLobbyByIdCallbackInfo data)
		{
			try
			{
				_currentLobbyId = data.LobbyId;
				_playerInLobby = true;
				_isOwner = false;
				SetMemberDisplayName(_currentLobbyId);
				RefreshMembers(_currentLobbyId);
				ReadHostPuidAndConnect(_currentLobbyId);
				this.OnLobbyJoined?.Invoke();
			}
			catch (Exception ex)
			{
				EvilLogger.LogError("[EOSLobby] Exception while completing lobby join: " + ex.Message, "HandleJoinLobbySuccess", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Networking\\EOS\\EOSLobbyManager.cs", 635);
				_errorService?.Report(NetworkErrorType.Unknown, ex.Message);
			}
		}

		private static NetworkErrorType ClassifyJoinResult(Epic.OnlineServices.Result result)
		{
			if (result != Epic.OnlineServices.Result.NotFound)
			{
				return NetworkErrorType.LobbyJoinFailed;
			}
			return NetworkErrorType.LobbyNotFound;
		}

		private void ReadHostPuidAndConnect(string lobbyId)
		{
			CopyLobbyDetailsHandleOptions options = new CopyLobbyDetailsHandleOptions
			{
				LobbyId = lobbyId,
				LocalUserId = EOSSDKComponent.LocalUserProductId
			};
			Epic.OnlineServices.Result result = GetLobbyInterface().CopyLobbyDetailsHandle(ref options, out var outLobbyDetailsHandle);
			if (result != Epic.OnlineServices.Result.Success || outLobbyDetailsHandle == null)
			{
				EvilLogger.LogError($"[EOSLobby] Failed to get lobby details: {result}", "ReadHostPuidAndConnect", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Networking\\EOS\\EOSLobbyManager.cs", 658);
				_errorService?.Report(NetworkErrorType.LobbyJoinFailed, $"CopyLobbyDetailsHandle: {result}");
				return;
			}
			_joinCode = ReadLobbyAttribute(outLobbyDetailsHandle, "join_code");
			string text = ReadLobbyAttribute(outLobbyDetailsHandle, "version");
			if (!IsCompatibleVersion(text))
			{
				outLobbyDetailsHandle.Release();
				_errorService?.Report(NetworkErrorType.LobbyJoinFailed, "version mismatch (lobby " + text + " vs " + Application.version + ")");
				return;
			}
			LobbyDetailsCopyAttributeByKeyOptions options2 = new LobbyDetailsCopyAttributeByKeyOptions
			{
				AttrKey = "host_puid"
			};
			result = outLobbyDetailsHandle.CopyAttributeByKey(ref options2, out var outAttribute);
			if (result == Epic.OnlineServices.Result.Success && outAttribute.HasValue)
			{
				_hostPuid = outAttribute.Value.Data?.Value.AsUtf8;
				ConnectToHostAsync(_hostPuid).Forget();
			}
			else
			{
				EvilLogger.LogError($"[EOSLobby] Failed to read host_puid attribute: {result}", "ReadHostPuidAndConnect", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Networking\\EOS\\EOSLobbyManager.cs", 695);
				_errorService?.Report(NetworkErrorType.LobbyJoinFailed, $"host_puid: {result}");
			}
			outLobbyDetailsHandle.Release();
		}

		private async UniTaskVoid ConnectToHostAsync(string hostPuid)
		{
			await _sceneFlowManager.TransitionToGameAsync();
			await UniTask.Delay(1000, ignoreTimeScale: false, PlayerLoopTiming.Update, this.GetCancellationTokenOnDestroy());
			NetworkManager.NetworkAddress = hostPuid;
			NetworkManager.StartClient();
		}

		public void LeaveLobby()
		{
			if (string.IsNullOrEmpty(_currentLobbyId))
			{
				return;
			}
			if (_isOwner)
			{
				DestroyLobbyOptions options = new DestroyLobbyOptions
				{
					LocalUserId = EOSSDKComponent.LocalUserProductId,
					LobbyId = _currentLobbyId
				};
				GetLobbyInterface().DestroyLobby(ref options, null, delegate(ref DestroyLobbyCallbackInfo destroyCallback)
				{
					if (destroyCallback.ResultCode != Epic.OnlineServices.Result.Success)
					{
						EvilLogger.LogError($"[EOSLobby] Failed to destroy lobby: {destroyCallback.ResultCode}", "LeaveLobby", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Networking\\EOS\\EOSLobbyManager.cs", 735);
					}
				});
			}
			else
			{
				LeaveLobbyOptions options2 = new LeaveLobbyOptions
				{
					LocalUserId = EOSSDKComponent.LocalUserProductId,
					LobbyId = _currentLobbyId
				};
				GetLobbyInterface().LeaveLobby(ref options2, null, delegate(ref LeaveLobbyCallbackInfo leaveCallback)
				{
					if (leaveCallback.ResultCode != Epic.OnlineServices.Result.Success && leaveCallback.ResultCode != Epic.OnlineServices.Result.NotFound)
					{
						EvilLogger.LogError($"[EOSLobby] Failed to leave lobby: {leaveCallback.ResultCode}", "LeaveLobby", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Networking\\EOS\\EOSLobbyManager.cs", 753);
					}
				});
			}
			ClearMembers();
			_bannedMembers.Clear();
			_playerInLobby = false;
			_currentLobbyId = null;
			_isOwner = false;
			_hostPuid = null;
			_joinCode = null;
			this.OnLobbyLeft?.Invoke();
		}

		private string GenerateJoinCode()
		{
			byte[] array = new byte[5];
			using RNGCryptoServiceProvider rNGCryptoServiceProvider = new RNGCryptoServiceProvider();
			rNGCryptoServiceProvider.GetBytes(array);
			char[] array2 = new char[5];
			for (int i = 0; i < 5; i++)
			{
				array2[i] = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"[array[i] % "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".Length];
			}
			return new string(array2);
		}

		public void SearchLobbyByCode(string code, Action<LobbySearchResult?> callback)
		{
			if (!EOSSDKComponent.Initialized)
			{
				EvilLogger.LogError("[EOSLobby] EOS not initialized, cannot search by code.", "SearchLobbyByCode", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Networking\\EOS\\EOSLobbyManager.cs", 784);
				callback?.Invoke(null);
				return;
			}
			CreateLobbySearchOptions options = new CreateLobbySearchOptions
			{
				MaxResults = 1u
			};
			LobbySearch search;
			Epic.OnlineServices.Result result = GetLobbyInterface().CreateLobbySearch(ref options, out search);
			if (result != Epic.OnlineServices.Result.Success)
			{
				EvilLogger.LogError($"[EOSLobby] Failed to create lobby search: {result}", "SearchLobbyByCode", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Networking\\EOS\\EOSLobbyManager.cs", 793);
				callback?.Invoke(null);
				return;
			}
			LobbySearchSetParameterOptions options2 = new LobbySearchSetParameterOptions
			{
				Parameter = new AttributeData
				{
					Key = "join_code",
					Value = new AttributeDataValue
					{
						AsUtf8 = code.ToUpperInvariant()
					}
				},
				ComparisonOp = ComparisonOp.Equal
			};
			search.SetParameter(ref options2);
			LobbySearchFindOptions options3 = new LobbySearchFindOptions
			{
				LocalUserId = EOSSDKComponent.LocalUserProductId
			};
			search.Find(ref options3, null, delegate(ref LobbySearchFindCallbackInfo findCallback)
			{
				if (findCallback.ResultCode == Epic.OnlineServices.Result.Success)
				{
					LobbySearchGetSearchResultCountOptions options4 = default(LobbySearchGetSearchResultCountOptions);
					if (search.GetSearchResultCount(ref options4) != 0)
					{
						LobbySearchCopySearchResultByIndexOptions options5 = new LobbySearchCopySearchResultByIndexOptions
						{
							LobbyIndex = 0u
						};
						if (search.CopySearchResultByIndex(ref options5, out var outLobbyDetailsHandle) == Epic.OnlineServices.Result.Success)
						{
							LobbySearchResult value = ParseLobbyDetails(outLobbyDetailsHandle);
							outLobbyDetailsHandle.Release();
							search.Release();
							callback?.Invoke(value);
							return;
						}
					}
				}
				else
				{
					EvilLogger.LogError($"[EOSLobby] Search by code failed: {findCallback.ResultCode}", "SearchLobbyByCode", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Networking\\EOS\\EOSLobbyManager.cs", 835);
				}
				search.Release();
				callback?.Invoke(null);
			});
		}

		public void KickMember(string memberId)
		{
			if (!_isOwner || string.IsNullOrEmpty(_currentLobbyId))
			{
				return;
			}
			ProductUserId productUserId = ProductUserId.FromString(memberId);
			if (productUserId == null)
			{
				EvilLogger.LogError("[EOSLobby] Invalid member id for kick: " + memberId, "KickMember", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Networking\\EOS\\EOSLobbyManager.cs", 854);
				return;
			}
			KickMemberOptions options = new KickMemberOptions
			{
				LobbyId = _currentLobbyId,
				LocalUserId = EOSSDKComponent.LocalUserProductId,
				TargetUserId = productUserId
			};
			GetLobbyInterface().KickMember(ref options, null, delegate(ref KickMemberCallbackInfo cb)
			{
				if (cb.ResultCode == Epic.OnlineServices.Result.Success)
				{
					NetworkManager.DisconnectByAddress(memberId);
				}
				else if (cb.ResultCode == Epic.OnlineServices.Result.NotFound)
				{
					NetworkManager.DisconnectByAddress(memberId);
				}
				else
				{
					EvilLogger.LogError($"[EOSLobby] Failed to kick member {memberId}: {cb.ResultCode}", "KickMember", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Networking\\EOS\\EOSLobbyManager.cs", 882);
				}
			});
		}

		public void BanMember(string memberId)
		{
			_bannedMembers.Add(memberId);
			KickMember(memberId);
			UpdateBannedMembersAttribute();
		}

		private void UpdateBannedMembersAttribute()
		{
			if (string.IsNullOrEmpty(_currentLobbyId) || !_isOwner)
			{
				return;
			}
			UpdateLobbyModificationOptions options = new UpdateLobbyModificationOptions
			{
				LobbyId = _currentLobbyId,
				LocalUserId = EOSSDKComponent.LocalUserProductId
			};
			LobbyModification outLobbyModificationHandle;
			Epic.OnlineServices.Result result = GetLobbyInterface().UpdateLobbyModification(ref options, out outLobbyModificationHandle);
			if (result != Epic.OnlineServices.Result.Success)
			{
				EvilLogger.LogError($"[EOSLobby] Failed to get lobby modification handle for ban list: {result}", "UpdateBannedMembersAttribute", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Networking\\EOS\\EOSLobbyManager.cs", 907);
				return;
			}
			string value = string.Join(",", _bannedMembers);
			AddAttribute(outLobbyModificationHandle, "banned_members", value);
			UpdateLobbyOptions options2 = new UpdateLobbyOptions
			{
				LobbyModificationHandle = outLobbyModificationHandle
			};
			GetLobbyInterface().UpdateLobby(ref options2, null, delegate(ref UpdateLobbyCallbackInfo cb)
			{
				if (cb.ResultCode != Epic.OnlineServices.Result.Success)
				{
					EvilLogger.LogError($"[EOSLobby] Failed to update banned members attribute: {cb.ResultCode}", "UpdateBannedMembersAttribute", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Networking\\EOS\\EOSLobbyManager.cs", 923);
				}
			});
			outLobbyModificationHandle.Release();
		}

		public bool IsBanned(string memberId)
		{
			return _bannedMembers.Contains(memberId);
		}

		public void SearchLobbies(Action<List<LobbySearchResult>> callback = null, int maxResults = 50)
		{
			CreateLobbySearchOptions options = new CreateLobbySearchOptions
			{
				MaxResults = (uint)maxResults
			};
			LobbySearch search;
			Epic.OnlineServices.Result result = GetLobbyInterface().CreateLobbySearch(ref options, out search);
			if (result != Epic.OnlineServices.Result.Success)
			{
				EvilLogger.LogError($"[EOSLobby] Failed to create lobby search: {result}", "SearchLobbies", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Networking\\EOS\\EOSLobbyManager.cs", 941);
				callback?.Invoke(new List<LobbySearchResult>());
				return;
			}
			LobbySearchSetParameterOptions options2 = new LobbySearchSetParameterOptions
			{
				Parameter = new AttributeData
				{
					Key = "game_mode",
					Value = new AttributeDataValue
					{
						AsUtf8 = "default"
					}
				},
				ComparisonOp = ComparisonOp.Equal
			};
			search.SetParameter(ref options2);
			LobbySearchSetParameterOptions options3 = new LobbySearchSetParameterOptions
			{
				Parameter = new AttributeData
				{
					Key = "is_public",
					Value = new AttributeDataValue
					{
						AsUtf8 = "1"
					}
				},
				ComparisonOp = ComparisonOp.Equal
			};
			search.SetParameter(ref options3);
			LobbySearchSetParameterOptions options4 = new LobbySearchSetParameterOptions
			{
				Parameter = new AttributeData
				{
					Key = "version",
					Value = new AttributeDataValue
					{
						AsUtf8 = Application.version
					}
				},
				ComparisonOp = ComparisonOp.Equal
			};
			search.SetParameter(ref options4);
			LobbySearchFindOptions options5 = new LobbySearchFindOptions
			{
				LocalUserId = EOSSDKComponent.LocalUserProductId
			};
			search.Find(ref options5, null, delegate(ref LobbySearchFindCallbackInfo findCallback)
			{
				List<LobbySearchResult> list = new List<LobbySearchResult>();
				if (findCallback.ResultCode == Epic.OnlineServices.Result.Success)
				{
					LobbySearchGetSearchResultCountOptions options6 = default(LobbySearchGetSearchResultCountOptions);
					uint searchResultCount = search.GetSearchResultCount(ref options6);
					for (uint num = 0u; num < searchResultCount; num++)
					{
						LobbySearchCopySearchResultByIndexOptions options7 = new LobbySearchCopySearchResultByIndexOptions
						{
							LobbyIndex = num
						};
						if (search.CopySearchResultByIndex(ref options7, out var outLobbyDetailsHandle) == Epic.OnlineServices.Result.Success)
						{
							LobbySearchResult item = ParseLobbyDetails(outLobbyDetailsHandle);
							outLobbyDetailsHandle.Release();
							if (item.IsPublic)
							{
								list.Add(item);
							}
						}
					}
				}
				else
				{
					EvilLogger.LogError($"[EOSLobby] Lobby search failed: {findCallback.ResultCode}", "SearchLobbies", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Networking\\EOS\\EOSLobbyManager.cs", 1022);
				}
				search.Release();
				callback?.Invoke(list);
			});
		}

		private LobbySearchResult ParseLobbyDetails(LobbyDetails details)
		{
			LobbySearchResult result = default(LobbySearchResult);
			LobbyDetailsCopyInfoOptions options = default(LobbyDetailsCopyInfoOptions);
			if (details.CopyInfo(ref options, out var outLobbyDetailsInfo) == Epic.OnlineServices.Result.Success && outLobbyDetailsInfo.HasValue)
			{
				result.LobbyId = outLobbyDetailsInfo.Value.LobbyId;
				result.MaxMembers = outLobbyDetailsInfo.Value.MaxMembers;
			}
			LobbyDetailsGetMemberCountOptions options2 = default(LobbyDetailsGetMemberCountOptions);
			result.MemberCount = details.GetMemberCount(ref options2);
			result.HostPuid = ReadLobbyAttribute(details, "host_puid");
			result.Version = ReadLobbyAttribute(details, "version");
			result.GameMode = ReadLobbyAttribute(details, "game_mode");
			result.Region = _regionService.FromAttribute(ReadLobbyAttribute(details, "region"));
			result.IsPublic = ReadLobbyAttribute(details, "is_public") != "0";
			result.LobbyName = ReadLobbyAttribute(details, "lobby_name") ?? "Unnamed";
			result.HasPassword = ReadLobbyAttribute(details, "has_password") == "1";
			result.PasswordHash = ReadLobbyAttribute(details, "password_hash") ?? "";
			result.JoinCode = ReadLobbyAttribute(details, "join_code") ?? "";
			result.BannedMembers = ReadLobbyAttribute(details, "banned_members") ?? "";
			return result;
		}

		private string ReadLobbyAttribute(LobbyDetails details, string key)
		{
			LobbyDetailsCopyAttributeByKeyOptions options = new LobbyDetailsCopyAttributeByKeyOptions
			{
				AttrKey = key
			};
			if (details.CopyAttributeByKey(ref options, out var outAttribute) == Epic.OnlineServices.Result.Success && outAttribute.HasValue)
			{
				return outAttribute.Value.Data?.Value.AsUtf8;
			}
			return null;
		}

		public bool ValidatePassword(LobbySearchResult lobby, string enteredPassword)
		{
			if (!lobby.HasPassword)
			{
				return true;
			}
			if (string.IsNullOrEmpty(enteredPassword))
			{
				return false;
			}
			return ComputeSHA256(enteredPassword) == lobby.PasswordHash;
		}

		public bool IsVersionCompatible(LobbySearchResult lobby)
		{
			return IsCompatibleVersion(lobby.Version);
		}

		private static bool IsCompatibleVersion(string lobbyVersion)
		{
			if (!string.IsNullOrEmpty(lobbyVersion))
			{
				return lobbyVersion == Application.version;
			}
			return false;
		}

		private static string ComputeSHA256(string input)
		{
			using SHA256 sHA = SHA256.Create();
			return BitConverter.ToString(sHA.ComputeHash(Encoding.UTF8.GetBytes(input))).Replace("-", "").ToLowerInvariant();
		}

		private void OnApplicationQuit()
		{
			if (!string.IsNullOrEmpty(_currentLobbyId))
			{
				DestroyLobbyImmediate();
			}
		}

		private void DestroyLobbyImmediate()
		{
			LobbyInterface lobbyInterface = GetLobbyInterface();
			if (!(lobbyInterface == null) && !string.IsNullOrEmpty(_currentLobbyId))
			{
				if (_isOwner)
				{
					DestroyLobbyOptions options = new DestroyLobbyOptions
					{
						LocalUserId = EOSSDKComponent.LocalUserProductId,
						LobbyId = _currentLobbyId
					};
					lobbyInterface.DestroyLobby(ref options, null, delegate
					{
					});
				}
				else
				{
					LeaveLobbyOptions options2 = new LeaveLobbyOptions
					{
						LocalUserId = EOSSDKComponent.LocalUserProductId,
						LobbyId = _currentLobbyId
					};
					lobbyInterface.LeaveLobby(ref options2, null, delegate
					{
					});
				}
				_playerInLobby = false;
				_currentLobbyId = null;
				_isOwner = false;
			}
		}

		public void Activate()
		{
			base.gameObject.SetActive(value: true);
		}

		public void Deactivate()
		{
			base.gameObject.SetActive(value: false);
		}

		private void OnDestroy()
		{
			UnsubscribeFromNotifications();
		}
	}
}
