using System.Collections.Generic;
using UnityEngine;

namespace Mimicraft.Customization
{
	public class WeaponSkinAsset : ScriptableObject
	{
		[Tooltip("Dosyanın içeriği - WeaponSkinFile formatında. İçe aktarma sırasında dolduruluyor.")]
		[SerializeField]
		[HideInInspector]
		private byte[] data;

		[Tooltip("Dosyanın içindeki silah kimliği. Sadece bilgi için; hangi asset olduğunu ayırt eder.")]
		[SerializeField]
		private string weaponId;

		public string WeaponId => weaponId;

		public void SetContents(byte[] bytes, string id)
		{
			data = bytes;
			weaponId = id;
		}

		public WeaponSkinData ToSkinData()
		{
			if (data == null || data.Length == 0)
			{
				return null;
			}
			if (!WeaponSkinFile.TryDecode(data, out var skins))
			{
				Debug.LogWarning("[WeaponSkinAsset] '" + base.name + "' okunamadi - bu build'in anlamadigi bir surumde olabilir.", this);
				return null;
			}
			using (Dictionary<string, WeaponSkinData>.Enumerator enumerator = skins.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					return enumerator.Current.Value;
				}
			}
			return null;
		}
	}
}
