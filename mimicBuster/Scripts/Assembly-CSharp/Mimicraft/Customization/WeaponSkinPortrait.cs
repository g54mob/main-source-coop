namespace Mimicraft.Customization
{
	public sealed class WeaponSkinPortrait
	{
		public readonly string WeaponId;

		public readonly WeaponSkinData Skin;

		public WeaponSkinPortrait(string weaponId, WeaponSkinData skin)
		{
			WeaponId = weaponId;
			Skin = skin;
		}
	}
}
