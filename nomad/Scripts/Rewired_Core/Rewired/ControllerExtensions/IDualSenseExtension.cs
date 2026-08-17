using Rewired.Interfaces;

namespace Rewired.ControllerExtensions
{
	public interface IDualSenseExtension : IDualShock4Extension, IControllerVibrator
	{
		bool SetTriggerEffect(DualSenseTriggerType trigger, IDualSenseTriggerEffect effect);

		DualSenseTriggerEffectStates GetTriggerEffectStates();
	}
}
