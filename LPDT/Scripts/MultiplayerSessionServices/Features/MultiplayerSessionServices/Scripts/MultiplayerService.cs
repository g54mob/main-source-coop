using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Features.CommandLineArguments.Scripts;
using Fusion;
using Fusion.Photon.Realtime;
using UnityEngine;

namespace Features.MultiplayerSessionServices.Scripts
{
	public class MultiplayerService : IMultiplayerService
	{
		private const string VERSION_PROPERTY_KEY = "Version";

		private const string ENVIRONMENT_PROPERTY_KEY = "Environment";

		private const string IS_SESSION_STARTED_PROPERTY_KEY = "IsSessionStarted";

		private const string IS_PUBLIC_PROPERTY_KEY = "IsPublic";

		private const string HOST_NAME_PROPERTY_KEY = "HostName";

		private const string CUSTOM_ENVIRONMENT_ARGUMENT_NAME = "-custom_environment";

		private readonly MultiplayerSessionConfig _multiplayerSessionConfig;

		private readonly KickPlayerRequestNetworkEvent _kickPlayerRequest;

		private readonly ICommandLineArgumentsService _commandLineArgumentsService;

		private readonly MultiplayerModel _multiplayerModel;

		public MultiplayerService(MultiplayerSessionConfig multiplayerSessionConfig, ICommandLineArgumentsService commandLineArgumentsService, KickPlayerRequestNetworkEvent kickPlayerRequest, MultiplayerModel multiplayerModel)
		{
			_multiplayerSessionConfig = multiplayerSessionConfig;
			_kickPlayerRequest = kickPlayerRequest;
			_multiplayerModel = multiplayerModel;
			_commandLineArgumentsService = commandLineArgumentsService;
			PhotonAppSettings.Global.AppSettings.AppVersion = Application.version;
		}

		public async UniTask<StartGameResult> CreateRoom(NetworkRunner runner, SessionCreateData sessionData)
		{
			StartGameResult result = await runner.StartGame(new StartGameArgs
			{
				GameMode = GameMode.Shared,
				SessionName = sessionData.RoomCode,
				SessionProperties = CreateSessionProperties(sessionData),
				PlayerCount = _multiplayerSessionConfig.MaxPlayersInRoom,
				SceneManager = runner.GetComponent<INetworkSceneManager>(),
				CustomPhotonAppSettings = CreatePhotonSettings()
			});
			if (result != null && result.Ok && runner != null)
			{
				runner.SessionInfo.IsVisible = false;
			}
			await UniTask.WaitUntil(() => runner == null || !runner.SceneManager.IsBusy);
			return result;
		}

		public async UniTask<StartGameResult> ConnectToRoom(NetworkRunner runner, SessionCreateData sessionData)
		{
			StartGameResult result = await runner.StartGame(new StartGameArgs
			{
				GameMode = GameMode.Shared,
				SessionName = sessionData.RoomCode,
				SceneManager = runner.GetComponent<INetworkSceneManager>(),
				EnableClientSessionCreation = false,
				CustomPhotonAppSettings = CreatePhotonSettings()
			});
			await UniTask.WaitUntil(() => runner == null || !runner.SceneManager.IsBusy);
			return result;
		}

		public async UniTask<StartGameResult> JoinSessionLobby(NetworkRunner runner, string lobbyCode = null)
		{
			return await runner.JoinSessionLobby(SessionLobby.Shared, lobbyCode);
		}

		public async UniTask Shutdown(NetworkRunner runner, ShutdownReason shutdownReason)
		{
			if (runner != null)
			{
				await runner.Shutdown(destroyGameObject: true, shutdownReason);
			}
		}

		public bool TryGetPlayerRefById(int playerId, out PlayerRef playerRef)
		{
			playerRef = _multiplayerModel.NetworkRunner.ActivePlayers.FirstOrDefault((PlayerRef player) => player.PlayerId == playerId);
			return !playerRef.IsNone;
		}

		private Dictionary<string, SessionProperty> CreateSessionProperties(SessionCreateData sessionData)
		{
			Dictionary<string, SessionProperty> dictionary = new Dictionary<string, SessionProperty>
			{
				["Version"] = Application.version,
				["IsSessionStarted"] = false,
				["IsPublic"] = false,
				["HostName"] = sessionData.HostName ?? string.Empty
			};
			if (Application.isEditor && _multiplayerSessionConfig.UseCustomEnvironment)
			{
				dictionary["Environment"] = _multiplayerSessionConfig.CustomEnvironment;
			}
			else if (_commandLineArgumentsService.HasArgument("-custom_environment"))
			{
				dictionary["Environment"] = _commandLineArgumentsService.GetString("-custom_environment");
			}
			return dictionary;
		}

		public void KickFromRoom(NetworkRunner runner, int playerId)
		{
			if (!(runner == null))
			{
				if (!runner.IsSharedModeMasterClient)
				{
					throw new Exception($"Kicking player {playerId} is not allowed since player {runner.LocalPlayer.PlayerId} is not a host");
				}
				if (runner.LocalPlayer.PlayerId == playerId)
				{
					throw new Exception("Kicking yourself from room is forbidden");
				}
				_kickPlayerRequest.SendKickRequest(playerId);
			}
		}

		public void SetSessionPublic(NetworkRunner runner, bool isPublic)
		{
			OverrideSessionProperty(runner, SessionPropertyType.IsPublic, isPublic);
			RefreshSessionVisibility(runner, isPublic);
		}

		public void MarkSessionStarted(NetworkRunner runner)
		{
			OverrideSessionProperty(runner, SessionPropertyType.IsSessionStarted, propertyValue: true);
			bool? isSessionStartedOverride = true;
			RefreshSessionVisibility(runner, null, isSessionStartedOverride);
		}

		public void RefreshSessionVisibility(NetworkRunner runner, bool? isPublicOverride = null, bool? isSessionStartedOverride = null)
		{
			if (!(runner == null) && runner.IsRunning && runner.IsSharedModeMasterClient)
			{
				bool flag = runner.SessionInfo.PlayerCount >= _multiplayerSessionConfig.MaxPlayersInRoom;
				bool value;
				bool flag2 = isPublicOverride ?? (TryGetSessionProperty<bool>(runner.SessionInfo, SessionPropertyType.IsPublic, out value) && value);
				bool value2;
				bool flag3 = isSessionStartedOverride ?? (TryGetSessionProperty<bool>(runner.SessionInfo, SessionPropertyType.IsSessionStarted, out value2) && value2);
				runner.SessionInfo.IsOpen = !flag;
				runner.SessionInfo.IsVisible = flag3 || (flag2 && !flag);
				_multiplayerModel.InvokeSessionOpenedChanged(runner.SessionInfo.IsOpen);
			}
		}

		public void OverrideSessionProperty<TProperty>(NetworkRunner runner, SessionPropertyType propertyType, TProperty propertyValue)
		{
			string sessionPropertyKey = GetSessionPropertyKey(propertyType);
			SessionProperty sessionProperty = SessionProperty.Convert(propertyValue);
			if (!runner.TrySetRoomCustomProperty(sessionPropertyKey, sessionProperty.PropertyValue))
			{
				Debug.LogError($"Failed to apply session property {propertyType}({sessionPropertyKey})={propertyValue} on session '{runner.SessionInfo.Name}'.");
			}
		}

		public TProperty GetSessionProperty<TProperty>(NetworkRunner runner, SessionPropertyType propertyType)
		{
			return (TProperty)runner.SessionInfo.Properties[GetSessionPropertyKey(propertyType)].PropertyValue;
		}

		public bool TryGetSessionProperty<TProperty>(SessionInfo sessionInfo, SessionPropertyType propertyType, out TProperty value)
		{
			value = default(TProperty);
			if (!sessionInfo.IsValid || sessionInfo.Properties == null)
			{
				return false;
			}
			if (!sessionInfo.Properties.TryGetValue(GetSessionPropertyKey(propertyType), out var value2))
			{
				return false;
			}
			if (typeof(TProperty) != value2.PropertyType)
			{
				return false;
			}
			value = (TProperty)value2.PropertyValue;
			return true;
		}

		private string GetSessionPropertyKey(SessionPropertyType propertyType)
		{
			return propertyType switch
			{
				SessionPropertyType.Version => "Version", 
				SessionPropertyType.Environment => "Environment", 
				SessionPropertyType.IsSessionStarted => "IsSessionStarted", 
				SessionPropertyType.IsPublic => "IsPublic", 
				SessionPropertyType.HostName => "HostName", 
				_ => throw new ArgumentOutOfRangeException("propertyType", propertyType, null), 
			};
		}

		private static FusionAppSettings CreatePhotonSettings()
		{
			FusionAppSettings copy = PhotonAppSettings.Global.AppSettings.GetCopy();
			copy.AppVersion = Application.version;
			return copy;
		}
	}
}
