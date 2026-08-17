using System.Collections.Generic;

namespace Febucci.UI.Core
{
	internal static class TAnimExtensions
	{
		internal static int GetIndexOfEffectNamed<T>(this List<T> effects, string tag) where T : EffectsBase
		{
			for (int num = effects.Count - 1; num >= 0; num--)
			{
				if (!effects[num].regionManager.IsLastRegionClosed() && effects[num].effectTag.Equals(tag))
				{
					return num;
				}
			}
			return -1;
		}

		internal static bool CloseElement<T>(this List<T> effects, int listIndex, int realTextIndex) where T : EffectsBase
		{
			if (listIndex < 0 || listIndex >= effects.Count || effects[listIndex].regionManager.IsLastRegionClosed())
			{
				return false;
			}
			effects[listIndex].regionManager.CloseEffect(realTextIndex);
			return true;
		}

		internal static bool CloseRegionNamed<T>(this List<T> effects, string endTag, int realTextIndex) where T : EffectsBase
		{
			return effects.CloseElement(effects.GetIndexOfEffectNamed(endTag), realTextIndex);
		}

		internal static bool TryAddingNewRegion<T>(this List<T> effects, T region) where T : EffectsBase
		{
			for (int i = 0; i < effects.Count; i++)
			{
				if (!effects[i].regionManager.IsLastRegionClosed() && effects[i].regionManager.entireRichTextTag.Equals(region.regionManager.entireRichTextTag))
				{
					return false;
				}
			}
			effects.Add(region);
			return true;
		}

		internal static bool CloseSingleOrAllEffects<T>(this List<T> effects, string closureTag, int realTextIndex) where T : EffectsBase
		{
			bool result = false;
			if (closureTag.Length <= 1)
			{
				for (int i = 0; i < effects.Count; i++)
				{
					if (effects.CloseElement(i, realTextIndex))
					{
						result = true;
					}
				}
			}
			else
			{
				result = effects.CloseRegionNamed(closureTag, realTextIndex);
			}
			return result;
		}
	}
}
