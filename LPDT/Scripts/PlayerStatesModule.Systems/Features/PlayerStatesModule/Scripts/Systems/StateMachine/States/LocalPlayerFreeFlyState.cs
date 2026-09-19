using Features.CameraModelModule;
using Features.LevelLightModule.Scripts;
using Features.Movement.Scripts;
using UnityEngine;

namespace Features.PlayerStatesModule.Scripts.Systems.StateMachine.States
{
	public class LocalPlayerFreeFlyState : LocalPlayerStateBase
	{
		public override PlayerState StateEnum => PlayerState.FreeFly;

		protected override void OnEnter()
		{
			LightObjectsGroupService.SetLightObjectGroupState(LightObjectGroup.Player, state: true);
			CameraModel.AddCameraReason(Features.CameraModelModule.CameraType.FPCamera, CameraReasonEnum.FreeFly);
			Transform currentTarget = ((PlayerMovableModel.FreeFlyMovable != null) ? PlayerMovableModel.FreeFlyMovable.transform : PlayerMovableModel.LocalMovable.CameraPositionTransform);
			CameraModel.Cameras[Features.CameraModelModule.CameraType.FPCamera].SetTrackingTarget(currentTarget);
			CameraModel.Cameras[Features.CameraModelModule.CameraType.TPCamera].SetTrackingTarget(currentTarget);
			PlayerMovableModel.AddLockMovementReason(LockMovementReasonEnum.FreeFly);
			CameraModel.AddPerlinDisableReason(PerlinDisableReasonEnum.FreeFly);
			CloseSpectatorAndDeathWindows();
			RequestMouseHidden();
			SetCommonHudVisible(isVisible: true);
			SetDefaultLineArmController();
		}

		protected override void OnExit()
		{
			CameraModel.RemoveCameraReason(Features.CameraModelModule.CameraType.FPCamera, CameraReasonEnum.FreeFly);
			CameraModel.RemovePerlinDisableReason(PerlinDisableReasonEnum.FreeFly);
			PlayerMovableModel.RemoveLockMovementReason(LockMovementReasonEnum.FreeFly);
			Transform cameraPositionTransform = PlayerMovableModel.LocalMovable.CameraPositionTransform;
			CameraModel.Cameras[Features.CameraModelModule.CameraType.FPCamera].SetTrackingTarget(cameraPositionTransform, forceUpdate: true);
			CameraModel.Cameras[Features.CameraModelModule.CameraType.TPCamera].SetTrackingTarget(cameraPositionTransform, forceUpdate: true);
			CameraModel.Cameras[Features.CameraModelModule.CameraType.FPFollowCamera].SetTrackingTarget(cameraPositionTransform, forceUpdate: true);
			CameraModel.Cameras[Features.CameraModelModule.CameraType.FPFollowCamera].SetLookAtTarget(PlayerMovableModel.Rotator.RotationObject);
		}
	}
}
