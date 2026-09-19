using System;
using System.Collections.Generic;
using System.Linq;
using Features.GameUpdaterModule;
using Features.LevelModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.QuotaModule.Scripts;
using Features.StoreModule.Scripts;
using Features.TutorialModule.Scripts.GuideModule;
using Features.TutorialModule.Scripts.TutorialStepsSystem.API.Configurations;
using Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialAdditionalProcessors.Default.BaseTutorial;
using UnityEngine;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialSteps.Default.BaseTutorial
{
	public class BaseTutorial_Default_ChooseStoreCard : BaseTutorialStep
	{
		private readonly CardsOnTableModel _cardsOnTableModel;

		private readonly IGameUpdater _gameUpdater;

		private readonly TutorialStepsConfiguration _tutorialStepsConfiguration;

		private readonly IGuideLineBuildService _guideLineBuildService;

		private readonly ActiveStoreModel _activeStoreModel;

		private readonly IStoreSeatingService _storeSeatingService;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly ITipService _tipService;

		private readonly CurrentWalletSynchronizedModel _currentWalletSynchronizedModel;

		private readonly StoreLevelConfiguration _storeLevelConfiguration;

		private readonly StorePhaseModel _storePhaseModel;

		private readonly StorePoolConfiguration _storePoolConfiguration;

		private readonly BaseTutorialStoreDataHolder _baseTutorialStoreDataHolder;

		private StoreBuyingZone _cachedStoreBuyingZone;

		private StoreBuyingZone _pendingZone;

		private StoreCardBehaviour _pendingZoneCard;

		private uint _countBoughtCards;

		private GuideLineHandle _guideLinePathHandle = GuideLineHandle.Invalid;

		private TipHandle _tipHandle = TipHandle.Invalid;

		private TipHandle _grabTipHandle = TipHandle.Invalid;

		public override event Action OnStepEnd;

		public BaseTutorial_Default_ChooseStoreCard(CardsOnTableModel cardsOnTableModel, IGameUpdater gameUpdater, TutorialStepsConfiguration tutorialStepsConfiguration, IGuideLineBuildService guideLineBuildService, ActiveStoreModel activeStoreModel, IStoreSeatingService storeSeatingService, MultiplayerModel multiplayerModel, ITipService tipService, CurrentWalletSynchronizedModel currentWalletSynchronizedModel, StoreLevelConfiguration storeLevelConfiguration, StorePhaseModel storePhaseModel, StorePoolConfiguration storePoolConfiguration, BaseTutorialStoreDataHolder baseTutorialStoreDataHolder)
		{
			_cardsOnTableModel = cardsOnTableModel;
			_gameUpdater = gameUpdater;
			_tutorialStepsConfiguration = tutorialStepsConfiguration;
			_guideLineBuildService = guideLineBuildService;
			_activeStoreModel = activeStoreModel;
			_storeSeatingService = storeSeatingService;
			_multiplayerModel = multiplayerModel;
			_tipService = tipService;
			_currentWalletSynchronizedModel = currentWalletSynchronizedModel;
			_storeLevelConfiguration = storeLevelConfiguration;
			_storePhaseModel = storePhaseModel;
			_storePoolConfiguration = storePoolConfiguration;
			_baseTutorialStoreDataHolder = baseTutorialStoreDataHolder;
		}

		public override void ActivateStep()
		{
			base.ActivateStep();
			_gameUpdater.OnUpdate += TryChooseTipCard;
			if (_activeStoreModel.ActiveStoreEntity != null)
			{
				InitStoreEntity(_activeStoreModel.ActiveStoreEntity);
			}
			else
			{
				_activeStoreModel.OnActiveStoreEntityChanged += InitStoreEntity;
			}
			if (_storePhaseModel.IsStoreActive.Value)
			{
				OnStoreLoadedChanged(_storePhaseModel.IsStoreActive.Value);
			}
			else
			{
				_storePhaseModel.OnStorePhaseChanged += OnStoreLoadedChanged;
			}
		}

		public override void Dispose()
		{
			_gameUpdater.OnUpdate -= TryChooseTipCard;
			_activeStoreModel.OnActiveStoreEntityChanged -= InitStoreEntity;
			if (_activeStoreModel.ActiveStoreEntity != null)
			{
				_activeStoreModel.ActiveStoreEntity.StoreActivationComponent.IsReadyBlocked = false;
			}
			_guideLineBuildService.StopTrackingPath(_guideLinePathHandle);
			if (_cachedStoreBuyingZone != null)
			{
				_cachedStoreBuyingZone.OnCardInStoreZone -= OnCardEnteredStoreZone;
			}
			ClearPendingCompletion();
			if (_baseTutorialStoreDataHolder.CurrentProcessedCard != null)
			{
				_baseTutorialStoreDataHolder.CurrentProcessedCard.SimplePointGrabable.OnGrab -= OnTargetCardGrabbed;
				_baseTutorialStoreDataHolder.CurrentProcessedCard.SimplePointGrabable.ForceOutline(enable: false);
			}
			_storePhaseModel.OnStorePhaseChanged -= OnStoreLoadedChanged;
			_tipService.KillTip(_tipHandle);
			_tipService.KillTip(_grabTipHandle);
		}

		private void EndStep()
		{
			OnStepEnd?.Invoke();
		}

		public override void DeActivateStep()
		{
			Dispose();
		}

		public override void SkipStep()
		{
			OnStepEnd?.Invoke();
		}

		public override void RevertStep()
		{
		}

		private void TryChooseTipCard()
		{
			if (_cardsOnTableModel.CardsOnTable.Count < _tutorialStepsConfiguration.CardsCountToChooseAsTip || _activeStoreModel.ActiveStoreEntity == null)
			{
				return;
			}
			int seatIndex = _storeSeatingService.GetSeatIndex(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId);
			if (seatIndex >= 0 && seatIndex < _activeStoreModel.ActiveStoreEntity.StoreTableBehaviour.StoreBuyingZones.Count)
			{
				List<StoreCardBehaviour> list = _cardsOnTableModel.CardsOnTable.Where((StoreCardBehaviour x) => !x.IsActivated).ToList();
				if (list.Count != 0 && TryInitializeCard(list[UnityEngine.Random.Range(0, list.Count)], _activeStoreModel.ActiveStoreEntity.StoreTableBehaviour.StoreBuyingZones[seatIndex]))
				{
					_gameUpdater.OnUpdate -= TryChooseTipCard;
				}
			}
		}

		private bool TryInitializeCard(StoreCardBehaviour storeCardBehaviour, StoreBuyingZone storeBuyingZone)
		{
			GuideLineHandle guideLinePathHandle = _guideLineBuildService.StartTrackingPath(storeBuyingZone.TargetTransform, storeCardBehaviour.Rigidbody.transform, GuideLineBuildType.StraightArc, GuideLineUpdateFrequencyType.EveryFrame);
			if (!guideLinePathHandle.IsValid)
			{
				return false;
			}
			_tipHandle = _tipService.CreateTip(TipType.StoreArrow, storeCardBehaviour.transform, TipFollowFlags.Position, 0.6f);
			_grabTipHandle = _tipService.CreateTip(TipType.StoreGrabLeftClick, storeCardBehaviour.transform, TipFollowFlags.FaceCameraEntire | TipFollowFlags.Position, 0.2f);
			storeCardBehaviour.SimplePointGrabable.ForceOutline(enable: true);
			_guideLinePathHandle = guideLinePathHandle;
			_cachedStoreBuyingZone = storeBuyingZone;
			_baseTutorialStoreDataHolder.CurrentProcessedCard = storeCardBehaviour;
			storeBuyingZone.OnCardInStoreZone += OnCardEnteredStoreZone;
			storeCardBehaviour.SimplePointGrabable.OnGrab += OnTargetCardGrabbed;
			return true;
		}

		private void OnTargetCardGrabbed()
		{
			_baseTutorialStoreDataHolder.CurrentProcessedCard.SimplePointGrabable.OnGrab += OnTargetCardGrabbed;
			_baseTutorialStoreDataHolder.CurrentProcessedCard.SimplePointGrabable.ForceOutline(enable: false);
			_tipService.KillTip(_tipHandle);
			_tipService.KillTip(_grabTipHandle);
		}

		private void OnCardEnteredStoreZone(StoreBuyingZone storeBuyingZone, StoreCardBehaviour storeCardBehaviour)
		{
			if (storeCardBehaviour.IsActivated)
			{
				CompleteCard(storeBuyingZone);
			}
			else
			{
				BeginPendingActivation(storeBuyingZone, storeCardBehaviour);
			}
		}

		private void BeginPendingActivation(StoreBuyingZone storeBuyingZone, StoreCardBehaviour storeCardBehaviour)
		{
			if (!(_pendingZoneCard == storeCardBehaviour))
			{
				ClearPendingCompletion();
				_pendingZone = storeBuyingZone;
				_pendingZoneCard = storeCardBehaviour;
				_pendingZone.OnCardInStoreZoneExit += OnPendingCardExitedZone;
				_gameUpdater.OnUpdate += TrackPendingActivation;
			}
		}

		private void TrackPendingActivation()
		{
			if (_pendingZoneCard.IsActivated)
			{
				StoreBuyingZone pendingZone = _pendingZone;
				ClearPendingCompletion();
				CompleteCard(pendingZone);
			}
		}

		private void OnPendingCardExitedZone(StoreBuyingZone storeBuyingZone, StoreCardBehaviour storeCardBehaviour)
		{
			if (!(storeCardBehaviour != _pendingZoneCard))
			{
				ClearPendingCompletion();
			}
		}

		private void ClearPendingCompletion()
		{
			if (_pendingZone != null)
			{
				_pendingZone.OnCardInStoreZoneExit -= OnPendingCardExitedZone;
			}
			_gameUpdater.OnUpdate -= TrackPendingActivation;
			_pendingZoneCard = null;
			_pendingZone = null;
		}

		private void CompleteCard(StoreBuyingZone storeBuyingZone)
		{
			storeBuyingZone.OnCardInStoreZone -= OnCardEnteredStoreZone;
			_guideLineBuildService.StopTrackingPath(_guideLinePathHandle);
			_countBoughtCards++;
			if (_countBoughtCards < _tutorialStepsConfiguration.CardsToBuyCount)
			{
				_gameUpdater.OnUpdate += TryChooseTipCard;
			}
			else
			{
				EndStep();
			}
		}

		private void InitStoreEntity(StoreEntity storeEntity)
		{
			if (!(storeEntity == null))
			{
				storeEntity.StoreActivationComponent.IsReadyBlocked = true;
			}
		}

		private void EnsureWalletNotEmpty()
		{
			_storeLevelConfiguration.StoreByLevelData.TryGetValue(LevelType.BaseTutorial_Part2, out var value);
			StoreCardData pooledReward = _storePoolConfiguration.RewardPoolWithChances.FirstOrDefault((StoreCardData x) => x.GetRewardCardItemType() == _tutorialStepsConfiguration.InteractedCardItemType);
			LevelRewardSetting levelRewardSetting = value.LevelRewardsSettings.FirstOrDefault((LevelRewardSetting x) => x.CardId == pooledReward.Id);
			if (_currentWalletSynchronizedModel.CurrentSessionMoney < (float)levelRewardSetting.CardPrice)
			{
				_currentWalletSynchronizedModel.AddQuota(levelRewardSetting.CardPrice);
			}
		}

		private void OnStoreLoadedChanged(bool isStoreLoaded)
		{
			EnsureWalletNotEmpty();
		}
	}
}
