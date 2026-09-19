using System;
using System.Collections.Generic;
using Global.Modules.LocalizationModule.Scripts.Generated;

namespace Features.CustomFontAssetCreatorModule.Scripts
{
	[Serializable]
	public class FontData
	{
		public FallbackFontType FontType;

		public List<Language> Languages;
	}
}
