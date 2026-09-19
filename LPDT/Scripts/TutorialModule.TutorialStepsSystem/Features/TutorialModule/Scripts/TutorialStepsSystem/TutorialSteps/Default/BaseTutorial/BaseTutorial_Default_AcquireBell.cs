using System;
using Features.Movement.Scripts;
using Features.TutorialModule.Scripts.GuideModule;
using Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialAdditionalProcessors.Default.BaseTutorial;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialSteps.Default.BaseTutorial
{
	public class BaseTutorial_Default_AcquireBell : BaseTutorialStep
	{
		private readonly BaseTutorialBellDataHolder _baseTutorialBellDataHolder;

		private readonly IGuideLineBuildService _guideLineBuildService;

		private readonly PlayerMovableModel _playerMovableModel;

		private readonly ITipService _tipService;

		private GuideLineHandle _guideLinePathHandle = GuideLineHandle.Invalid;

		private TipHandle _tipHandle = TipHandle.Invalid;

		private TipHandle _destinationTipHandle = TipHandle.Invalid;

		private TipHandle _grabTipHandle = TipHandle.Invalid;

		public override event Action OnStepEnd;

		public BaseTutorial_Default_AcquireBell(BaseTutorialBellDataHolder baseTutorialBellDataHolder, IGuideLineBuildService guideLineBuildService, PlayerMovableModel playerMovableModel, ITipService tipService)
		{
			_baseTutorialBellDataHolder = baseTutorialBellDataHolder;
			_guideLineBuildService = guideLineBuildService;
			_playerMovableModel = playerMovableModel;
			_tipService = tipService;
		}

		public override void ActivateStep()
		{
			base.ActivateStep();
			if (_baseTutorialBellDataHolder.BellEntity != null)
			{
				InitializeBellEntity(_baseTutorialBellDataHolder.BellEntity);
			}
			else
			{
				_baseTutorialBellDataHolder.OnBellEntityChanged += InitializeBellEntity;
			}
		}

		public override void Dispose()
		{
			_baseTutorialBellDataHolder.OnBellEntityChanged -= InitializeBellEntity;
			if (_baseTutorialBellDataHolder.BellEntity != null)
			{
				_baseTutorialBellDataHolder.BellEntity.ClapperGrabbable.OnGrab -= EndStep;
				_baseTutorialBellDataHolder.BellEntity.ClapperGrabbable.ForceOutline(enable: false);
			}
			_guideLineBuildService.StopTrackingPath(_guideLinePathHandle);
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

		private void InitializeBellEntity(BaseTutorialBellEntity baseTutorialBellEntity)
		{
			if (baseTutorialBellEntity == null)
			{
				return;
			}
			baseTutorialBellEntity.ClapperGrabbable.OnGrab += EndStep;
			_guideLinePathHandle = _guideLineBuildService.StartTrackingPath(baseTutorialBellEntity.BellHandle, _playerMovableModel.LocalMovable.Rigidbody.transform, GuideLineBuildType.PathFinding, GuideLineUpdateFrequencyType.EveryFrame);
			baseTutorialBellEntity.ClapperGrabbable.ForceOutline(enable: true);
			SetStepPoi(baseTutorialBellEntity.ClapperGrabbable.transform);
			if (baseTutorialBellEntity.TipHolders.TryGetValue(TipType.Arrow, out var value))
			{
				_tipHandle = _tipService.CreateTip(TipType.Arrow, value, TipFollowFlags.Position | TipFollowFlags.Rotation);
				if (baseTutorialBellEntity.TipHolders.TryGetValue(TipType.DestinationPulsing, out var value2))
				{
					_destinationTipHandle = _tipService.CreateTip(TipType.DestinationPulsing, value2, TipFollowFlags.None, 0.2f, project: true);
					_grabTipHandle = _tipService.CreateTip(TipType.GrabLeftClick, value2, TipFollowFlags.FaceCameraEntire | TipFollowFlags.Position);
				}
			}
		}
	}
}
