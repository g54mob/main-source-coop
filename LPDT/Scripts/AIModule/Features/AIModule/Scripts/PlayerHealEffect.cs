using Features.DamageableTrackModule.Scripts;
using UnityEngine;

namespace Features.AIModule.Scripts
{
	public class PlayerHealEffect : EffectBase
	{
		[SerializeField]
		private PlayerDamageable _simpleMonoDamageable;

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
