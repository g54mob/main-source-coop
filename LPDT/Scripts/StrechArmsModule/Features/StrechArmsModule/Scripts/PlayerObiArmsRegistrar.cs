using Fusion;
using UnityEngine;
using Zenject;

namespace Features.StrechArmsModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class PlayerObiArmsRegistrar : NetworkBehaviour
	{
		[SerializeField]
		private Transform _playerObiArm;

		private PlayersArmsModel _playerArmsModel;

		[Inject]
		public void InjectDependencies(PlayersArmsModel playerArmsModel)
		{
			_playerArmsModel = playerArmsModel;
		}

		public override void Spawned()
		{
			_playerArmsModel.AddPlayerObiArm(base.Object.InputAuthority.PlayerId, _playerObiArm);
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			_playerArmsModel.RemovePlayerObiArm(base.Object.InputAuthority.PlayerId);
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
