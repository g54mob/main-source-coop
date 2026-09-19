using Features.DamageableTrackModule.Scripts;
using Fusion;

namespace Features.AIModule.Scripts
{
	public static class EnemyTargetValidationExtensions
	{
		public static bool IsActiveInSession(this PlayerRef player, NetworkRunner runner)
		{
			if (player == PlayerRef.None || runner == null)
			{
				return false;
			}
			foreach (PlayerRef activePlayer in runner.ActivePlayers)
			{
				if (activePlayer == player)
				{
					return true;
				}
			}
			return false;
		}

		public static bool IsSpawnedAndValid(this NetworkObject networkObject)
		{
			if (networkObject != null)
			{
				return networkObject.IsValid;
			}
			return false;
		}

		public static bool IsSpawnedAndValid(this NetworkBehaviour networkBehaviour)
		{
			if (networkBehaviour != null && networkBehaviour.Object != null)
			{
				return networkBehaviour.Object.IsValid;
			}
			return false;
		}

		public static bool IsValidDamageTarget(this IDamageable damageable)
		{
			if (damageable == null || !damageable.IsActive)
			{
				return false;
			}
			if (damageable is NetworkBehaviour networkBehaviour)
			{
				return networkBehaviour.IsSpawnedAndValid();
			}
			return false;
		}
	}
}
