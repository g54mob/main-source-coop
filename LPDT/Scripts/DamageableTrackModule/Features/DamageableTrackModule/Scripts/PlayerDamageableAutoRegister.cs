using Fusion;
using UnityEngine;
using Zenject;

namespace Features.DamageableTrackModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class PlayerDamageableAutoRegister : NetworkBehaviour
	{
		[SerializeField]
		private PlayerDamageable _damageable;

		private PlayerDamageablesTrackModel _playerDamageablesTrackModel;

		private int _registeredPlayerId = -1;

		[Inject]
		public void InjectDependencies(PlayerDamageablesTrackModel playerDamageablesTrackModel)
		{
			_playerDamageablesTrackModel = playerDamageablesTrackModel;
		}

		public override void Spawned()
		{
			base.Spawned();
			_registeredPlayerId = ResolveOwnerPlayerId();
			if (_registeredPlayerId >= 0)
			{
				_playerDamageablesTrackModel.AddTrackedDamageable(_registeredPlayerId, _damageable);
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);
			if (_registeredPlayerId >= 0)
			{
				_playerDamageablesTrackModel.RemoveTrackedDamageable(_registeredPlayerId);
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
