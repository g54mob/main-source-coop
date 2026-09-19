using Features.AIModuleStateMachine.Scripts.Core.Damageable;
using Features.DamageableTrackModule.Scripts;
using UnityEngine;

namespace Features.AIModule.Scripts
{
	public class HitEffect : EffectBase
	{
		[SerializeField]
		private SimpleEnemyDamageable _simpleMonoDamageable;

		private void OnEnable()
		{
			_simpleMonoDamageable.OnDamaged += ToggleHitEffect;
		}

		private void OnDisable()
		{
			_simpleMonoDamageable.OnDamaged -= ToggleHitEffect;
		}

		private void ToggleHitEffect(DamageData damageData)
		{
			ToggleEffect();
		}
	}
}
