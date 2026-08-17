using UnityEngine;
using UnityEngine.Scripting;

namespace Febucci.UI.Core
{
	[Preserve]
	[EffectInfo("rainb")]
	internal class RainbowBehavior : BehaviorBase
	{
		private float hueShiftSpeed = 0.8f;

		private float hueShiftWaveSize = 0.08f;

		private Color32 temp;

		public override void SetDefaultValues(BehaviorDefaultValues data)
		{
			hueShiftSpeed = data.defaults.hueShiftSpeed;
			hueShiftWaveSize = data.defaults.hueShiftWaveSize;
		}

		public override void SetModifier(string modifierName, string modifierValue)
		{
			switch (modifierName)
			{
			case "f":
				ApplyModifierTo(ref hueShiftSpeed, modifierValue);
				break;
			case "s":
				ApplyModifierTo(ref hueShiftWaveSize, modifierValue);
				break;
			}
		}

		public override void ApplyEffect(ref CharacterData data, int charIndex)
		{
			for (byte b = 0; b < 4; b++)
			{
				temp = Color.HSVToRGB(Mathf.PingPong(base.time.timeSinceStart * hueShiftSpeed + (float)charIndex * hueShiftWaveSize, 1f), 1f, 1f);
				temp.a = data.colors[b].a;
				data.colors[b] = temp;
			}
		}

		public override string ToString()
		{
			return $"hueShiftSpeed: {hueShiftSpeed}\n" + $"hueShiftWaveSize: {hueShiftWaveSize}" + "\n" + base.ToString();
		}
	}
}
