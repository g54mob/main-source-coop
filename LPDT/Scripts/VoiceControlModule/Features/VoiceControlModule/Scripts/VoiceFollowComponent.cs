using Features.Movement.Scripts;
using Fusion;
using Zenject;

namespace Features.VoiceControlModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class VoiceFollowComponent : NetworkBehaviour
	{
		private PlayerMovableModel _playerMovableModel;

		[Inject]
		private void InjectDependencies(PlayerMovableModel playerMovableModel)
		{
			_playerMovableModel = playerMovableModel;
		}

		private void FixedUpdate()
		{
			if (_playerMovableModel.AllCharacterMovables.TryGetValue(base.Object.InputAuthority, out var value) && !(value.CameraPositionTransform == null))
			{
				base.transform.position = value.CameraPositionTransform.position;
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
