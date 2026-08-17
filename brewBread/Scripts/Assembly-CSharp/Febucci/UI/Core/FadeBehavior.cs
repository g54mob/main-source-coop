using UnityEngine;
using UnityEngine.Scripting;

namespace Febucci.UI.Core
{
	[Preserve]
	[EffectInfo("fade")]
	internal class FadeBehavior : BehaviorBase
	{
		private float delay = 0.3f;

		private float[] charPCTs;

		private Color32 temp;

		public override void SetDefaultValues(BehaviorDefaultValues data)
		{
			delay = data.defaults.fadeDelay;
		}

		public override void Initialize(int charactersCount)
		{
			base.Initialize(charactersCount);
			charPCTs = new float[charactersCount];
		}

		public override void SetModifier(string modifierName, string modifierValue)
		{
			if (modifierName != null && modifierName == "d")
			{
				ApplyModifierTo(ref delay, modifierValue);
			}
		}

		public override void ApplyEffect(ref CharacterData data, int charIndex)
		{
			if (data.passedTime <= delay)
			{
				return;
			}
			charPCTs[charIndex] += base.time.deltaTime;
			if (charPCTs[charIndex] > 1f)
			{
				charPCTs[charIndex] = 1f;
			}
			if (charPCTs[charIndex] < 1f && charPCTs[charIndex] >= 0f)
			{
				for (int i = 0; i < 4; i++)
				{
					temp = data.colors[i];
					temp.a = 0;
					data.colors[i] = Color32.LerpUnclamped(data.colors[i], temp, Tween.EaseInOut(charPCTs[charIndex]));
				}
			}
			else
			{
				for (int j = 0; j < 4; j++)
				{
					temp = data.colors[j];
					temp.a = 0;
					data.colors[j] = temp;
				}
			}
		}

		public override string ToString()
		{
			return $"delay: {delay}\n" + "\n" + base.ToString();
		}
	}
}
