using System;
using TMPro;

namespace Features.CustomUiElementsModule.Scripts
{
	[Serializable]
	public struct TextAutoSizeEntry
	{
		public TMP_Text Text;

		public TextAutoSizeMode Mode;

		public float FontSizeMultiplier;
	}
}
