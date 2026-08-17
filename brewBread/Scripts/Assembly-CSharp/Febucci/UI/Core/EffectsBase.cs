using System;
using System.Collections.Generic;

namespace Febucci.UI.Core
{
	public abstract class EffectsBase
	{
		internal class RegionManager
		{
			private struct TextRegion
			{
				public int startIndex;

				public int endIndex;

				public TextRegion(int startIndex)
				{
					this.startIndex = startIndex;
					endIndex = int.MaxValue;
				}
			}

			public string entireRichTextTag;

			private List<TextRegion> textRegions = new List<TextRegion>();

			internal bool IsLastRegionClosed()
			{
				if (textRegions.Count > 0)
				{
					return textRegions[textRegions.Count - 1].endIndex != int.MaxValue;
				}
				return false;
			}

			internal void AddRegion(int startIndex)
			{
				textRegions.Add(new TextRegion(startIndex));
			}

			internal bool TryReutilizingWithTag(string richTextTag, int indexNewRegionStart)
			{
				if (!entireRichTextTag.Equals(richTextTag))
				{
					return false;
				}
				if (!IsLastRegionClosed())
				{
					return true;
				}
				AddRegion(indexNewRegionStart);
				return true;
			}

			internal void CloseEffect(int index)
			{
				TextRegion value = textRegions[textRegions.Count - 1];
				value.endIndex = index;
				textRegions[textRegions.Count - 1] = value;
			}

			internal bool IsCharInsideRegion(int charIndex)
			{
				foreach (TextRegion textRegion in textRegions)
				{
					if (charIndex >= textRegion.startIndex && charIndex < textRegion.endIndex)
					{
						return true;
					}
				}
				return false;
			}

			public override string ToString()
			{
				string text = $"{entireRichTextTag} - {textRegions.Count} region(s): ";
				for (int i = 0; i < textRegions.Count; i++)
				{
					text = ((textRegions[i].endIndex != int.MaxValue) ? (text + $"[{textRegions[i].startIndex}; {textRegions[i].endIndex}] ") : (text + $"[{textRegions[i].startIndex}; Infinity] "));
				}
				return text;
			}
		}

		public float uniformIntensity = 1f;

		internal RegionManager regionManager;

		public string effectTag { get; private set; }

		[Obsolete("This value will be removed from next versions. Please use 'uniformIntensity' instead")]
		public float effectIntensity => uniformIntensity;

		internal void _Initialize(string effectTag, string entireRichTextTag)
		{
			this.effectTag = effectTag;
			regionManager = new RegionManager();
			regionManager.entireRichTextTag = entireRichTextTag;
		}

		protected void ApplyModifierTo(ref float value, string modifierValue)
		{
			if (FormatUtils.ParseFloat(modifierValue, out var result))
			{
				value *= result;
			}
		}

		public virtual void Initialize(int charactersCount)
		{
		}

		public virtual void Calculate()
		{
		}

		public abstract void ApplyEffect(ref CharacterData data, int charIndex);

		public abstract void SetModifier(string modifierName, string modifierValue);
	}
}
