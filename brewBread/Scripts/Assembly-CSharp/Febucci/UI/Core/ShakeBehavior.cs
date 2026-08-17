using UnityEngine;
using UnityEngine.Scripting;

namespace Febucci.UI.Core
{
	[Preserve]
	[EffectInfo("shake")]
	internal class ShakeBehavior : BehaviorBase
	{
		public float shakeStrength;

		public float shakeDelay;

		private float timePassed;

		private int randIndex;

		private int lastRandomIndex;

		public override void SetDefaultValues(BehaviorDefaultValues data)
		{
			shakeDelay = data.defaults.shakeDelay;
			shakeStrength = data.defaults.shakeStrength;
			ClampValues();
		}

		private void ClampValues()
		{
			shakeDelay = Mathf.Clamp(shakeDelay, 0.002f, 500f);
		}

		public override void Initialize(int charactersCount)
		{
			base.Initialize(charactersCount);
			randIndex = Random.Range(0, 25);
			lastRandomIndex = randIndex;
		}

		public override void SetModifier(string modifierName, string modifierValue)
		{
			switch (modifierName)
			{
			case "a":
				ApplyModifierTo(ref shakeStrength, modifierValue);
				break;
			case "d":
				ApplyModifierTo(ref shakeDelay, modifierValue);
				break;
			}
			ClampValues();
		}

		public override void Calculate()
		{
			timePassed += base.time.deltaTime;
			if (!(timePassed >= shakeDelay))
			{
				return;
			}
			timePassed = 0f;
			randIndex = Random.Range(0, 25);
			if (lastRandomIndex == randIndex)
			{
				randIndex++;
				if (randIndex >= 25)
				{
					randIndex = 0;
				}
			}
			lastRandomIndex = randIndex;
		}

		public override void ApplyEffect(ref CharacterData data, int charIndex)
		{
			data.vertices.MoveChar(TextUtilities.fakeRandoms[Mathf.RoundToInt((charIndex + randIndex) % 24)] * shakeStrength * uniformIntensity);
		}

		public override string ToString()
		{
			return $"shake delay: {shakeDelay}\n" + $"strength: {shakeStrength}" + "\n" + base.ToString();
		}
	}
}
