using Features.DamageableTrackModule.Scripts;
using UnityEngine;

namespace Features.AIModule.Scripts
{
	public class PlayerHitEffect : EffectBase
	{
		[SerializeField]
		private PlayerDamageable _simpleMonoDamageable;

		private void OnEnable()
		{
			_simpleMonoDamageable.OnDamaged += ToggleHitEffect;
		}

		private void OnDisable()
		{
			_simpleMonoDamageable.OnDamaged -= ToggleHitEffect;
		}

		private void ToggleHitEffect(DamageData _)
		{
			ToggleEffect();
		}
	}
}
