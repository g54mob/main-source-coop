using System;
using UnityEngine;

namespace Mimicraft.Customization
{
	[Serializable]
	public class CharacterAssetDefinition
	{
		public string name;

		public CharacterAsset asset;

		public Sprite icon;

		[Tooltip("Çeviri anahtarı. Boş bırak: anahtar isimden türetilir, 'MBI Agent' için 'Preset.MBIAgent' gibi. Tabloda o satır yoksa ismin kendisi görünür.")]
		public string nameKey = "";

		public string DisplayName => PresetNames.Resolve(nameKey, name);
	}
}
