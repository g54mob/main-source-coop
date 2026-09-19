using UnityEngine;

namespace Features.DamageableTrackModule.Scripts
{
	public static class DamageableExtensions
	{
		public static void ApplyStunHit(this IDamageable damageable, float damage, Vector3 knockbackDirection, float force, int dealerPlayerId, DamageRpcSource rpcSource, DamageSource source, bool stunPlayers, float stunThrowMultiplier, float playerKnockbackUpBias)
		{
			if (damageable == null)
			{
				return;
			}
			if (stunPlayers && damageable is PlayerDamageable)
			{
				Vector3 vector = knockbackDirection;
				vector.y = 0f;
				if (vector.sqrMagnitude < 1E-06f)
				{
					vector = Vector3.forward;
				}
				float num = ((stunThrowMultiplier > 0f) ? stunThrowMultiplier : 1f);
				Vector3 normalized = (vector.normalized + Vector3.up * playerKnockbackUpBias).normalized;
				damageable.Damage(new DamageData
				{
					Damage = damage,
					Direction = normalized,
					Force = force * num,
					DamageDealerPlayerID = dealerPlayerId,
					ForceMode = ForceMode.Impulse,
					IsStunning = true,
					Source = source
				});
			}
			else
			{
				damageable.DamageRPC(damage, dealerPlayerId, rpcSource);
				damageable.AddRPCForce(force, knockbackDirection, ForceMode.Impulse);
			}
		}
	}
}
