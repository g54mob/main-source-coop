using System;
using UnityEngine;

namespace Mimicraft.Customization
{
	[Serializable]
	public class WeaponSkinDefinition
	{
		public string name;

		public WeaponSkinAsset asset;

		public Sprite icon;

		[Tooltip("Kapalıyken bu preset listede hiç görünmez. Hazırlanmakta olan ya da şimdilik kaldırılmak istenen bir preseti satırı silmeden dışarıda bırakmak için - ismi, modeli ve ikonu yerinde kalır.")]
		public bool isActive = true;

		[Tooltip("Çeviri anahtarı. Boş bırak: anahtar isimden türetilir, 'Double Barreled' için 'Preset.DoubleBarreled' gibi. Tabloda o satır yoksa ismin kendisi görünür.")]
		public string nameKey = "";

		public string DisplayName => PresetNames.Resolve(nameKey, name);
	}
}
