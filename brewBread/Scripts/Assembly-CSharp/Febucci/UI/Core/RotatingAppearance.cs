using UnityEngine;
using UnityEngine.Scripting;

namespace Febucci.UI.Core
{
	[Preserve]
	[EffectInfo("rot")]
	internal class RotatingAppearance : AppearanceBase
	{
		private float targetAngle;

		public override void SetDefaultValues(AppearanceDefaultValues data)
		{
			effectDuration = data.defaults.rotationDuration;
			targetAngle = data.defaults.rotationStartAngle;
		}

		public override void ApplyEffect(ref CharacterData data, int charIndex)
		{
			data.vertices.RotateChar(Mathf.Lerp(targetAngle, 0f, Tween.EaseInOut(data.passedTime / effectDuration)));
		}

		public override void SetModifier(string modifierName, string modifierValue)
		{
			base.SetModifier(modifierName, modifierValue);
			if (modifierName != null && modifierName == "a")
			{
				ApplyModifierTo(ref targetAngle, modifierValue);
			}
		}
	}
}
