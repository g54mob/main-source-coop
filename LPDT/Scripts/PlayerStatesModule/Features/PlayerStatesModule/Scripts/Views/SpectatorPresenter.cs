using System;
using System.Collections.Generic;
using System.Linq;
using Features.InputModule.Scripts.Generated;
using Features.MultiplayerSessionServices.Scripts;
using Features.PlayerItemViewModule.Scripts;
using Features.ViewSystemModule.Scripts.Windows;
using Features.VoiceControlModule.Scripts;
using Features.VoiceSpeakersModule.Scripts.Data;
using Fusion;
using JetBrains.Annotations;
using PlayerCustomization;
using RSG.Muffin.InputSubmodule.InputModule.Core.Scripts;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using RSG.Muffin.UINavigationSubmodule.UINavigationModule.Scripts;
using UnityEngine;
using Zenject;

namespace Features.PlayerStatesModule.Scripts.Views
{
	[PublicAPI]
	public class SpectatorPresenter : PresenterBehaviour<SpectatorViewBase>
	{
		private static readonly int Pressed = Animator.StringToHash("Pressed");

		private readonly Dictionary<int, PlayerItemPresenter> _itemPresenters = new Dictionary<int, PlayerItemPresenter>();

		private readonly IInputService _inputService;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly SpectatorModel _spectatorModel;

		private readonly PlayerCustomizationModel _playerCustomizationModel;

		private readonly IVoiceService _voiceService;

		private readonly DiContainer _container;

		private readonly SpectatorWindow _spectatorWindow;

		private readonly PlayersStatesSynchronizer _playersStatesSynchronizer;

		private readonly ActiveSpeakerModel _speakerModel;

		private readonly IPlayerStateService _playerStateService;

		private readonly SpectatorViewModel _spectatorViewModel;

		private readonly INavigationService _navigationService;

		public SpectatorPresenter(IInputService inputService, MultiplayerModel multiplayerModel, SpectatorModel spectatorModel, PlayerCustomizationModel playerCustomizationModel, IVoiceService voiceService, DiContainer diContainer, SpectatorWindow spectatorWindow, PlayersStatesSynchronizer playersStatesSynchronizer, ActiveSpeakerModel speakerModel, IPlayerStateService playerStateService, SpectatorViewModel spectatorViewModel, INavigationService navigationService)
		{
			_inputService = inputService;
			_multiplayerModel = multiplayerModel;
			_spectatorModel = spectatorModel;
			_playerCustomizationModel = playerCustomizationModel;
			_voiceService = voiceService;
			_container = diContainer;
			_spectatorWindow = spectatorWindow;
			_playersStatesSynchronizer = playersStatesSynchronizer;
			_speakerModel = speakerModel;
			_playerStateService = playerStateService;
			_spectatorViewModel = spectatorViewModel;
			_navigationService = navigationService;
		}

		protected override void OnViewEnabled()
		{
			base.OnViewEnabled();
			InputDefaultActions spectatorLeft = _inputService.SpectatorLeft;
			spectatorLeft.Performed = (Action)Delegate.Combine(spectatorLeft.Performed, new Action(Left));
			InputDefaultActions spectatorRight = _inputService.SpectatorRight;
			spectatorRight.Performed = (Action)Delegate.Combine(spectatorRight.Performed, new Action(Right));
			_spectatorModel.OnSpectatableChanged += ChangePlayerInfo;
			_playersStatesSynchronizer.OnSomePlayerStateChanged += UpdatePlayerInfo;
			_spectatorWindow.OnWindowOpened += UpdatePlayerInfo;
			ProcessFadeOnEnable();
			ChangePlayerInfo();
			ApplyFirstNavigation();
		}

		protected override void OnViewDisabled()
		{
			base.OnViewDisabled();
			InputDefaultActions spectatorLeft = _inputService.SpectatorLeft;
			spectatorLeft.Performed = (Action)Delegate.Remove(spectatorLeft.Performed, new Action(Left));
			InputDefaultActions spectatorRight = _inputService.SpectatorRight;
			spectatorRight.Performed = (Action)Delegate.Remove(spectatorRight.Performed, new Action(Right));
			_spectatorModel.OnSpectatableChanged -= ChangePlayerInfo;
			_playersStatesSynchronizer.OnSomePlayerStateChanged -= UpdatePlayerInfo;
			_spectatorWindow.OnWindowOpened -= UpdatePlayerInfo;
			_spectatorViewModel.Clear();
		}

		private void ApplyFirstNavigation()
		{
			if (base.View.FirstButtonToSelect != null)
			{
				_navigationService.SetNavigationToObject(base.View.FirstButtonToSelect);
			}
		}

		private void UpdatePlayerInfo(Type obj)
		{
			_spectatorWindow.OnWindowOpened -= UpdatePlayerInfo;
			UpdatePlayerInfo();
		}

		private void ProcessFadeOnEnable()
		{
			if (_spectatorViewModel.ShowWithFade)
			{
				base.View.StartFade();
			}
			else
			{
				base.View.DisableFade();
			}
		}

		protected override void OnDisposed()
		{
			base.OnDisposed();
			base.View.ClearFade();
		}

		private void Left()
		{
			List<PlayerRef> list = _multiplayerModel.NetworkRunner.ActivePlayers.ToList();
			int num = _spectatorModel.CurrentSpectatablePlayer - 1;
			if (num < 0)
			{
				num = list.Count - 1;
			}
			base.View.LeftArrowAnimator.SetTrigger(Pressed);
			_spectatorModel.CurrentSpectatablePlayer = num;
			_spectatorModel.InvokeSpectatableChanged();
		}

		private void Right()
		{
			List<PlayerRef> list = _multiplayerModel.NetworkRunner.ActivePlayers.ToList();
			int num = _spectatorModel.CurrentSpectatablePlayer + 1;
			if (num >= list.Count)
			{
				num = 0;
			}
			base.View.RightArrowAnimator.SetTrigger(Pressed);
			_spectatorModel.CurrentSpectatablePlayer = num;
			_spectatorModel.InvokeSpectatableChanged();
		}

		private void UpdatePlayerInfo(PlayerStateData playerStateData)
		{
			UpdatePlayerInfo();
		}

		private void UpdatePlayerInfo()
		{
			if (_spectatorWindow.WindowStatus == WindowStatus.Closed)
			{
				return;
			}
			ClearUnusedItems();
			_playerCustomizationModel.SortPlayers();
			foreach (PlayerCustomizationSlotData slot in _playerCustomizationModel.Slots)
			{
				if (slot.PlayerId != -1 && _playerStateService.GetPlayerState(slot.PlayerId) == PlayerState.Dead)
				{
					ProcessPlayerCustomizationData(slot);
				}
			}
			InitSpeakerState();
		}

		private void InitSpeakerState()
		{
			foreach (KeyValuePair<int, VoiceActivityStatus> item in _speakerModel.SpeakersActiveStatus)
			{
				UpdateSpeakerState(item.Key, item.Value);
			}
		}

		private void UpdateSpeakerState(int player, VoiceActivityStatus voiceActivityStatus)
		{
			if (_itemPresenters.TryGetValue(player, out var value))
			{
				value.SetSpeakerStatus(voiceActivityStatus);
				value.SetSpectatorView(isActive: true);
			}
		}

		private void ClearUnusedItems()
		{
			HashSet<int> hashSet = new HashSet<int>();
			foreach (PlayerCustomizationSlotData slot in _playerCustomizationModel.Slots)
			{
				if (slot.PlayerId != -1)
				{
					hashSet.Add(slot.PlayerId);
				}
			}
			List<int> list = new List<int>();
			foreach (int key in _itemPresenters.Keys)
			{
				if (!hashSet.Contains(key))
				{
					list.Add(key);
				}
				if (_playerStateService.GetPlayerState(key) != PlayerState.Dead && _playerStateService.GetPlayerState(key) != PlayerState.PreDeadCrouch)
				{
					list.Add(key);
				}
			}
			foreach (int item in list)
			{
				_itemPresenters[item].DestroyView();
				_itemPresenters.Remove(item);
			}
		}

		private void ProcessPlayerCustomizationData(PlayerCustomizationSlotData playerCustomizationSlotData)
		{
			if (_itemPresenters.TryGetValue(playerCustomizationSlotData.PlayerId, out var value))
			{
				ApplyPlayerCustomizationData(playerCustomizationSlotData, value);
			}
			else
			{
				CreatePlayerItem(playerCustomizationSlotData);
			}
		}

		private void ChangePlayerInfo()
		{
			PlayerCustomizationSlotData playerCustomizationSlotData = _playerCustomizationModel.Slots.First((PlayerCustomizationSlotData slotData) => slotData.PlayerId == _multiplayerModel.NetworkRunner.ActivePlayers.ElementAt(_spectatorModel.CurrentSpectatablePlayer).PlayerId);
			base.View.Nickname.SetText(playerCustomizationSlotData.Nickname);
			base.View.Nickname.color = playerCustomizationSlotData.PrimaryColor;
		}

		private void CreatePlayerItem(PlayerCustomizationSlotData playerCustomizationSlotData)
		{
			PlayerItemPresenter playerItemPresenter = CreatePlayerSlot(base.View.GetPlayerItemsContainer(), _spectatorWindow, playerCustomizationSlotData.PlayerId);
			playerItemPresenter.DisablePing(disablePing: true);
			_itemPresenters.Add(playerCustomizationSlotData.PlayerId, playerItemPresenter);
			ApplyPlayerCustomizationData(playerCustomizationSlotData, playerItemPresenter);
		}

		private static void ApplyPlayerCustomizationData(PlayerCustomizationSlotData playerCustomizationSlotData, PlayerItemPresenter existingPresenter)
		{
			existingPresenter.SetIcon(playerCustomizationSlotData.PlayerId);
			existingPresenter.SetPlayerId(playerCustomizationSlotData.PlayerId);
		}

		private PlayerItemPresenter CreatePlayerSlot(Transform parent, FocusableWindowBehaviour window, int playerId)
		{
			PlayerItemViewBase component = _container.InstantiatePrefab(base.View.PlayerItemViewBase.gameObject).GetComponent<PlayerItemViewBase>();
			_spectatorWindow.AddView(component.transform, worldPositionStays: false);
			PlayerItemPresenter presenterForView = _spectatorWindow.GetPresenterForView<PlayerItemPresenter>(component);
			presenterForView.SetOwnerWindow(_spectatorWindow);
			presenterForView.SetParent(parent);
			presenterForView.SetIcon(playerId);
			presenterForView.SetPlayerId(playerId);
			return presenterForView;
		}
	}
}
