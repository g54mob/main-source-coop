using System.Collections.Generic;
using Features.MultiplayerSessionServices.Scripts;
using Features.PlayerItemViewModule.Scripts;
using Features.ViewSystemModule.Scripts.Windows;
using Features.VoiceSpeakersModule.Scripts.Data;
using JetBrains.Annotations;
using PlayerCustomization;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;
using Zenject;

namespace Features.MainMenuModule.Scripts
{
	[PublicAPI]
	public class LobbyPlayersPresenter : PresenterBehaviour<LobbyPlayersViewBase>
	{
		private readonly Dictionary<int, PlayerItemPresenter> _itemPresenters = new Dictionary<int, PlayerItemPresenter>();

		private readonly PlayerCustomizationModel _playerCustomizationModel;

		private readonly LobbyWindow _lobbyWindow;

		private readonly ActiveSpeakerModel _speakerModel;

		private readonly MultiplayerSessionConfig _multiplayerSessionConfig;

		private readonly DiContainer _container;

		public LobbyPlayersPresenter(LobbyWindow lobbyWindow, PlayerCustomizationModel playerCustomizationModel, ActiveSpeakerModel speakerModel, MultiplayerSessionConfig multiplayerSessionConfig, DiContainer container)
		{
			_playerCustomizationModel = playerCustomizationModel;
			_lobbyWindow = lobbyWindow;
			_speakerModel = speakerModel;
			_multiplayerSessionConfig = multiplayerSessionConfig;
			_container = container;
		}

		protected override void OnViewSet()
		{
			UpdatePlayerInfo();
			_speakerModel.OnPlayerSpeakerStateChanged += UpdateSpeakerState;
			_playerCustomizationModel.OnSlotsChanged += UpdatePlayerInfo;
			InitSpeakerState();
		}

		protected override void OnDisposed()
		{
			_speakerModel.OnPlayerSpeakerStateChanged -= UpdateSpeakerState;
			_playerCustomizationModel.OnSlotsChanged -= UpdatePlayerInfo;
			ClearAllItems();
		}

		private void ClearAllItems()
		{
			foreach (PlayerItemPresenter value in _itemPresenters.Values)
			{
				value.DestroyView();
			}
			_itemPresenters.Clear();
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
			}
		}

		private void UpdatePlayerInfo()
		{
			ClearUnusedItems();
			_playerCustomizationModel.SortPlayers();
			foreach (PlayerCustomizationSlotData slot in _playerCustomizationModel.Slots)
			{
				if (slot.PlayerId != -1)
				{
					ProcessPlayerCustomizationData(slot);
				}
			}
			InitSpeakerState();
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

		private static void ApplyPlayerCustomizationData(PlayerCustomizationSlotData playerCustomizationSlotData, PlayerItemPresenter existingPresenter)
		{
			existingPresenter.SetIcon(playerCustomizationSlotData.PlayerId);
			existingPresenter.SetPlayerId(playerCustomizationSlotData.PlayerId);
		}

		private void CreatePlayerItem(PlayerCustomizationSlotData playerCustomizationSlotData)
		{
			PlayerItemPresenter playerItemPresenter = CreatePlayerSlot(base.View.GetPlayerItemsContainer(), _lobbyWindow, playerCustomizationSlotData.PlayerId);
			_itemPresenters.Add(playerCustomizationSlotData.PlayerId, playerItemPresenter);
			ApplyPlayerCustomizationData(playerCustomizationSlotData, playerItemPresenter);
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
			}
			foreach (int item in list)
			{
				_itemPresenters[item].DestroyView();
				_itemPresenters.Remove(item);
			}
		}

		private PlayerItemPresenter CreatePlayerSlot(Transform parent, FocusableWindowBehaviour window, int playerId)
		{
			PlayerItemViewBase component = _container.InstantiatePrefab(base.View.PlayerItemViewBase.gameObject).GetComponent<PlayerItemViewBase>();
			_lobbyWindow.AddView(component.transform, worldPositionStays: false);
			PlayerItemPresenter presenterForView = _lobbyWindow.GetPresenterForView<PlayerItemPresenter>(component);
			presenterForView.SetOwnerWindow(_lobbyWindow);
			presenterForView.SetParent(parent);
			presenterForView.SetIcon(playerId);
			presenterForView.SetPlayerId(playerId);
			return presenterForView;
		}
	}
}
