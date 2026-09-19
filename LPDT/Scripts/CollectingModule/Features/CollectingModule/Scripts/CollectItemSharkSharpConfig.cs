using FMODUnity;
using Features.ItemsModule.Scripts;
using UnityEngine;

namespace Features.CollectingModule.Scripts
{
	[CreateAssetMenu(menuName = "Items/CollectItemSharkSharpConfig", fileName = "CollectItemSharkSharpConfig", order = 0)]
	public class CollectItemSharkSharpConfig : ItemConfig
	{
		[SerializeField]
		private float _enemiesDamage = 50f;

		[SerializeField]
		private float _itemsDamage = 5f;

		[SerializeField]
		private float _forceStrength = 20f;

		[SerializeField]
		private float _hitCooldown = 0.5f;

		[SerializeField]
		private float _grabSafeTime = 0.3f;

		[SerializeField]
		private EventReference _hitDamageableSound;

		[SerializeField]
		private EventReference _hitEnvironmentSound;

		public float EnemiesDamage => _enemiesDamage;

		public float ItemsDamage => _itemsDamage;

		public float ForceStrength => _forceStrength;

		public float HitCooldown => _hitCooldown;

		public float GrabSafeTime => _grabSafeTime;

		public EventReference HitDamageableSound => _hitDamageableSound;

		public EventReference HitEnvironmentSound => _hitEnvironmentSound;
	}
}
