using System;
using UnityEngine;

namespace Mimicraft.Customization
{
	public static class WeaponSkinPortraitService
	{
		public static string StudioSceneName
		{
			get
			{
				return PortraitService<WeaponSkinStudio, WeaponSkinPortrait>.StudioSceneName;
			}
			set
			{
				PortraitService<WeaponSkinStudio, WeaponSkinPortrait>.StudioSceneName = value;
			}
		}

		public static float KeepLoadedSeconds
		{
			get
			{
				return PortraitService<WeaponSkinStudio, WeaponSkinPortrait>.KeepLoadedSeconds;
			}
			set
			{
				PortraitService<WeaponSkinStudio, WeaponSkinPortrait>.KeepLoadedSeconds = value;
			}
		}

		public static Vector3 StudioOffset
		{
			get
			{
				return PortraitService<WeaponSkinStudio, WeaponSkinPortrait>.StudioOffset;
			}
			set
			{
				PortraitService<WeaponSkinStudio, WeaponSkinPortrait>.StudioOffset = value;
			}
		}

		public static int Pending => PortraitService<WeaponSkinStudio, WeaponSkinPortrait>.Pending;

		public static event Action<string> PortraitWritten
		{
			add
			{
				PortraitService<WeaponSkinStudio, WeaponSkinPortrait>.PortraitWritten += value;
			}
			remove
			{
				PortraitService<WeaponSkinStudio, WeaponSkinPortrait>.PortraitWritten -= value;
			}
		}

		static WeaponSkinPortraitService()
		{
			PortraitService<WeaponSkinStudio, WeaponSkinPortrait>.StudioSceneName = "WeaponStudio";
		}

		public static void Request(string skinFilePath, string weaponId, WeaponSkinData skin)
		{
			if (!string.IsNullOrEmpty(weaponId) && skin != null)
			{
				PortraitService<WeaponSkinStudio, WeaponSkinPortrait>.Request(skinFilePath, new WeaponSkinPortrait(weaponId, skin));
			}
		}

		public static int RequestMissing(string weaponId = null, bool all = false)
		{
			int num = 0;
			foreach (WeaponSkinListEntry item in WeaponSkinStorage.List(weaponId))
			{
				if (all || !SavedThumbnails.Exists(item.FilePath))
				{
					string weaponId2;
					string skinName;
					WeaponSkinData weaponSkinData = WeaponSkinStorage.LoadFile(item.FilePath, out weaponId2, out skinName);
					if (weaponSkinData != null)
					{
						Request(item.FilePath, item.WeaponId, weaponSkinData);
						num++;
					}
				}
			}
			return num;
		}
	}
}
