using Fusion;
using UnityEngine;
using Zenject;

namespace Features.RagdollModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class PlayerRagdollAutoRegister : NetworkBehaviour
	{
		[SerializeField]
		private PlayerRagdollEntity _playerRagdoll;

		private PlayersRagdollModel _playersRagdollModel;

		private int _registeredPlayerId = -1;

		[Inject]
		public void InjectDependencies(PlayersRagdollModel playersRagdollModel)
		{
			_playersRagdollModel = playersRagdollModel;
		}

		public override void Spawned()
		{
			base.Spawned();
			_registeredPlayerId = ResolveOwnerPlayerId();
			if (_registeredPlayerId >= 0)
			{
				_playersRagdollModel.RegisterPlayerRagdoll(_registeredPlayerId, _playerRagdoll);
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);
			if (_registeredPlayerId >= 0)
			{
				_playersRagdollModel.UnregisterPlayerRagdoll(_registeredPlayerId);
				_registeredPlayerId = -1;
			}
		}

		private int ResolveOwnerPlayerId()
		{
			PlayerRef playerRef = base.Object.InputAuthority;
			if (playerRef == PlayerRef.None)
			{
				playerRef = base.Object.StateAuthority;
			}
			if (!(playerRef == PlayerRef.None))
			{
				return playerRef.PlayerId;
			}
			return -1;
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
