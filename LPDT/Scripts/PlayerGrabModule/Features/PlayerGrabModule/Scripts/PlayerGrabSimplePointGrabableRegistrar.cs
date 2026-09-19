using Features.GrabModule.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.PlayerGrabModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class PlayerGrabSimplePointGrabableRegistrar : NetworkBehaviour
	{
		[SerializeField]
		private SimplePointGrabable _simplePointGrabable;

		private PlayerGrabSimplePointGrabableModel _playerGrabSimplePointGrabableModel;

		[Inject]
		private void InjectDependencies(PlayerGrabSimplePointGrabableModel playerGrabSimplePointGrabableModel)
		{
			_playerGrabSimplePointGrabableModel = playerGrabSimplePointGrabableModel;
		}

		public override void Spawned()
		{
			_playerGrabSimplePointGrabableModel.PlayerGrabables.Add(_simplePointGrabable.Object.InputAuthority.PlayerId, _simplePointGrabable);
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			_playerGrabSimplePointGrabableModel.PlayerGrabables.Remove(_simplePointGrabable.Object.InputAuthority.PlayerId);
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
