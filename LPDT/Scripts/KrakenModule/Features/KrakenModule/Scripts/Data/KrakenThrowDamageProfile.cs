namespace Features.KrakenModule.Scripts.Data
{
	public readonly struct KrakenThrowDamageProfile
	{
		public float Damage { get; }

		public float KnockbackForce { get; }

		public float HitTrackingTime { get; }

		public bool DespawnItemAfterHit { get; }

		public float HitRadius { get; }

		public static KrakenThrowDamageProfile None => new KrakenThrowDamageProfile(0f, 0f, 0f, despawnItemAfterHit: false);

		public KrakenThrowDamageProfile(float damage, float knockbackForce, float hitTrackingTime, bool despawnItemAfterHit, float hitRadius = 0f)
		{
			Damage = damage;
			KnockbackForce = knockbackForce;
			HitTrackingTime = hitTrackingTime;
			DespawnItemAfterHit = despawnItemAfterHit;
			HitRadius = hitRadius;
		}
	}
}
