using System.Collections.Generic;
using Global.SerializableDictionary;
using UnityEngine;

namespace Features.CustomFontAssetCreatorModule.Scripts
{
	[CreateAssetMenu(fileName = "FontsData", menuName = "Configurations/FontsData/FontsData")]
	public class FontsData : ScriptableObject
	{
		[field: SerializeField]
		public List<FontData> Datas { get; private set; }

		[field: SerializeField]
		public List<PrimaryFontBindingData> PrimaryFontBindings { get; private set; }

		[field: SerializeField]
		public SerializableDictionary<FallbackFontType, string> FontsPath { get; private set; }

		[field: SerializeField]
		public SerializableDictionary<PrimaryFontType, string> PrimaryFontsPath { get; private set; }

		[field: SerializeField]
		public List<AdditionalSpecialFontData> AdditionalSpecialFonts { get; private set; }
	}
}
