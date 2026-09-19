using System.Collections.Generic;
using TMPro;

namespace Features.CustomFontAssetCreatorModule.Scripts
{
	public class CustomFontAssetModel
	{
		public readonly Dictionary<FallbackFontType, TMP_FontAsset> GeneratedFonts = new Dictionary<FallbackFontType, TMP_FontAsset>();

		public readonly Dictionary<PrimaryFontType, List<TMP_FontAsset>> AppliedPrimaryFallbacks = new Dictionary<PrimaryFontType, List<TMP_FontAsset>>();
	}
}
