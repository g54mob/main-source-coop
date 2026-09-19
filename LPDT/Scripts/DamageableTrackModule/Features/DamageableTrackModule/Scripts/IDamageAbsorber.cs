namespace Features.DamageableTrackModule.Scripts
{
	public interface IDamageAbsorber
	{
		bool TryAbsorb(DamageData damageData);
	}
}
