namespace Mimicraft.Customization
{
	public static class WeaponSounds
	{
		public static WeaponSoundKind FromByte(byte value)
		{
			if (value != 1)
			{
				return WeaponSoundKind.Normal;
			}
			return WeaponSoundKind.Suppressed;
		}
	}
}
