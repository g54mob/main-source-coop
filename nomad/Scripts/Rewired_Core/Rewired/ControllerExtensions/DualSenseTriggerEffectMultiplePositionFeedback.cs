using System;
using Rewired.Utils.Attributes;
using UnityEngine;

namespace Rewired.ControllerExtensions
{
	[Serializable]
	[Preserve]
	public struct DualSenseTriggerEffectMultiplePositionFeedback : IDualSenseTriggerEffect
	{
		[SerializeField]
		private DualSenseTriggerEffectPositionValueSet _strength;

		public DualSenseTriggerEffectPositionValueSet strength
		{
			get
			{
				return _strength;
			}
			set
			{
				value.prLCBAAOSyauBrnZEgWKljaIbEQpA(0, 8);
				_strength = value;
			}
		}

		DualSenseTriggerEffectType IDualSenseTriggerEffect.triggerEffectType => DualSenseTriggerEffectType.MultiplePositionFeedback;
	}
}
