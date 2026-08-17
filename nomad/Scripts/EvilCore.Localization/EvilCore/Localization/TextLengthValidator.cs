using UnityEngine;

namespace EvilCore.Localization
{
	internal class TextLengthValidator
	{
		private readonly int _defaultMaxCharacters;

		private readonly float _expansionWarningThreshold;

		public TextLengthValidator(int defaultMaxCharacters, float expansionWarningThreshold)
		{
			_defaultMaxCharacters = defaultMaxCharacters;
			_expansionWarningThreshold = expansionWarningThreshold;
		}

		public void Validate(string key, string localizedText)
		{
			if (!string.IsNullOrEmpty(localizedText) && localizedText.Length > _defaultMaxCharacters)
			{
				Debug.LogWarning($"[Localization] Text for \"{key}\" exceeds max length ({localizedText.Length}/{_defaultMaxCharacters})");
			}
		}

		public void ValidateExpansion(string key, string localizedText, string baseText)
		{
			if (!string.IsNullOrEmpty(localizedText) && !string.IsNullOrEmpty(baseText))
			{
				float num = (float)localizedText.Length / (float)baseText.Length;
				if (num > _expansionWarningThreshold)
				{
					Debug.LogWarning($"[Localization] Text expansion for \"{key}\": {num:F1}x " + $"(threshold: {_expansionWarningThreshold:F1}x) " + $"base={baseText.Length} localized={localizedText.Length}");
				}
			}
		}
	}
}
