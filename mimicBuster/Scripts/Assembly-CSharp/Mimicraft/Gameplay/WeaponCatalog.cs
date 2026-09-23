using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mimicraft.Gameplay
{
	public static class WeaponCatalog
	{
		private const string ResourceFolder = "Weapons";

		private static WeaponDefinition[] cached;

		public static IReadOnlyList<WeaponDefinition> All
		{
			get
			{
				if (cached != null)
				{
					return cached;
				}
				List<WeaponDefinition> list = new List<WeaponDefinition>();
				WeaponDefinition[] array = Resources.LoadAll<WeaponDefinition>("Weapons");
				foreach (WeaponDefinition weaponDefinition in array)
				{
					if (!(weaponDefinition == null))
					{
						if (!weaponDefinition.IsUsable)
						{
							Debug.LogWarning("[WeaponCatalog] '" + weaponDefinition.name + "' asset'inin Weapon Id'si bos - listeye alinmadi, cunku kimliksiz bir silah aga gonderilemez.");
						}
						else
						{
							list.Add(weaponDefinition);
						}
					}
				}
				list.Sort((WeaponDefinition a, WeaponDefinition b) => string.Compare(a.DisplayName, b.DisplayName, StringComparison.OrdinalIgnoreCase));
				cached = list.ToArray();
				return cached;
			}
		}

		public static WeaponDefinition Find(string weaponId)
		{
			if (string.IsNullOrWhiteSpace(weaponId))
			{
				return null;
			}
			foreach (WeaponDefinition item in All)
			{
				if (item.WeaponId == weaponId)
				{
					return item;
				}
			}
			return null;
		}
	}
}
