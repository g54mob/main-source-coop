using System;
using Rewired.Utils.Attributes;
using UnityEngine;

namespace Rewired.ControllerExtensions
{
	[Serializable]
	[Preserve]
	public struct DualSenseTriggerEffectFeedback : IDualSenseTriggerEffect
	{
		[SerializeField]
		private byte _position;

		[SerializeField]
		private byte _strength;

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

		public byte strength
		{
			get
			{
				return _strength;
			}
			set
			{
				_strength = DualSenseTriggerEffect.Clamp(value, 0, 8);
			}
		}

		DualSenseTriggerEffectType IDualSenseTriggerEffect.triggerEffectType => DualSenseTriggerEffectType.Feedback;
	}
}
