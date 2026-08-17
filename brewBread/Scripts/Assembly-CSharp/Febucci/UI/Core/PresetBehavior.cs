using UnityEngine;
using UnityEngine.Scripting;

namespace Febucci.UI.Core
{
	[Preserve]
	[EffectInfo("")]
	internal class PresetBehavior : BehaviorBase
	{
		private bool enabled;

		private float timeSpeed;

		private float weightMult;

		private Matrix4x4 matrix;

		private Vector3 offset;

		private Quaternion rotationQua;

		private float uniformEffectTime;

		private bool hasTransformEffects;

		private bool isOnOneCharacter;

		private float weight = 1f;

		private EmissionControl emissionControl;

		private PresetAppearance.ThreeAxisEffector movement;

		private PresetAppearance.ThreeAxisEffector rotation;

		private PresetAppearance.TwoAxisEffector scale;

		private bool setColor;

		private Color32 color;

		private ColorCurve colorCurve;

		public override void SetDefaultValues(BehaviorDefaultValues data)
		{
			weightMult = 1f;
			timeSpeed = 1f;
			uniformEffectTime = 0f;
			weight = 0f;
			isOnOneCharacter = false;
			enabled = false;
			if (TAnimBuilder.GetPresetFromArray(base.effectTag, data.presets, out var result))
			{
				AssignValues(result);
			}
			else if (TAnimBuilder.TryGetGlobalPresetBehavior(base.effectTag, out result))
			{
				AssignValues(result);
			}
			void AssignValues(PresetBehaviorValues presetBehaviorValues)
			{
				float showDuration = 0f;
				emissionControl = presetBehaviorValues.emission;
				enabled = PresetAppearance.SetPreset(isAppearance: false, presetBehaviorValues, ref movement, ref showDuration, ref rotation, ref scale, ref rotationQua, ref hasTransformEffects, ref setColor, ref colorCurve);
				emissionControl.Initialize(showDuration);
			}
		}

		public override void SetModifier(string modifierName, string modifierValue)
		{
			switch (modifierName)
			{
			case "f":
				ApplyModifierTo(ref timeSpeed, modifierValue);
				break;
			case "a":
				ApplyModifierTo(ref weightMult, modifierValue);
				break;
			}
		}

		public override void Calculate()
		{
			if (isOnOneCharacter)
			{
				uniformEffectTime = emissionControl.IncreaseEffectTime(base.time.deltaTime * timeSpeed);
			}
		}

		public override void ApplyEffect(ref CharacterData data, int charIndex)
		{
			if (!enabled)
			{
				return;
			}
			if (!isOnOneCharacter)
			{
				isOnOneCharacter = data.passedTime > 0f;
			}
			weight = emissionControl.effectWeigth * weightMult;
			if (weight == 0f)
			{
				return;
			}
			if (hasTransformEffects)
			{
				offset = (data.vertices[0] + data.vertices[2]) / 2f;
				rotationQua.eulerAngles = rotation.EvaluateEffect(uniformEffectTime, charIndex) * weight;
				matrix.SetTRS(movement.EvaluateEffect(uniformEffectTime, charIndex) * uniformIntensity * weight, rotationQua, Vector3.LerpUnclamped(Vector3.one, scale.EvaluateEffect(uniformEffectTime, charIndex), weight));
				for (byte b = 0; b < data.vertices.Length; b++)
				{
					data.vertices[b] -= offset;
					data.vertices[b] = matrix.MultiplyPoint3x4(data.vertices[b]);
					data.vertices[b] += offset;
				}
			}
			if (setColor)
			{
				color = colorCurve.GetColor(uniformEffectTime, charIndex);
				data.colors.LerpUnclamped(color, Mathf.Clamp(weight, -1f, 1f));
			}
		}
	}
}
