using UnityEngine;
using UnityEngine.Scripting;

namespace Febucci.UI.Core
{
	[Preserve]
	[EffectInfo("vertexp")]
	internal class VerticalExpandAppearance : AppearanceBase
	{
		private int startA;

		private int targetA;

		private int startB;

		private int targetB;

		private float pct;

		public override void SetDefaultValues(AppearanceDefaultValues data)
		{
			effectDuration = data.defaults.verticalExpandDuration;
			SetOrientation(data.defaults.verticalFromBottom);
		}

		private void SetOrientation(bool fromBottom)
		{
			if (fromBottom)
			{
				startA = 0;
				targetA = 1;
				startB = 3;
				targetB = 2;
			}
			else
			{
				startA = 1;
				targetA = 0;
				startB = 2;
				targetB = 3;
			}
		}

		public override void ApplyEffect(ref CharacterData data, int charIndex)
		{
			pct = Tween.EaseInOut(data.passedTime / effectDuration);
			data.vertices[targetA] = Vector3.LerpUnclamped(data.vertices[startA], data.vertices[targetA], pct);
			data.vertices[targetB] = Vector3.LerpUnclamped(data.vertices[startB], data.vertices[targetB], pct);
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
