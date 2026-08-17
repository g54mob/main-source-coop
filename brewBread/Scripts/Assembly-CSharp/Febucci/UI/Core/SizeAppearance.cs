using UnityEngine.Scripting;

namespace Febucci.UI.Core
{
	[Preserve]
	[EffectInfo("size")]
	internal class SizeAppearance : AppearanceBase
	{
		private float amplitude;

		public override void SetDefaultValues(AppearanceDefaultValues data)
		{
			effectDuration = data.defaults.sizeDuration;
			amplitude = data.defaults.sizeAmplitude * -1f + 1f;
		}

		public override void ApplyEffect(ref CharacterData data, int charIndex)
		{
			data.vertices.LerpUnclamped(data.vertices.GetMiddlePos(), Tween.EaseIn(1f - data.passedTime / effectDuration) * amplitude);
		}

		public override void SetModifier(string modifierName, string modifierValue)
		{
			base.SetModifier(modifierName, modifierValue);
			if (modifierName != null && modifierName == "a")
			{
				ApplyModifierTo(ref amplitude, modifierValue);
			}
		}
	}
}
