using System;

namespace Features.DamageableTrackModule.Scripts
{
	public class FallDamageGateModel
	{
		public bool IsArmed { get; private set; }

		public FallDamageGateIntent Intent { get; private set; } = FallDamageGateIntent.Disarmed;

		public event Action OnIntentChanged;

		public void Disarm()
		{
			IsArmed = false;
			Intent = FallDamageGateIntent.Disarmed;
			this.OnIntentChanged?.Invoke();
		}

		public void RequestArming()
		{
			Intent = FallDamageGateIntent.Arming;
			this.OnIntentChanged?.Invoke();
		}

		public void SetArmed()
		{
			IsArmed = true;
		}
	}
}
