using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace EvilCore.Localization
{
	[CreateAssetMenu(menuName = "EvilCore/Localization/Font Config", fileName = "LocaleFontConfig")]
	public class LocaleFontConfig : ScriptableObject
	{
		[Serializable]
		public class LocaleFontMapping
		{
			public string localeCode;

			public TMP_FontAsset font;

			public TMP_FontAsset fontBold;
		}

		[SerializeField]
		private TMP_FontAsset defaultFont;

		[SerializeField]
		private List<LocaleFontMapping> mappings = new List<LocaleFontMapping>();

		public TMP_FontAsset DefaultFont => defaultFont;

		public TMP_FontAsset GetFont(string localeCode, string style = null)
		{
			if (string.IsNullOrEmpty(localeCode))
			{
				return defaultFont;
			}
			string b = localeCode;
			int num = localeCode.IndexOf('-');
			if (num > 0)
			{
				b = localeCode.Substring(0, num);
			}
			foreach (LocaleFontMapping mapping in mappings)
			{
				if (string.Equals(mapping.localeCode, localeCode, StringComparison.OrdinalIgnoreCase) || string.Equals(mapping.localeCode, b, StringComparison.OrdinalIgnoreCase))
				{
					if (style == "bold" && mapping.fontBold != null)
					{
						return mapping.fontBold;
					}
					return (mapping.font != null) ? mapping.font : defaultFont;
				}
			}
			return defaultFont;
		}
	}
}
