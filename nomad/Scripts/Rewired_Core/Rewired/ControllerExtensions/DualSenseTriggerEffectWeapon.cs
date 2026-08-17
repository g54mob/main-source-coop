using System;
using Rewired.Utils.Attributes;
using UnityEngine;

namespace Rewired.ControllerExtensions
{
	[Serializable]
	[Preserve]
	public struct DualSenseTriggerEffectWeapon : IDualSenseTriggerEffect
	{
		[SerializeField]
		private byte _startPosition;

		[SerializeField]
		private byte _endPosition;

		[SerializeField]
		private byte _strength;

		public byte startPosition
		{
			get
			{
				return _startPosition;
			}
			set
			{
				_startPosition = DualSenseTriggerEffect.Clamp(value, 2, 7);
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
				_endPosition = DualSenseTriggerEffect.Clamp(value, 1, 9);
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

		DualSenseTriggerEffectType IDualSenseTriggerEffect.triggerEffectType => DualSenseTriggerEffectType.Weapon;
	}
}
