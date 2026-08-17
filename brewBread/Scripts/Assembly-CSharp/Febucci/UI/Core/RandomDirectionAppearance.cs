using UnityEngine;
using UnityEngine.Scripting;

namespace Febucci.UI.Core
{
	[Preserve]
	[EffectInfo("rdir")]
	internal class RandomDirectionAppearance : AppearanceBase
	{
		private float amount;

		private Vector3[] directions;

		public override void Initialize(int charactersCount)
		{
			base.Initialize(charactersCount);
			directions = new Vector3[charactersCount];
			for (int i = 0; i < charactersCount; i++)
			{
				directions[i] = TextUtilities.fakeRandoms[Random.Range(0, 24)] * Mathf.Sign(Mathf.Sin(i));
			}
		}

		public override void SetDefaultValues(AppearanceDefaultValues data)
		{
			amount = data.defaults.randomDirAmplitude;
			effectDuration = data.defaults.randomDirDuration;
		}

		public override void ApplyEffect(ref CharacterData data, int charIndex)
		{
			data.vertices.MoveChar(directions[charIndex] * amount * uniformIntensity * Tween.EaseIn(1f - data.passedTime / effectDuration));
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
