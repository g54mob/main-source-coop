using System;
using Rewired.Utils.Attributes;
using UnityEngine;

namespace Rewired.ControllerExtensions
{
	[Serializable]
	[Preserve]
	public struct DualSenseTriggerEffectVibration : IDualSenseTriggerEffect
	{
		[SerializeField]
		private byte _position;

		[SerializeField]
		private byte _amplitude;

		[SerializeField]
		private byte _frequency;

		public byte position
		{
			get
			{
				return _position;
			}
			set
			{
				_position = DualSenseTriggerEffect.Clamp(value, 0, 9);
			}
		}

		public byte amplitude
		{
			get
			{
				return _amplitude;
			}
			set
			{
				_amplitude = DualSenseTriggerEffect.Clamp(value, 0, 8);
			}
		}

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

		DualSenseTriggerEffectType IDualSenseTriggerEffect.triggerEffectType => DualSenseTriggerEffectType.Vibration;
	}
}
