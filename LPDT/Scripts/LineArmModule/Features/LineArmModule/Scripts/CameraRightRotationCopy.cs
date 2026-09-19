using Features.CameraModelModule;
using Features.Movement.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.LineArmModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class CameraRightRotationCopy : NetworkBehaviour
	{
		[Header("References")]
		public Transform target;

		[SerializeField]
		public bool _ignoreYRotation;

		[SerializeField]
		private float _armEndMoveSpeed = 5f;

		private CameraModel _cameraModel;

		private bool _isInitialized;

		private PlayerMovableModel _playerMovableModel;

		[Inject]
		private void InjectDependencies(CameraModel cameraModel, PlayerMovableModel playerMovableModel)
		{
			_cameraModel = cameraModel;
			_playerMovableModel = playerMovableModel;
		}

		public override void Spawned()
		{
			_isInitialized = true;
		}

		private void LateUpdate()
		{
			if (!_isInitialized)
			{
				return;
			}
			if (base.Object.InputAuthority != base.Runner.LocalPlayer)
			{
				if (_playerMovableModel.AllCharacterMovables.ContainsKey(base.Object.InputAuthority))
				{
					Vector3 forward = _playerMovableModel.AllCharacterMovables[base.Object.InputAuthority].RotatoblePart.forward;
					ProcessRotation(forward);
				}
			}
			else if (!(_cameraModel.CameraObject == null) && !(target == null))
			{
				Vector3 forward2 = _cameraModel.AimTransform.forward;
				ProcessRotation(forward2);
			}
		}

		private void ProcessRotation(Vector3 targetDirection)
		{
			if (_ignoreYRotation)
			{
				targetDirection.y = 0f;
				targetDirection.Normalize();
			}
			target.right = Vector3.Lerp(target.right, targetDirection, Time.deltaTime * _armEndMoveSpeed);
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}
	}
}
