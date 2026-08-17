using UnityEngine;
using UnityEngine.Scripting;

namespace Febucci.UI.Core
{
	[Preserve]
	[EffectInfo("offset")]
	internal class OffsetAppearance : AppearanceBase
	{
		private float amount;

		private Vector2 direction;

		public override void SetDefaultValues(AppearanceDefaultValues data)
		{
			direction = data.defaults.offsetDir;
			amount = data.defaults.offsetAmplitude * uniformIntensity;
			effectDuration = data.defaults.offsetDuration;
		}

		public override void ApplyEffect(ref CharacterData data, int charIndex)
		{
			data.vertices.MoveChar(direction * amount * Tween.EaseIn(1f - data.passedTime / effectDuration));
		}

		public override void SetModifier(string modifierName, string modifierValue)
		{
			base.SetModifier(modifierName, modifierValue);
			if (modifierName != null && modifierName == "a")
			{
				ApplyModifierTo(ref amount, modifierValue);
			}
		}
	}
}
