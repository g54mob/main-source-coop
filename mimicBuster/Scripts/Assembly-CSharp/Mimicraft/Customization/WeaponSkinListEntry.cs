namespace Mimicraft.Customization
{
	public readonly struct WeaponSkinListEntry
	{
		public readonly string FilePath;

		public readonly string WeaponId;

		public readonly string SkinName;

		public readonly bool IsLegacy;

		public WeaponSkinListEntry(string filePath, string weaponId, string skinName, bool legacy)
		{
			FilePath = filePath;
			WeaponId = weaponId;
			SkinName = skinName;
			IsLegacy = legacy;
		}
	}
}
