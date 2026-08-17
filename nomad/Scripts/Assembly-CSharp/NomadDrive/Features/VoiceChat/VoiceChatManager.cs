using System;
using System.Collections.Generic;
using Epic.OnlineServices;
using Epic.OnlineServices.Lobby;
using Epic.OnlineServices.RTC;
using Epic.OnlineServices.RTCAudio;
using EpicTransport;
using EvilCore.EvilPack.EvilLogger;
using EvilCore.Networking;
using NomadDrive.Features.Player;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.VoiceChat
{
	public class VoiceChatManager : MonoBehaviour, IVoiceChatManager, IVoiceDeviceController
	{
		[Inject]
		private IEOSLobbyManager _lobbyManager;

		[Inject]
		private IPlayerService _playerService;

		private string _rtcRoomName;

		private bool _isConnected;

		private bool _isMuted;

		private bool _lastLocalSpeaking;

		private ulong _roomConnectionChangedNotifyId;

		private ulong _participantStatusChangedNotifyId;

		private ulong _participantAudioUpdatedNotifyId;

		private ulong _audioDevicesChangedNotifyId;

		private readonly HashSet<string> _participants = new HashSet<string>();

		private readonly HashSet<string> _receivingEnabled = new HashSet<string>();

		private readonly List<VoiceAudioDevice> _inputDevices = new List<VoiceAudioDevice>();

		private readonly List<VoiceAudioDevice> _outputDevices = new List<VoiceAudioDevice>();

		private string _activeInputDeviceId;

		private string _activeOutputDeviceId;

		public bool IsConnected => _isConnected;

		public bool IsMuted => _isMuted;

		public IReadOnlyList<VoiceAudioDevice> InputDevices => _inputDevices;

		public IReadOnlyList<VoiceAudioDevice> OutputDevices => _outputDevices;

		public string ActiveInputDeviceId => _activeInputDeviceId;

		public string ActiveOutputDeviceId => _activeOutputDeviceId;

		public event Action<bool> OnMuteStateChanged;

		public event Action OnAudioDevicesChanged;

		private void OnEnable()
		{
			if (_lobbyManager != null)
			{
				_lobbyManager.OnLobbyCreated += OnLobbyReady;
				_lobbyManager.OnLobbyJoined += OnLobbyReady;
				_lobbyManager.OnLobbyLeft += OnLobbyGone;
				_lobbyManager.OnLobbyClosed += OnLobbyGone;
			}
		}

		private void OnDisable()
		{
			if (_lobbyManager != null)
			{
				_lobbyManager.OnLobbyCreated -= OnLobbyReady;
				_lobbyManager.OnLobbyJoined -= OnLobbyReady;
				_lobbyManager.OnLobbyLeft -= OnLobbyGone;
				_lobbyManager.OnLobbyClosed -= OnLobbyGone;
			}
		}

		private void Update()
		{
			if (_audioDevicesChangedNotifyId == 0L)
			{
				EnsureDeviceSystemReady();
			}
		}

		private void OnLobbyReady()
		{
			string currentLobbyId = _lobbyManager.CurrentLobbyId;
			if (string.IsNullOrEmpty(currentLobbyId))
			{
				return;
			}
			LobbyInterface lobbyInterface = EOSSDKComponent.GetLobbyInterface();
			if (lobbyInterface == null)
			{
				EvilLogger.LogError("[VoiceChat] LobbyInterface is null", "OnLobbyReady", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\VoiceChat\\Scripts\\VoiceChatManager.cs", 96);
				return;
			}
			ProductUserId localUserProductId = EOSSDKComponent.LocalUserProductId;
			GetRTCRoomNameOptions options = new GetRTCRoomNameOptions
			{
				LobbyId = currentLobbyId,
				LocalUserId = localUserProductId
			};
			Utf8String outBuffer;
			Result rTCRoomName = lobbyInterface.GetRTCRoomName(ref options, out outBuffer);
			if (rTCRoomName != Result.Success)
			{
				EvilLogger.LogError($"[VoiceChat] Failed to get RTC room name: {rTCRoomName}", "OnLobbyReady", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\VoiceChat\\Scripts\\VoiceChatManager.cs", 111);
				return;
			}
			_rtcRoomName = outBuffer;
			IsRTCRoomConnectedOptions options2 = new IsRTCRoomConnectedOptions
			{
				LobbyId = currentLobbyId,
				LocalUserId = localUserProductId
			};
			if (lobbyInterface.IsRTCRoomConnected(ref options2, out var outIsConnected) == Result.Success)
			{
				_isConnected = outIsConnected;
			}
			SubscribeToRTCNotifications(lobbyInterface, localUserProductId);
			ApplySendingState();
		}

		private void OnLobbyGone()
		{
			UnsubscribeFromRTCNotifications();
			_rtcRoomName = null;
			_isConnected = false;
			_participants.Clear();
			_receivingEnabled.Clear();
			BridgeLocalSpeaking(speaking: false, force: true);
		}

		private void SubscribeToRTCNotifications(LobbyInterface lobbyInterface, ProductUserId localUserId)
		{
			UnsubscribeFromRTCNotifications();
			AddNotifyRTCRoomConnectionChangedOptions options = default(AddNotifyRTCRoomConnectionChangedOptions);
			_roomConnectionChangedNotifyId = lobbyInterface.AddNotifyRTCRoomConnectionChanged(ref options, null, OnRTCRoomConnectionChanged);
			RTCInterface rTCInterface = EOSSDKComponent.GetRTCInterface();
			if (rTCInterface != null)
			{
				AddNotifyParticipantStatusChangedOptions options2 = new AddNotifyParticipantStatusChangedOptions
				{
					LocalUserId = localUserId,
					RoomName = _rtcRoomName
				};
				_participantStatusChangedNotifyId = rTCInterface.AddNotifyParticipantStatusChanged(ref options2, null, OnParticipantStatusChanged);
			}
			RTCAudioInterface rTCAudioInterface = EOSSDKComponent.GetRTCAudioInterface();
			if (rTCAudioInterface != null)
			{
				AddNotifyParticipantUpdatedOptions options3 = new AddNotifyParticipantUpdatedOptions
				{
					LocalUserId = localUserId,
					RoomName = _rtcRoomName
				};
				_participantAudioUpdatedNotifyId = rTCAudioInterface.AddNotifyParticipantUpdated(ref options3, null, OnParticipantAudioUpdated);
			}
		}

		private void OnRTCRoomConnectionChanged(ref RTCRoomConnectionChangedCallbackInfo data)
		{
			_isConnected = data.IsConnected;
			if (data.IsConnected)
			{
				ApplySendingState();
			}
		}

		private void OnParticipantStatusChanged(ref ParticipantStatusChangedCallbackInfo data)
		{
			if (!IsLocalUser(data.ParticipantId))
			{
				string item = data.ParticipantId?.ToString() ?? "unknown";
				if (data.ParticipantStatus == RTCParticipantStatus.Joined)
				{
					_participants.Add(item);
				}
				else if (data.ParticipantStatus == RTCParticipantStatus.Left)
				{
					_participants.Remove(item);
					_receivingEnabled.Remove(item);
				}
			}
		}

		private void OnParticipantAudioUpdated(ref ParticipantUpdatedCallbackInfo data)
		{
			if (IsLocalUser(data.ParticipantId))
			{
				BridgeLocalSpeaking(!_isMuted && data.Speaking);
				return;
			}
			string item = data.ParticipantId?.ToString() ?? "unknown";
			if (_receivingEnabled.Add(item))
			{
				EnableReceivingFrom(data.ParticipantId);
			}
		}

		private void EnableReceivingFrom(ProductUserId participantId)
		{
			RTCAudioInterface rTCAudioInterface = EOSSDKComponent.GetRTCAudioInterface();
			if (!(rTCAudioInterface == null))
			{
				UpdateReceivingOptions options = new UpdateReceivingOptions
				{
					LocalUserId = EOSSDKComponent.LocalUserProductId,
					RoomName = _rtcRoomName,
					ParticipantId = participantId,
					AudioEnabled = true
				};
				rTCAudioInterface.UpdateReceiving(ref options, null, delegate(ref UpdateReceivingCallbackInfo cb)
				{
					_ = cb.ResultCode;
				});
			}
		}

		public void ToggleMute()
		{
			SetMuted(!_isMuted);
		}

		public void SetMuted(bool muted)
		{
			if (_isMuted != muted)
			{
				_isMuted = muted;
				ApplySendingState();
				this.OnMuteStateChanged?.Invoke(_isMuted);
				if (_isMuted)
				{
					BridgeLocalSpeaking(speaking: false, force: true);
				}
			}
		}

		private void ApplySendingState()
		{
			if (!string.IsNullOrEmpty(_rtcRoomName))
			{
				UpdateSendingStatus((!_isMuted) ? RTCAudioStatus.Enabled : RTCAudioStatus.Disabled);
			}
		}

		private void UpdateSendingStatus(RTCAudioStatus status)
		{
			RTCAudioInterface rTCAudioInterface = EOSSDKComponent.GetRTCAudioInterface();
			if (rTCAudioInterface == null)
			{
				return;
			}
			UpdateSendingOptions options = new UpdateSendingOptions
			{
				LocalUserId = EOSSDKComponent.LocalUserProductId,
				RoomName = _rtcRoomName,
				AudioStatus = status
			};
			rTCAudioInterface.UpdateSending(ref options, null, delegate(ref UpdateSendingCallbackInfo cb)
			{
				if (cb.ResultCode != Result.Success)
				{
					EvilLogger.LogError($"[VoiceChat] Failed to update sending ({status}): {cb.ResultCode}", "UpdateSendingStatus", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\VoiceChat\\Scripts\\VoiceChatManager.cs", 270);
				}
			});
		}

		private void BridgeLocalSpeaking(bool speaking, bool force = false)
		{
			if (force || speaking != _lastLocalSpeaking)
			{
				_lastLocalSpeaking = speaking;
				NomadDrive.Features.Player.Player player = _playerService?.LocalPlayer;
				if (!(player == null) && player.isOwned && (force || player.IsSpeaking != speaking))
				{
					player.CmdSetSpeaking(speaking);
				}
			}
		}

		private bool IsLocalUser(ProductUserId userId)
		{
			ProductUserId localUserProductId = EOSSDKComponent.LocalUserProductId;
			if (localUserProductId == null || userId == null)
			{
				return false;
			}
			return localUserProductId.ToString() == userId.ToString();
		}

		private void EnsureDeviceSystemReady()
		{
			RTCAudioInterface rTCAudioInterface = EOSSDKComponent.GetRTCAudioInterface();
			if (!(rTCAudioInterface == null))
			{
				AddNotifyAudioDevicesChangedOptions options = default(AddNotifyAudioDevicesChangedOptions);
				_audioDevicesChangedNotifyId = rTCAudioInterface.AddNotifyAudioDevicesChanged(ref options, null, OnAudioDevicesChangedNative);
				QueryDevices();
			}
		}

		private void OnAudioDevicesChangedNative(ref AudioDevicesChangedCallbackInfo data)
		{
			QueryDevices();
		}

		public void RefreshAudioDevices()
		{
			QueryDevices();
		}

		private void QueryDevices()
		{
			RTCAudioInterface rTCAudioInterface = EOSSDKComponent.GetRTCAudioInterface();
			if (!(rTCAudioInterface == null))
			{
				QueryInputDevicesInformationOptions options = default(QueryInputDevicesInformationOptions);
				rTCAudioInterface.QueryInputDevicesInformation(ref options, null, OnInputDevicesQueried);
				QueryOutputDevicesInformationOptions options2 = default(QueryOutputDevicesInformationOptions);
				rTCAudioInterface.QueryOutputDevicesInformation(ref options2, null, OnOutputDevicesQueried);
			}
		}

		private void OnInputDevicesQueried(ref OnQueryInputDevicesInformationCallbackInfo data)
		{
			if (data.ResultCode == Result.Success)
			{
				RebuildInputDevices();
				this.OnAudioDevicesChanged?.Invoke();
			}
		}

		private void OnOutputDevicesQueried(ref OnQueryOutputDevicesInformationCallbackInfo data)
		{
			if (data.ResultCode == Result.Success)
			{
				RebuildOutputDevices();
				this.OnAudioDevicesChanged?.Invoke();
			}
		}

		private void RebuildInputDevices()
		{
			_inputDevices.Clear();
			RTCAudioInterface rTCAudioInterface = EOSSDKComponent.GetRTCAudioInterface();
			if (rTCAudioInterface == null)
			{
				return;
			}
			GetInputDevicesCountOptions options = default(GetInputDevicesCountOptions);
			uint inputDevicesCount = rTCAudioInterface.GetInputDevicesCount(ref options);
			for (uint num = 0u; num < inputDevicesCount; num++)
			{
				CopyInputDeviceInformationByIndexOptions options2 = new CopyInputDeviceInformationByIndexOptions
				{
					DeviceIndex = num
				};
				if (rTCAudioInterface.CopyInputDeviceInformationByIndex(ref options2, out var outInputDeviceInformation) == Result.Success && outInputDeviceInformation.HasValue)
				{
					InputDeviceInformation value = outInputDeviceInformation.Value;
					_inputDevices.Add(new VoiceAudioDevice(value.DeviceId, value.DeviceName, value.DefaultDevice));
					if (value.DefaultDevice && string.IsNullOrEmpty(_activeInputDeviceId))
					{
						_activeInputDeviceId = value.DeviceId;
					}
				}
			}
		}

		private void RebuildOutputDevices()
		{
			_outputDevices.Clear();
			RTCAudioInterface rTCAudioInterface = EOSSDKComponent.GetRTCAudioInterface();
			if (rTCAudioInterface == null)
			{
				return;
			}
			GetOutputDevicesCountOptions options = default(GetOutputDevicesCountOptions);
			uint outputDevicesCount = rTCAudioInterface.GetOutputDevicesCount(ref options);
			for (uint num = 0u; num < outputDevicesCount; num++)
			{
				CopyOutputDeviceInformationByIndexOptions options2 = new CopyOutputDeviceInformationByIndexOptions
				{
					DeviceIndex = num
				};
				if (rTCAudioInterface.CopyOutputDeviceInformationByIndex(ref options2, out var outOutputDeviceInformation) == Result.Success && outOutputDeviceInformation.HasValue)
				{
					OutputDeviceInformation value = outOutputDeviceInformation.Value;
					_outputDevices.Add(new VoiceAudioDevice(value.DeviceId, value.DeviceName, value.DefaultDevice));
					if (value.DefaultDevice && string.IsNullOrEmpty(_activeOutputDeviceId))
					{
						_activeOutputDeviceId = value.DeviceId;
					}
				}
			}
		}

		public void SetInputDevice(string deviceId)
		{
			RTCAudioInterface rTCAudioInterface = EOSSDKComponent.GetRTCAudioInterface();
			if (rTCAudioInterface == null)
			{
				return;
			}
			ProductUserId localUserProductId = EOSSDKComponent.LocalUserProductId;
			if (localUserProductId == null)
			{
				return;
			}
			SetInputDeviceSettingsOptions options = new SetInputDeviceSettingsOptions
			{
				LocalUserId = localUserProductId,
				RealDeviceId = deviceId,
				PlatformAEC = true
			};
			rTCAudioInterface.SetInputDeviceSettings(ref options, null, delegate(ref OnSetInputDeviceSettingsCallbackInfo cb)
			{
				if (cb.ResultCode == Result.Success)
				{
					_activeInputDeviceId = cb.RealDeviceId;
					this.OnAudioDevicesChanged?.Invoke();
				}
			});
		}

		public void SetOutputDevice(string deviceId)
		{
			RTCAudioInterface rTCAudioInterface = EOSSDKComponent.GetRTCAudioInterface();
			if (rTCAudioInterface == null)
			{
				return;
			}
			ProductUserId localUserProductId = EOSSDKComponent.LocalUserProductId;
			if (localUserProductId == null)
			{
				return;
			}
			SetOutputDeviceSettingsOptions options = new SetOutputDeviceSettingsOptions
			{
				LocalUserId = localUserProductId,
				RealDeviceId = deviceId
			};
			rTCAudioInterface.SetOutputDeviceSettings(ref options, null, delegate(ref OnSetOutputDeviceSettingsCallbackInfo cb)
			{
				if (cb.ResultCode == Result.Success)
				{
					_activeOutputDeviceId = cb.RealDeviceId;
					this.OnAudioDevicesChanged?.Invoke();
				}
			});
		}

		private void UnsubscribeFromRTCNotifications()
		{
			LobbyInterface lobbyInterface = EOSSDKComponent.GetLobbyInterface();
			if (lobbyInterface != null && _roomConnectionChangedNotifyId != 0L)
			{
				try
				{
					lobbyInterface.RemoveNotifyRTCRoomConnectionChanged(_roomConnectionChangedNotifyId);
				}
				catch (Exception)
				{
				}
				_roomConnectionChangedNotifyId = 0uL;
			}
			RTCInterface rTCInterface = EOSSDKComponent.GetRTCInterface();
			if (rTCInterface != null && _participantStatusChangedNotifyId != 0L)
			{
				try
				{
					rTCInterface.RemoveNotifyParticipantStatusChanged(_participantStatusChangedNotifyId);
				}
				catch (Exception)
				{
				}
				_participantStatusChangedNotifyId = 0uL;
			}
			RTCAudioInterface rTCAudioInterface = EOSSDKComponent.GetRTCAudioInterface();
			if (rTCAudioInterface != null && _participantAudioUpdatedNotifyId != 0L)
			{
				try
				{
					rTCAudioInterface.RemoveNotifyParticipantUpdated(_participantAudioUpdatedNotifyId);
				}
				catch (Exception)
				{
				}
				_participantAudioUpdatedNotifyId = 0uL;
			}
		}

		private void OnDestroy()
		{
			UnsubscribeFromRTCNotifications();
			RTCAudioInterface rTCAudioInterface = EOSSDKComponent.GetRTCAudioInterface();
			if (rTCAudioInterface != null && _audioDevicesChangedNotifyId != 0L)
			{
				try
				{
					rTCAudioInterface.RemoveNotifyAudioDevicesChanged(_audioDevicesChangedNotifyId);
				}
				catch (Exception)
				{
				}
				_audioDevicesChangedNotifyId = 0uL;
			}
		}
	}
}
