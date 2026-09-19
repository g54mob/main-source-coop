namespace Features.HingeModule.Scripts
{
	public interface IChestScreamerLidGate
	{
		bool ShouldSuppressHingeAngleBreak { get; }

		bool ShouldSuppressHingeDespawn { get; }

		bool ShouldSuppressStandUpOpenImpulse { get; }
	}
}
