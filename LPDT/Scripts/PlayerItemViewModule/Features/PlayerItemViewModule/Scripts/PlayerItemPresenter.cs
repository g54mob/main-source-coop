using System.Linq;
using Features.AudioDevicesModule.Scripts.PushToTalk.Views;
using Features.GameUpdaterModule;
using Features.MultiplayerSessionServices.Scripts;
using Features.PlayersPingModule.Scripts;
using Features.SettingsMenuModule.Scripts.Data;
using Features.VoiceSpeakersModule.Scripts.Data;
using JetBrains.Annotations;
using PlayerCustomization;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Features.PlayerItemViewModule.Scripts
{
	[PublicAPI]
	public class PlayerItemPresenter : PresenterBehaviour<PlayerItemViewBase>
	{
		private const float MutedVolumeThreshold = 0.001f;

		private static readonly int IsSpeaking = Animator.StringToHash("IsSpeaking");

		private static readonly int IsMuted = Animator.StringToHash("IsMuted");

		private readonly PlayerCustomizationModel _playerCustomizationModel;

		private readonly PlayersPingModel _playersPingModel;

		private readonly IGameUpdater _gameUpdater;

		private readonly RegionsPingConfiguration _regionsPingConfiguration;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly IMultiplayerService _multiplayerService;

		private readonly PlayerVoiceActivityModel _playerVoiceActivityModel;

		private readonly PlayersVoiceEnabledModel _playersVoiceEnabledModel;

		private readonly PlayersVolumeData _playersVolumeData;

		private readonly DiContainer _diContainer;

		private PlayerVolumeInfo _trackedVolumeInfo;

		private IWindow _ownerWindow;

		private bool _isPushToTalkSpawned;

		private float _currentPingValue;

		private bool _disablePing;

		private int _playerId = -1;

		public PlayerItemPresenter(PlayerCustomizationModel playerCustomizationModel, PlayersPingModel playersPingModel, IGameUpdater gameUpdater, RegionsPingConfiguration regionsPingConfiguration, MultiplayerModel multiplayerModel, PlayerVoiceActivityModel playerVoiceActivityModel, PlayersVoiceEnabledModel playersVoiceEnabledModel, PlayersVolumeData playersVolumeData, DiContainer diContainer, IWindowsService windowsService, IMultiplayerService multiplayerService)
		{
			_playerCustomizationModel = playerCustomizationModel;
			_playersPingModel = playersPingModel;
			_gameUpdater = gameUpdater;
			_regionsPingConfiguration = regionsPingConfiguration;
			_multiplayerModel = multiplayerModel;
			_multiplayerService = multiplayerService;
			_playerVoiceActivityModel = playerVoiceActivityModel;
			_playersVoiceEnabledModel = playersVoiceEnabledModel;
			_playersVolumeData = playersVolumeData;
			_diContainer = diContainer;
		}

		public void SetOwnerWindow(IWindow ownerWindow)
		{
			_ownerWindow = ownerWindow;
		}

		protected override void OnViewEnabled()
		{
			base.OnViewEnabled();
			_playerVoiceActivityModel.OnPlayerSpeakingChanged += OnPlayerSpeakingChanged;
			_playersVoiceEnabledModel.OnPlayerVoiceEnabledChanged += OnPlayerVoiceEnabledChanged;
			_playersVolumeData.OnPlayerVolumeRegistered += OnPlayerVolumeRegistered;
			_playersVolumeData.BeforePlayerVolumeUnregistered += OnPlayerVolumeUnregistered;
			if (base.View.KickButton != null)
			{
				base.View.KickButton.onClick.AddListener(OnKickClicked);
			}
			UpdateKickButtonVisibility();
			if (_disablePing)
			{
				base.View.Ping.SetText("");
			}
			else
			{
				_gameUpdater.OnUpdate += ProcessPingValue;
			}
		}

		protected override void OnViewDisabled()
		{
			base.OnViewDisabled();
			_playerVoiceActivityModel.OnPlayerSpeakingChanged -= OnPlayerSpeakingChanged;
			_playersVoiceEnabledModel.OnPlayerVoiceEnabledChanged -= OnPlayerVoiceEnabledChanged;
			_playersVolumeData.OnPlayerVolumeRegistered -= OnPlayerVolumeRegistered;
			_playersVolumeData.BeforePlayerVolumeUnregistered -= OnPlayerVolumeUnregistered;
			if (base.View.KickButton != null)
			{
				base.View.KickButton.onClick.RemoveListener(OnKickClicked);
			}
			UnbindVolumeInfo();
			_gameUpdater.OnUpdate -= ProcessPingValue;
		}

		private void ProcessPingValue()
		{
			if (base.View.Ping == null)
			{
				return;
			}
			if (_disablePing)
			{
				_gameUpdater.OnUpdate -= ProcessPingValue;
				base.View.Ping.SetText("");
			}
			else if (_playerId != -1)
			{
				if (_playersPingModel.PlayersPings.ContainsKey(_playerId))
				{
					_currentPingValue = Mathf.Lerp(_currentPingValue, (float)_playersPingModel.PlayersPings[_playerId], Time.deltaTime);
					float currentPingValue = _currentPingValue;
					string arg = _regionsPingConfiguration.EvaluateHex(currentPingValue);
					string text = $"<color=#{arg}>{Mathf.Max((int)currentPingValue, 0)} ms</color>";
					base.View.Ping.SetText(text);
				}
				else
				{
					base.View.Ping.SetText("");
				}
			}
		}

		public void DisablePing(bool disablePing)
		{
			_disablePing = disablePing;
		}

		public void DestroyView()
		{
			Object.Destroy(base.View.gameObject);
		}

		public void SetSpeakerStatus(VoiceActivityStatus voiceActivityStatus)
		{
		}

		public void SetSpectatorView(bool isActive)
		{
			base.View.CrossGameObject.SetActive(isActive);
		}

		public void SetDeadColor(bool isDead)
		{
			base.View.SetDeadColor(isDead);
		}

		public void SetDeadPartDamaged(bool isDeadPartDamaged)
		{
			base.View.DeadPartDamagedObject.SetActive(isDeadPartDamaged);
		}

		public void SetIcon(int playerId)
		{
			PlayerCustomizationSlotData playerCustomizationSlotData = _playerCustomizationModel.Slots.FirstOrDefault((PlayerCustomizationSlotData slotData) => slotData.PlayerId == playerId);
			if (playerCustomizationSlotData == null)
			{
				return;
			}
			base.View.PlayerName.SetText(playerCustomizationSlotData.Nickname);
			foreach (Image playerIcon in base.View.PlayerIcons)
			{
				playerIcon.color = playerCustomizationSlotData.PrimaryColor;
			}
		}

		public void SetPlayerId(int playerId)
		{
			_playerId = playerId;
			base.View.transform.SetSiblingIndex(playerId);
			if (base.View.Animator != null)
			{
				base.View.Animator.SetBool(IsSpeaking, _playerVoiceActivityModel.IsPlayerSpeaking(playerId));
			}
			RebindVolumeInfo();
			UpdateMuteState();
			UpdateKickButtonVisibility();
			if (playerId == _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId && !base.View.SpawnPushToTalkDisabled && !_isPushToTalkSpawned)
			{
				_isPushToTalkSpawned = true;
				CreatePushToTalkView();
			}
		}

		private void CreatePushToTalkView()
		{
			PushToTalkHintViewBase component = _diContainer.InstantiatePrefab(base.View.PushToTalkHintViewBase).GetComponent<PushToTalkHintViewBase>();
			_ownerWindow.AddView(component.transform, worldPositionStays: false);
			_ownerWindow.GetPresenterForView<PushToTalkHintPresenter>(component).SetParent(base.View.PushToTalkContainer);
		}

		public void SetParent(Transform parent)
		{
			base.View.transform.SetParent(parent);
		}

		private void UpdateKickButtonVisibility()
		{
			if (!(base.View.KickButton == null))
			{
				bool isSharedModeMasterClient = _multiplayerModel.NetworkRunner.IsSharedModeMasterClient;
				bool flag = _playerId == _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId;
				base.View.KickButton.gameObject.SetActive(isSharedModeMasterClient && !flag && _playerId != -1);
			}
		}

		private void OnKickClicked()
		{
			_multiplayerService.KickFromRoom(_multiplayerModel.NetworkRunner, _playerId);
		}

		private void OnPlayerSpeakingChanged(int playerId, bool isSpeaking)
		{
			if (playerId == _playerId && base.View.Animator != null)
			{
				base.View.Animator.SetBool(IsSpeaking, isSpeaking);
			}
		}

		private void OnPlayerVoiceEnabledChanged(int playerId, bool _)
		{
			if (playerId == _playerId)
			{
				UpdateMuteState();
			}
		}

		private void OnPlayerVolumeRegistered(int playerId)
		{
			if (playerId == _playerId)
			{
				RebindVolumeInfo();
				UpdateMuteState();
			}
		}

		private void OnPlayerVolumeUnregistered(int playerId)
		{
			if (playerId == _playerId)
			{
				UnbindVolumeInfo();
				UpdateMuteState();
			}
		}

		private void OnVolumeUpdated(float _)
		{
			UpdateMuteState();
		}

		private void RebindVolumeInfo()
		{
			UnbindVolumeInfo();
			if (_playerId != -1 && _playersVolumeData.PlayersVolumeInfo.TryGetValue(_playerId, out var value))
			{
				_trackedVolumeInfo = value;
				_trackedVolumeInfo.OnPlayerVolumeUpdate += OnVolumeUpdated;
			}
		}

		private void UnbindVolumeInfo()
		{
			if (_trackedVolumeInfo != null)
			{
				_trackedVolumeInfo.OnPlayerVolumeUpdate -= OnVolumeUpdated;
				_trackedVolumeInfo = null;
			}
		}

		private void UpdateMuteState()
		{
			if (_playerId != -1)
			{
				bool flag = !_playersVoiceEnabledModel.IsVoiceEnabled(_playerId);
				PlayerVolumeInfo value;
				bool flag2 = _playersVolumeData.PlayersVolumeInfo.TryGetValue(_playerId, out value) && value.Volume <= 0.001f;
				if (!(base.View.Animator == null))
				{
					base.View.Animator.SetBool(IsMuted, flag || flag2);
				}
			}
		}
	}
}
