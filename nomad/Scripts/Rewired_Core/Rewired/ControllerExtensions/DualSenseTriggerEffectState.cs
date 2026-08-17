namespace Rewired.ControllerExtensions
{
	public enum DualSenseTriggerEffectState
	{
		Unknown = -1,
		Off = 0,
		FeedbackIdle = 1,
		FeedbackApplyingForce = 2,
		WeaponIdle = 3,
		WeaponFiring = 4,
		WeaponFired = 5,
		VibrationIdle = 6,
		VibrationVibrating = 7
	}
}
