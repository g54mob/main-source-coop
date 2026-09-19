using System;
using System.Collections.Generic;
using Features.DeadPartsModule.Scripts;
using Features.InputModule.Scripts.Generated;
using Features.MultiplayerSessionServices.Scripts;
using Features.PlayerItemViewModule.Scripts;
using Features.PlayerStatesModule.Scripts;
using Features.SceneTransitionsModule.Scripts.LoadingScreen;
using Features.VoiceSpeakersModule.Scripts.Data;
using PlayerCustomization;
using RSG.Muffin.InputSubmodule.InputModule.Core.Scripts;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;
using Zenject;

namespace Features.PlayersStatisticsModule.Scripts.Views.Statistics
{
	public class StatisticsPresenter : PresenterBehaviour<StatisticsViewBase>
	{
		private readonly Dictionary<int, PlayerItemPresenter> _itemPresenters = new Dictionary<int, PlayerItemPresenter>();

		private readonly DiContainer _container;

		private StatisticWindow _statisticWindow;

		private readonly PlayerCustomizationModel _playerCustomizationModel;

		private readonly ActiveSpeakerModel _speakerModel;

		private readonly IInputService _inputService;

		private readonly ILoadingScreenService _loadingScreenService;

		private readonly PlayersStatesSynchronizer _playersStatesSynchronizer;

		private readonly PlayerDeadPartTypes _playerDeadPartTypes;

		private readonly PlayerDeadPartsConfiguration _playerDeadPartsConfiguration;

		private readonly PlayerDeadPartModel _playerDeadPartModel;

		private readonly IMultiplayerService _multiplayerService;

		private readonly LoadingScreenModel _loadingScreenModel;

		public StatisticsPresenter(PlayerCustomizationModel playerCustomizationModel, ActiveSpeakerModel speakerModel, DiContainer container, StatisticWindow statisticWindow, IInputService inputService, ILoadingScreenService loadingScreenService, PlayersStatesSynchronizer playersStatesSynchronizer, PlayerDeadPartTypes playerDeadPartTypes, PlayerDeadPartsConfiguration playerDeadPartsConfiguration, PlayerDeadPartModel playerDeadPartModel, IMultiplayerService multiplayerService, LoadingScreenModel loadingScreenModel)
		{
			_playerCustomizationModel = playerCustomizationModel;
			_speakerModel = speakerModel;
			_container = container;
			_statisticWindow = statisticWindow;
			_inputService = inputService;
			_loadingScreenService = loadingScreenService;
			_playersStatesSynchronizer = playersStatesSynchronizer;
			_playerDeadPartTypes = playerDeadPartTypes;
			_playerDeadPartsConfiguration = playerDeadPartsConfiguration;
			_playerDeadPartModel = playerDeadPartModel;
			_multiplayerService = multiplayerService;
			_loadingScreenModel = loadingScreenModel;
		}

		protected override void OnViewSet()
		{
			UpdatePlayerInfo();
			_speakerModel.OnPlayerSpeakerStateChanged += UpdateSpeakerState;
			_playerCustomizationModel.OnSlotsChanged += UpdatePlayerInfo;
			InitSpeakerState();
			_loadingScreenModel.OnEndedFadeOut += TriggerAnimation;
		}

		private void TriggerAnimation()
		{
			_loadingScreenModel.OnEndedFadeOut -= TriggerAnimation;
			base.View.WindowAnimator.SetTrigger("Activate");
		}

		protected override void OnDisposed()
		{
			_speakerModel.OnPlayerSpeakerStateChanged -= UpdateSpeakerState;
			_playerCustomizationModel.OnSlotsChanged -= UpdatePlayerInfo;
			_loadingScreenModel.OnEndedFadeOut -= TriggerAnimation;
		}

		protected override void OnViewEnabled()
		{
			base.OnViewEnabled();
			InputDefaultActions uIApply = _inputService.UIApply;
			uIApply.Performed = (Action)Delegate.Combine(uIApply.Performed, new Action(OnUIApply));
		}

		protected override void OnViewDisabled()
		{
			base.OnViewDisabled();
			InputDefaultActions uIApply = _inputService.UIApply;
			uIApply.Performed = (Action)Delegate.Remove(uIApply.Performed, new Action(OnUIApply));
		}

		private void OnUIApply()
		{
			InputDefaultActions uIApply = _inputService.UIApply;
			uIApply.Performed = (Action)Delegate.Remove(uIApply.Performed, new Action(OnUIApply));
			if (_statisticWindow.WindowStatus == WindowStatus.Showed)
			{
				if (_loadingScreenService.IsBlackoutRaised || _loadingScreenService.ActiveShowType == LoadingScreenShowType.ShowFade)
				{
					CloseWindowWithoutLoadingScreen();
				}
				else
				{
					_loadingScreenService.Show(LoadingScreenShowType.ShowFade, CloseWindowWithLoadingScreen);
				}
			}
		}

		private void CloseWindowWithoutLoadingScreen()
		{
			_statisticWindow.Close();
			_inputService.EnableArmMap();
		}

		private void CloseWindowWithLoadingScreen()
		{
			_statisticWindow.Close();
			_loadingScreenService.Hide(LoadingScreenShowType.ShowFade);
			_inputService.EnableArmMap();
		}

		private static void ApplyPlayerCustomizationData(PlayerCustomizationSlotData playerCustomizationSlotData, PlayerItemPresenter existingPresenter)
		{
			existingPresenter.SetIcon(playerCustomizationSlotData.PlayerId);
			existingPresenter.SetPlayerId(playerCustomizationSlotData.PlayerId);
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

		private void InitSpeakerState()
		{
			foreach (KeyValuePair<int, VoiceActivityStatus> item in _speakerModel.SpeakersActiveStatus)
			{
				UpdateSpeakerState(item.Key, item.Value);
			}
		}

		private void UpdateSpeakerState(int player, VoiceActivityStatus voiceActivityStatus)
		{
			if (!_itemPresenters.TryGetValue(player, out var value))
			{
				return;
			}
			value.SetSpeakerStatus(voiceActivityStatus);
			if (_playersStatesSynchronizer.TryGetState(player, out var state) && (state == PlayerState.Dead || state == PlayerState.PreDeadCrouch))
			{
				value.SetSpectatorView(isActive: true);
				value.SetDeadColor(isDead: true);
				return;
			}
			value.SetSpectatorView(isActive: false);
			if (TryGetAlivePart(player, out var alivePart))
			{
				DeadPartType deadPartType = _playerDeadPartTypes.GetDeadPartType(player);
				int num = _playerDeadPartsConfiguration.MaxDeadPartUsageCount[deadPartType];
				bool deadPartDamaged = num - alivePart.DeadPartUsageCount != num;
				value.SetDeadPartDamaged(deadPartDamaged);
			}
		}

		private bool TryGetAlivePart(int playerId, out PlayerAlivePart alivePart)
		{
			alivePart = null;
			if (_multiplayerService.TryGetPlayerRefById(playerId, out var playerRef) && _playerDeadPartModel.AllPlayersAliveParts.TryGetValue(playerRef, out alivePart))
			{
				return alivePart != null;
			}
			return false;
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

		private void CreatePlayerItem(PlayerCustomizationSlotData playerCustomizationSlotData)
		{
			PlayerItemPresenter playerItemPresenter = CreatePlayerSlot(base.View.GetPlayerItemsContainer(), _statisticWindow, playerCustomizationSlotData.PlayerId);
			_itemPresenters.Add(playerCustomizationSlotData.PlayerId, playerItemPresenter);
			ApplyPlayerCustomizationData(playerCustomizationSlotData, playerItemPresenter);
		}

		private PlayerItemPresenter CreatePlayerSlot(Transform parent, FocusableWindowBehaviour window, int playerId)
		{
			GameObject gameObject = _container.InstantiatePrefab(base.View.PlayerItemViewBase.gameObject);
			PlayerItemViewBase component = gameObject.GetComponent<PlayerItemViewBase>();
			_statisticWindow.AddView(component.transform, worldPositionStays: false);
			PlayerItemPresenter presenterForView = _statisticWindow.GetPresenterForView<PlayerItemPresenter>(component);
			presenterForView.SetOwnerWindow(_statisticWindow);
			presenterForView.SetIcon(playerId);
			presenterForView.SetPlayerId(playerId);
			StatisticsPlayerViewBase component2 = gameObject.GetComponent<StatisticsPlayerViewBase>();
			_statisticWindow.AddView(component.transform, worldPositionStays: false);
			_statisticWindow.GetPresenterForView<StatisticsPlayerPresenter>(component2).SetPlayerId(playerId);
			presenterForView.SetParent(parent);
			return presenterForView;
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
	}
}
