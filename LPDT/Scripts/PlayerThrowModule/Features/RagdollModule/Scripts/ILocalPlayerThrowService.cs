using System;

namespace Features.RagdollModule.Scripts
{
	public interface ILocalPlayerThrowService
	{
		event Action OnThrowEntered;

		void EnterThrow(StunDurationPreset stunDurationPreset = StunDurationPreset.Default);

		StunDurationPreset ConsumePendingStunPreset();

		void ExitThrow(bool isGoingToDead);
	}
}
