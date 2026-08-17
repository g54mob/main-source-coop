using UnityEngine;
using UnityEngine.Scripting;

namespace Febucci.UI.Core
{
	[Preserve]
	[EffectInfo("wiggle")]
	internal class WiggleBehavior : BehaviorBase
	{
		private float amplitude = 0.15f;

		private float frequency = 7.67f;

		private Vector3[] directions;

		public override void SetDefaultValues(BehaviorDefaultValues data)
		{
			amplitude = data.defaults.wiggleAmplitude;
			frequency = data.defaults.wiggleFrequency;
		}

		public override void Initialize(int charactersCount)
		{
			base.Initialize(charactersCount);
			directions = new Vector3[charactersCount];
			for (int i = 0; i < charactersCount; i++)
			{
				directions[i] = TextUtilities.fakeRandoms[Random.Range(0, 24)] * Mathf.Sign(Mathf.Sin(i));
			}
		}

		public override void SetModifier(string modifierName, string modifierValue)
		{
			switch (modifierName)
			{
			case "a":
				ApplyModifierTo(ref amplitude, modifierValue);
				break;
			case "f":
				ApplyModifierTo(ref frequency, modifierValue);
				break;
			}
		}

		public override void ApplyEffect(ref CharacterData data, int charIndex)
		{
			data.vertices.MoveChar(directions[charIndex] * Mathf.Sin(base.time.timeSinceStart * frequency + (float)charIndex) * amplitude * uniformIntensity);
		}

		public override string ToString()
		{
			return $"freq: {frequency}\n" + $"ampl: {amplitude}" + "\n" + base.ToString();
		}
	}
}
