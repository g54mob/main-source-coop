using UnityEngine;
using UnityEngine.Scripting;

namespace Febucci.UI.Core
{
	[Preserve]
	[EffectInfo("diagexp")]
	internal class DiagonalExpandAppearance : AppearanceBase
	{
		private int targetA;

		private int targetB;

		private Vector3 middlePos;

		private float pct;

		public override void SetDefaultValues(AppearanceDefaultValues data)
		{
			effectDuration = data.defaults.diagonalExpandDuration;
			SetOrientation(data.defaults.diagonalFromBttmLeft);
		}

		private void SetOrientation(bool btmLeft)
		{
			if (btmLeft)
			{
				targetA = 0;
				targetB = 2;
			}
			else
			{
				targetA = 1;
				targetB = 3;
			}
		}

		public override void ApplyEffect(ref CharacterData data, int charIndex)
		{
			middlePos = data.vertices.GetMiddlePos();
			pct = Tween.EaseInOut(data.passedTime / effectDuration);
			data.vertices[targetA] = Vector3.LerpUnclamped(middlePos, data.vertices[targetA], pct);
			data.vertices[targetB] = Vector3.LerpUnclamped(middlePos, data.vertices[targetB], pct);
		}

		public override void SetModifier(string modifierName, string modifierValue)
		{
			base.SetModifier(modifierName, modifierValue);
			if (modifierName != null && modifierName == "bot")
			{
				SetOrientation(modifierValue == "1");
			}
		}
	}
}
