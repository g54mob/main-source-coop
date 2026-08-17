using UnityEngine;
using UnityEngine.Scripting;

namespace Febucci.UI.Core
{
	[Preserve]
	[EffectInfo("fade")]
	internal class FadeAppearance : AppearanceBase
	{
		private Color32 temp;

		public override void SetDefaultValues(AppearanceDefaultValues data)
		{
			effectDuration = data.defaults.fadeDuration;
		}

		public override void ApplyEffect(ref CharacterData data, int charIndex)
		{
			for (int i = 0; i < 4; i++)
			{
				temp = data.colors[i];
				temp.a = 0;
				data.colors[i] = Color32.LerpUnclamped(data.colors[i], temp, Tween.EaseInOut(1f - data.passedTime / effectDuration));
			}
		}
	}
}
