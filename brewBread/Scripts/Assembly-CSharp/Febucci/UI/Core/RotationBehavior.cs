using UnityEngine.Scripting;

namespace Febucci.UI.Core
{
	[Preserve]
	[EffectInfo("rot")]
	internal class RotationBehavior : BehaviorBase
	{
		private float angleSpeed = 180f;

		private float angleDiffBetweenChars = 10f;

		public override void SetDefaultValues(BehaviorDefaultValues data)
		{
			angleSpeed = data.defaults.angleSpeed;
			angleDiffBetweenChars = data.defaults.angleDiffBetweenChars;
		}

		public override void SetModifier(string modifierName, string modifierValue)
		{
			switch (modifierName)
			{
			case "f":
				ApplyModifierTo(ref angleSpeed, modifierValue);
				break;
			case "s":
				ApplyModifierTo(ref angleDiffBetweenChars, modifierValue);
				break;
			}
		}

		public override void ApplyEffect(ref CharacterData data, int charIndex)
		{
			data.vertices.RotateChar((0f - base.time.timeSinceStart) * angleSpeed + angleDiffBetweenChars * (float)charIndex);
		}

		public override string ToString()
		{
			return $"angleSpeed: {angleSpeed}\n" + $"angleDiffBetweenChars: {angleDiffBetweenChars}" + "\n" + base.ToString();
		}
	}
}
