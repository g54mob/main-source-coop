using System;
using System.Collections.Generic;

namespace Features.CustomFontAssetCreatorModule.Scripts
{
	[Serializable]
	public class PrimaryFontBindingData
	{
		public PrimaryFontType PrimaryFontType;

		public List<FallbackFontType> FontsToSet;
	}
}
