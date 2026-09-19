using Features.MultiplayerSessionServices.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.DeadPartsModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class PlayerDeadPartsCollision : NetworkBehaviour
	{
		private MultiplayerModel _multiplayerModel;

		private PlayerDeadPartModel _playerDeadPartModel;

		[Inject]
		public void InjectDependencies(MultiplayerModel multiplayerModel, PlayerDeadPartModel playerDeadPartModel)
		{
			_multiplayerModel = multiplayerModel;
			_playerDeadPartModel = playerDeadPartModel;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (base.Object == null || _multiplayerModel.NetworkRunner == null || base.Object.InputAuthority != _multiplayerModel.NetworkRunner.LocalPlayer)
			{
				return;
			}
			if (other.TryGetComponent<PlayerDeadPart>(out var component))
			{
				if (!component.IsUsed)
				{
					_playerDeadPartModel.RegisterDeadPart(component);
				}
				return;
			}
			PlayerDeadPart componentInParent = other.GetComponentInParent<PlayerDeadPart>();
			if (componentInParent != null && !componentInParent.IsUsed)
			{
				_playerDeadPartModel.RegisterDeadPart(componentInParent);
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (!(base.Object.InputAuthority != _multiplayerModel.NetworkRunner.LocalPlayer))
			{
				if (other.TryGetComponent<PlayerDeadPart>(out var component))
				{
					_playerDeadPartModel.RemoveDeadPart(component);
				}
				PlayerDeadPart componentInParent = other.GetComponentInParent<PlayerDeadPart>();
				if (componentInParent != null)
				{
					_playerDeadPartModel.RemoveDeadPart(componentInParent);
				}
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
