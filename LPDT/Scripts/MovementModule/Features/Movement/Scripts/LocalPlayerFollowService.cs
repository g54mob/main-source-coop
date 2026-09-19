using Features.CameraModelModule;
using Features.RagdollModule.Scripts;
using UnityEngine;

namespace Features.Movement.Scripts
{
	public class LocalPlayerFollowService : ILocalPlayerFollowService
	{
		private readonly CameraModel _cameraModel;

		private readonly PlayerMovableModel _playerMovableModel;

		public LocalPlayerFollowService(CameraModel cameraModel, PlayerMovableModel playerMovableModel)
		{
			_cameraModel = cameraModel;
			_playerMovableModel = playerMovableModel;
		}

		public void EnterFollowMode()
		{
			Transform cameraPositionTransform = _playerMovableModel.LocalMovable.CameraPositionTransform;
			_cameraModel.Cameras[Features.CameraModelModule.CameraType.FPFollowCamera].SetTrackingTarget(cameraPositionTransform);
			_cameraModel.Cameras[Features.CameraModelModule.CameraType.FPFollowCamera].SetLookAtTarget(_playerMovableModel.Rotator.RotationObject);
			_cameraModel.AddPerlinDisableReason(PerlinDisableReasonEnum.Follow);
			_cameraModel.AddCameraReason(Features.CameraModelModule.CameraType.FPFollowCamera, CameraReasonEnum.Follow);
			_playerMovableModel.LocalMovable.IsAutomaticForwardMovement = true;
			_playerMovableModel.LocalMovable.ForceCrouch(isCrouching: false);
		}

		public void ExitFollowMode()
		{
			_cameraModel.RemoveCameraReason(Features.CameraModelModule.CameraType.FPFollowCamera, CameraReasonEnum.Follow);
			_cameraModel.RemovePerlinDisableReason(PerlinDisableReasonEnum.Follow);
			_playerMovableModel.LocalMovable.IsAutomaticForwardMovement = false;
		}
	}
}
