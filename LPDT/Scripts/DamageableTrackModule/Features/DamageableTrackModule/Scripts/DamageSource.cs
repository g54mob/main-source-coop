namespace Features.DamageableTrackModule.Scripts
{
	public readonly struct DamageSource
	{
		public DamageCauseCategory Category { get; }

		public DamageOwner Owner { get; }

		public DamageType Type { get; }

		public DamageSource(DamageCauseCategory category, DamageOwner owner, DamageType type)
		{
			Category = category;
			Owner = owner;
			Type = type;
		}
	}
}
