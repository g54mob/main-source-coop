using System;
using Ami.Extension;
using UnityEngine;

namespace Ami.BroAudio
{
	[Serializable]
	public struct Effect : IComparable<Effect>
	{
		public static class Defaults
		{
			public const float Volume = 1f;

			public const float LowPass = 22000f;

			public const float HighPass = 10f;
		}

		private float _value;

		public readonly EffectType Type;

		public readonly Fading Fading;

		public readonly string CustomExposedParameter;

		internal readonly bool IsDominator;

		public float Value
		{
			get
			{
				return _value;
			}
			private set
			{
				switch (Type)
				{
				case EffectType.None:
					Debug.LogError("<b><color=#F3E9D7>[BroAudio] </color></b>EffectParameter's EffectType must be set before the Value");
					break;
				case EffectType.Volume:
					_value = value.ToDecibel();
					break;
				case EffectType.LowPass:
				case EffectType.HighPass:
					if (AudioExtension.IsValidFrequency(value))
					{
						_value = value;
					}
					break;
				default:
					_value = value;
					break;
				}
			}
		}

		public static Effect HighPass(float frequency, float fadeTime = 0f, Ease ease = Ease.InCubic)
		{
			return new Effect(EffectType.HighPass, frequency, SingleFading(fadeTime, ease));
		}

		public static Effect ResetHighPass(float fadeTime = 0f, Ease ease = Ease.OutCubic)
		{
			return new Effect(EffectType.HighPass, 10f, SingleFading(fadeTime, ease));
		}

		public static Effect LowPass(float frequency, float fadeTime = 0f, Ease ease = Ease.OutCubic)
		{
			return new Effect(EffectType.LowPass, frequency, SingleFading(fadeTime, ease));
		}

		public static Effect ResetLowPass(float fadeTime = 0f, Ease ease = Ease.InCubic)
		{
			return new Effect(EffectType.LowPass, 22000f, SingleFading(fadeTime, ease));
		}

		public static Effect Custom(string exposedParameterName, float value, float fadeTime = 0f, Ease ease = Ease.Linear)
		{
			return new Effect(exposedParameterName, value, SingleFading(fadeTime, ease));
		}

		private static Fading SingleFading(float fadeTime, Ease ease)
		{
			return new Fading(fadeTime, 0f, ease, Ease.Linear);
		}

		internal Effect(EffectType type, float value, Fading fading, bool isDominator = false)
			: this(type)
		{
			Value = value;
			Fading = fading;
			IsDominator = isDominator;
		}

		internal Effect(string exposedParaName, float value, Fading fading)
			: this(EffectType.Custom, value, fading)
		{
			CustomExposedParameter = exposedParaName;
		}

		public Effect(EffectType type)
		{
			this = default(Effect);
			Type = type;
			Value = type switch
			{
				EffectType.Volume => 1f, 
				EffectType.LowPass => 300f, 
				EffectType.HighPass => 2000f, 
				_ => 0f, 
			};
		}

		public bool IsDefault()
		{
			return Type switch
			{
				EffectType.Volume => Value == 0f, 
				EffectType.LowPass => Value == 22000f, 
				EffectType.HighPass => Value == 10f, 
				_ => false, 
			};
		}

		public int CompareTo(Effect other)
		{
			if (Type != other.Type)
			{
				int type = (int)Type;
				return type.CompareTo((int)other.Type);
			}
			switch (Type)
			{
			case EffectType.Volume:
			case EffectType.HighPass:
				return Value.CompareTo(other.Value);
			case EffectType.LowPass:
				return Value.CompareTo(other.Value) * -1;
			default:
				return 0;
			}
		}
	}
}
