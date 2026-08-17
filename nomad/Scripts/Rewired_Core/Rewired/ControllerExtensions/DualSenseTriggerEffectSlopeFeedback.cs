using System;
using Rewired.Utils.Attributes;
using UnityEngine;

namespace Rewired.ControllerExtensions
{
	[Serializable]
	[Preserve]
	public struct DualSenseTriggerEffectSlopeFeedback : IDualSenseTriggerEffect
	{
		[SerializeField]
		private byte _startPosition;

		[SerializeField]
		private byte _endPosition;

		[SerializeField]
		private byte _startStrength;

		[SerializeField]
		private byte _endStrength;

		public byte startPosition
		{
			get
			{
				return _startPosition;
			}
			set
			{
				_startPosition = DualSenseTriggerEffect.Clamp(value, 0, 9);
			}
		}

		public byte endPosition
		{
			get
			{
				return _endPosition;
			}
			set
			{
				_endPosition = DualSenseTriggerEffect.Clamp(value, 0, 9);
			}
		}

		public byte startStrength
		{
			get
			{
				return _startStrength;
			}
			set
			{
				_startStrength = DualSenseTriggerEffect.Clamp(value, 1, 8);
			}
		}

		public byte endStrength
		{
			get
			{
				return _endStrength;
			}
			set
			{
				_endStrength = DualSenseTriggerEffect.Clamp(value, 1, 8);
			}
		}

		DualSenseTriggerEffectType IDualSenseTriggerEffect.triggerEffectType => DualSenseTriggerEffectType.SlopeFeedback;
	}
}
