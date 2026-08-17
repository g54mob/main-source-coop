using System;
using Rewired.Utils.Attributes;
using UnityEngine;

namespace Rewired.ControllerExtensions
{
	[Serializable]
	[Preserve]
	public struct DualSenseTriggerEffectMultiplePositionVibration : IDualSenseTriggerEffect
	{
		[SerializeField]
		private byte _frequency;

		[SerializeField]
		private DualSenseTriggerEffectPositionValueSet _amplitude;

		public byte frequency
		{
			get
			{
				return _frequency;
			}
			set
			{
				_frequency = DualSenseTriggerEffect.Clamp(value, 0, 255);
			}
		}

		public DualSenseTriggerEffectPositionValueSet amplitude
		{
			get
			{
				return _amplitude;
			}
			set
			{
				value.prLCBAAOSyauBrnZEgWKljaIbEQpA(0, 8);
				_amplitude = value;
			}
		}

		DualSenseTriggerEffectType IDualSenseTriggerEffect.triggerEffectType => DualSenseTriggerEffectType.MultiplePositionVibration;
	}
}
