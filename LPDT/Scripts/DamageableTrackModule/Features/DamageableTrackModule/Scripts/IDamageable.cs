using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace Features.DamageableTrackModule.Scripts
{
	public interface IDamageable
	{
		NetworkObject NetworkObject { get; }

		Transform Transform { get; }

		float Health { get; }

		bool IsFullHealth { get; }

		List<DamageableTag> DamageableTags { get; }

		bool IsActive { get; }

		event Action<DamageData> OnDamaged;

		event Action<bool> OnIsActiveChanged;

		void Damage(DamageData damage);

		void DamageRPC(float damage);

		void DamageRPC(float damage, int dealerPlayerID);

		void DamageRPC(float damage, int dealerPlayerID, DamageRpcSource rpcSource);

		void HealToFullValue();

		void SetDamageableTags(List<DamageableTag> entityDamageableTags);

		void Heal(float value, bool isSynchronize = false);

		void AddRPCForce(float force, Vector3 direction, ForceMode forceMode);
	}
}
