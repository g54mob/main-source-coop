namespace Features.WeaponModule.Scripts
{
	public interface IWeapon
	{
		bool TryToAttack();

		bool IsOnCooldown();

		float GetRemainingCooldown();
	}
}
