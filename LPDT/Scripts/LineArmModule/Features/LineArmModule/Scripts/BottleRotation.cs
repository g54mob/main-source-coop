using Features.CollectingModule.Scripts.New;
using Features.Movement.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.LineArmModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class BottleRotation : NetworkBehaviour
	{
		[SerializeField]
		private PhysicsItemUpVectorLimiter _physicsItemUpVectorLimiter;

		private PlayerMovableModel _playerMovableModel;

		private bool _isInitialized;

		[Inject]
		private void InjectDependencies(PlayerMovableModel playerMovableModel)
		{
			_playerMovableModel = playerMovableModel;
		}

		public override void Spawned()
		{
			_isInitialized = true;
		}

		private void LateUpdate()
		{
			if (_isInitialized && !(_playerMovableModel.LocalMovable == null))
			{
				Vector3 normalized = (_playerMovableModel.LocalMovable.CameraPositionTransform.position - base.transform.position).normalized;
				_physicsItemUpVectorLimiter.CurrentUpVector = normalized;
			}
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
