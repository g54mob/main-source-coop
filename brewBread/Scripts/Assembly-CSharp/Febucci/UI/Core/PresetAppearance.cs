using UnityEngine;
using UnityEngine.Scripting;

namespace Febucci.UI.Core
{
	[Preserve]
	[EffectInfo("")]
	internal class PresetAppearance : AppearanceBase
	{
		internal abstract class Effector
		{
			protected abstract Vector3 _EvaluateEffect(float passedTime, int charInde);

			public Vector3 EvaluateEffect(float passedTime, int charIndex)
			{
				return _EvaluateEffect(passedTime, charIndex);
			}
		}

		internal sealed class ThreeAxisEffector : Effector
		{
			private EffectEvaluator x;

			private EffectEvaluator y;

			private EffectEvaluator z;

			public ThreeAxisEffector(EffectEvaluator x, EffectEvaluator y, EffectEvaluator z)
			{
				this.x = x;
				this.y = y;
				this.z = z;
			}

			protected override Vector3 _EvaluateEffect(float passedTime, int charIndex)
			{
				return new Vector3(x.Evaluate(passedTime, charIndex), y.Evaluate(passedTime, charIndex), z.Evaluate(passedTime, charIndex));
			}
		}

		internal sealed class TwoAxisEffector : Effector
		{
			private EffectEvaluator x;

			private EffectEvaluator y;

			public TwoAxisEffector(EffectEvaluator x, EffectEvaluator y)
			{
				this.x = x;
				this.y = y;
			}

			protected override Vector3 _EvaluateEffect(float passedTime, int charIndex)
			{
				return new Vector3(x.Evaluate(passedTime, charIndex), y.Evaluate(passedTime, charIndex), 1f);
			}
		}

		private bool enabled;

		private Matrix4x4 matrix;

		private Vector3 offset;

		private Quaternion rotationQua;

		private bool hasTransformEffects;

		private ThreeAxisEffector movement;

		private ThreeAxisEffector rotation;

		private TwoAxisEffector scale;

		private bool setColor;

		private Color32 color;

		private ColorCurve colorCurve;

		public override void SetDefaultValues(AppearanceDefaultValues data)
		{
			effectDuration = 0f;
			enabled = false;
			if (TAnimBuilder.GetPresetFromArray(base.effectTag, data.presets, out var result))
			{
				AssignValues(result);
			}
			else if (TAnimBuilder.TryGetGlobalPresetAppearance(base.effectTag, out result))
			{
				AssignValues(result);
			}
			void AssignValues(PresetAppearanceValues values)
			{
				enabled = SetPreset(isAppearance: true, values, ref movement, ref effectDuration, ref rotation, ref scale, ref rotationQua, ref hasTransformEffects, ref setColor, ref colorCurve);
			}
		}

		public static bool SetPreset<T>(bool isAppearance, T values, ref ThreeAxisEffector movement, ref float showDuration, ref ThreeAxisEffector rotation, ref TwoAxisEffector scale, ref Quaternion rotationQua, ref bool hasTransformEffects, ref bool setColor, ref ColorCurve colorCurve) where T : PresetBaseValues
		{
			values.Initialize(isAppearance);
			showDuration = values.GetMaxDuration();
			movement = new ThreeAxisEffector(values.movementX, values.movementY, values.movementZ);
			scale = new TwoAxisEffector(values.scaleX, values.scaleY);
			rotation = new ThreeAxisEffector(values.rotX, values.rotY, values.rotZ);
			rotationQua = Quaternion.identity;
			hasTransformEffects = values.movementX.enabled || values.movementY.enabled || values.movementZ.enabled || values.rotX.enabled || values.rotY.enabled || values.rotZ.enabled || values.scaleX.enabled || values.scaleY.enabled;
			setColor = values.color.enabled;
			if (setColor)
			{
				colorCurve = values.color;
				colorCurve.Initialize(isAppearance);
			}
			return hasTransformEffects | setColor;
		}

		public override void ApplyEffect(ref CharacterData data, int charIndex)
		{
			if (!enabled)
			{
				return;
			}
			if (hasTransformEffects)
			{
				offset = (data.vertices[0] + data.vertices[2]) / 2f;
				rotationQua.eulerAngles = rotation.EvaluateEffect(data.passedTime, charIndex);
				matrix.SetTRS(movement.EvaluateEffect(data.passedTime, charIndex) * uniformIntensity, rotationQua, scale.EvaluateEffect(data.passedTime, charIndex));
				for (byte b = 0; b < data.vertices.Length; b++)
				{
					data.vertices[b] -= offset;
					data.vertices[b] = matrix.MultiplyPoint3x4(data.vertices[b]);
					data.vertices[b] += offset;
				}
			}
			if (setColor)
			{
				color = colorCurve.GetColor(data.passedTime, charIndex);
				data.colors.LerpUnclamped(color, 1f - data.passedTime / effectDuration);
			}
		}
	}
}
