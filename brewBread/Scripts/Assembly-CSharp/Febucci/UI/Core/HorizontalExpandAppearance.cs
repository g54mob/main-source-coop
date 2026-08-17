using UnityEngine;
using UnityEngine.Scripting;

namespace Febucci.UI.Core
{
	[Preserve]
	[EffectInfo("horiexp")]
	internal class HorizontalExpandAppearance : AppearanceBase
	{
		public enum ExpType
		{
			Left = 0,
			Middle = 1,
			Right = 2
		}

		private ExpType type;

		private Vector2 startTop;

		private Vector2 startBot;

		private float pct;

		public override void SetDefaultValues(AppearanceDefaultValues data)
		{
			effectDuration = data.defaults.horizontalExpandDuration;
			type = data.defaults.horizontalExpandStart;
		}

		public override void ApplyEffect(ref CharacterData data, int charIndex)
		{
			pct = Tween.EaseInOut(data.passedTime / effectDuration);
			switch (type)
			{
			default:
				startTop = data.vertices[1];
				startBot = data.vertices[0];
				data.vertices[2] = Vector3.LerpUnclamped(startTop, data.vertices[2], pct);
				data.vertices[3] = Vector3.LerpUnclamped(startBot, data.vertices[3], pct);
				break;
			case ExpType.Right:
				startTop = data.vertices[2];
				startBot = data.vertices[3];
				data.vertices[1] = Vector3.LerpUnclamped(startTop, data.vertices[1], pct);
				data.vertices[0] = Vector3.LerpUnclamped(startBot, data.vertices[0], pct);
				break;
			case ExpType.Middle:
				startTop = (data.vertices[1] + data.vertices[2]) / 2f;
				startBot = (data.vertices[0] + data.vertices[3]) / 2f;
				data.vertices[1] = Vector3.LerpUnclamped(startTop, data.vertices[1], pct);
				data.vertices[2] = Vector3.LerpUnclamped(startTop, data.vertices[2], pct);
				data.vertices[0] = Vector3.LerpUnclamped(startBot, data.vertices[0], pct);
				data.vertices[3] = Vector3.LerpUnclamped(startBot, data.vertices[3], pct);
				break;
			}
		}

		public override void SetModifier(string modifierName, string modifierValue)
		{
			base.SetModifier(modifierName, modifierValue);
			if (modifierName != null && modifierName == "x")
			{
				switch (modifierValue)
				{
				case "-1":
					type = ExpType.Left;
					return;
				case "0":
					type = ExpType.Middle;
					return;
				case "1":
					type = ExpType.Right;
					return;
				}
				Debug.LogError("Text Animator: you set an '" + modifierName + "' modifier with value '" + modifierValue + "' for the HorizontalExpandAppearance effect, but it can only be '-1', '0', or '1'");
			}
		}
	}
}
