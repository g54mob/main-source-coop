namespace Features.AIModuleStateMachine.Scripts.Core.Contexts
{
	public interface IFacingSpeedLimitContext
	{
		float FacingMoveSpeedMultiplier { get; }

		float FacingMoveAngle { get; }

		void SetFacingMoveSpeedMultiplier(float multiplier, float angle);
	}
}
