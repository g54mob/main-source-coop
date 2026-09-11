using Unity.Mathematics;
using UnityEngine;
using Zorro.ControllerSupport;
using Zorro.Core;
using Zorro.Settings.DebugUI;

namespace Zorro.Settings
{
	public abstract class FloatSetting : Setting
	{
		public float Value { get; protected set; }

		public float MinValue { get; protected set; }

		public float MaxValue { get; protected set; }

		public float SliderAmount { get; protected set; } = 0.1f;

		public override void Load(ISettingsSaveLoad loader)
		{
			if (loader.TryLoadFloat(GetType(), out var value))
			{
				Value = value;
			}
			else
			{
				Debug.LogWarning("Failed to load setting of type " + GetType().FullName + " from PlayerPrefs.");
				Value = GetDefaultValue();
			}
			float2 minMaxValue = GetMinMaxValue();
			MinValue = minMaxValue.x;
			MaxValue = minMaxValue.y;
		}

		public override void Save(ISettingsSaveLoad saver)
		{
			saver.SaveFloat(GetType(), Value);
		}

		public override SettingUI GetDebugUI(ISettingHandler settingHandler)
		{
			return new FloatSettingUI(this, settingHandler);
		}

		public override GameObject GetSettingUICell()
		{
			return SingletonAsset<InputCellMapper>.Instance.FloatSettingCell;
		}

		protected abstract float GetDefaultValue();

		protected abstract float2 GetMinMaxValue();

		public void SetValue(float value, ISettingHandler handler)
		{
			value = Clamp(value);
			if (InputHandler.GetCurrentUsedInputScheme() == InputScheme.KeyboardMouse)
			{
				Value = value;
				ApplyValue();
				handler.SaveSetting(this);
				return;
			}
			if (value < Value)
			{
				Value = Mathf.Clamp(Value - SliderAmount, MinValue, MaxValue);
			}
			else
			{
				Value = Mathf.Clamp(Value + SliderAmount, MinValue, MaxValue);
			}
			ApplyValue();
			handler.SaveSetting(this);
		}

		public virtual float Clamp(float value)
		{
			return Mathf.Clamp(value, MinValue, MaxValue);
		}

		public virtual string Expose(float result)
		{
			return result.ToString("F");
		}
	}
}
