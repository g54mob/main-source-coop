using Features.AIModuleStateMachine.Scripts.Core.Damageable;
using UnityEngine;

namespace Features.AIModule.Scripts
{
	public class HealEffect : EffectBase
	{
		[SerializeField]
		private SimpleEnemyDamageable _simpleMonoDamageable;

		private void OnEnable()
		{
			_simpleMonoDamageable.OnHealed += ToggleHitEffect;
		}

		private void OnDisable()
		{
			_simpleMonoDamageable.OnHealed -= ToggleHitEffect;
		}

		private void ToggleHitEffect(float _)
		{
			ToggleEffect();
		}
	}
}
