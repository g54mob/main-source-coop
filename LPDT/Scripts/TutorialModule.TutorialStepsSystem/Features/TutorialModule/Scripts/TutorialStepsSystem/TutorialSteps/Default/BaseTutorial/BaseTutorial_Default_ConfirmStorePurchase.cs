using System;
using Features.MultiplayerSessionServices.Scripts;
using Features.StoreModule.Scripts;
using Features.TutorialModule.Scripts.GuideModule;
using Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialAdditionalProcessors.Default.BaseTutorial;
using UnityEngine;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialSteps.Default.BaseTutorial
{
	public class BaseTutorial_Default_ConfirmStorePurchase : BaseTutorialStep
	{
		private readonly IStoreReadyRoster _storeReadyRoster;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly ITipService _tipService;

		private readonly BaseTutorialStoreDataHolder _baseTutorialStoreDataHolder;

		private readonly ActiveStoreModel _activeStoreModel;

		private TipHandle _tipHandle = TipHandle.Invalid;

		private TipHandle _destinationTipHandle = TipHandle.Invalid;

		private TipHandle _grabTipHandle = TipHandle.Invalid;

		public override event Action OnStepEnd;

		public BaseTutorial_Default_ConfirmStorePurchase(IStoreReadyRoster storeReadyRoster, MultiplayerModel multiplayerModel, ITipService tipService, BaseTutorialStoreDataHolder baseTutorialStoreDataHolder, ActiveStoreModel activeStoreModel)
		{
			_storeReadyRoster = storeReadyRoster;
			_multiplayerModel = multiplayerModel;
			_tipService = tipService;
			_baseTutorialStoreDataHolder = baseTutorialStoreDataHolder;
			_activeStoreModel = activeStoreModel;
		}

		public override void ActivateStep()
		{
			base.ActivateStep();
			_storeReadyRoster.OnPlayerReady += OnPlayerSetReady;
			if (_baseTutorialStoreDataHolder.TipHolders.TryGetValue(TipType.StoreArrow, out var value))
			{
				InitPurchaseConfirmTip(TipType.StoreArrow, value);
			}
			if (_baseTutorialStoreDataHolder.TipHolders.TryGetValue(TipType.StoreDestinationPulsing, out var value2))
			{
				InitPurchaseConfirmTip(TipType.StoreDestinationPulsing, value2);
			}
			if (_baseTutorialStoreDataHolder.TipHolders.TryGetValue(TipType.StoreGrabLeftClick, out var value3))
			{
				InitPurchaseConfirmTip(TipType.StoreGrabLeftClick, value3);
			}
			if (_activeStoreModel.ActiveStoreEntity != null)
			{
				InitRing(_activeStoreModel.ActiveStoreEntity);
			}
			else
			{
				_activeStoreModel.OnActiveStoreEntityChanged += InitRing;
			}
			_baseTutorialStoreDataHolder.OnTipHolderAdded += InitPurchaseConfirmTip;
		}

		public override void Dispose()
		{
			_storeReadyRoster.OnPlayerReady -= OnPlayerSetReady;
			_baseTutorialStoreDataHolder.OnTipHolderAdded -= InitPurchaseConfirmTip;
			_activeStoreModel.OnActiveStoreEntityChanged -= InitRing;
			if (_activeStoreModel.ActiveStoreEntity != null)
			{
				_activeStoreModel.ActiveStoreEntity.StoreActivationComponent.SimplePointGrabable.ForceOutline(enable: false);
			}
			_tipService.KillTip(_tipHandle);
			_tipService.KillTip(_destinationTipHandle);
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

		private void OnPlayerSetReady(int playerId)
		{
			if (_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId == playerId)
			{
				EndStep();
			}
		}

		private void InitPurchaseConfirmTip(TipType tipType, Transform transform)
		{
			switch (tipType)
			{
			case TipType.StoreArrow:
				_tipHandle = _tipService.CreateTip(tipType, transform, TipFollowFlags.Position | TipFollowFlags.Rotation);
				break;
			case TipType.StoreDestinationPulsing:
				_destinationTipHandle = _tipService.CreateTip(tipType, transform, TipFollowFlags.None, 0.2f, project: true);
				break;
			case TipType.StoreGrabLeftClick:
				_grabTipHandle = _tipService.CreateTip(tipType, transform, TipFollowFlags.FaceCameraEntire | TipFollowFlags.Position);
				break;
			}
		}

		private void InitRing(StoreEntity storeEntity)
		{
		}
	}
}
