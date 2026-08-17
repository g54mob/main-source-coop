using System;
using System.Runtime.InteropServices;
using Rewired.Utils.Attributes;

namespace Rewired.ControllerExtensions
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	[Preserve]
	public struct DualSenseTriggerEffectOff : IDualSenseTriggerEffect
	{
		DualSenseTriggerEffectType IDualSenseTriggerEffect.triggerEffectType => DualSenseTriggerEffectType.Off;
	}
}
