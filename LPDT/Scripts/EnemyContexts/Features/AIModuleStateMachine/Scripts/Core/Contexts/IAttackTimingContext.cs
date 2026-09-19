namespace Features.AIModuleStateMachine.Scripts.Core.Contexts
{
	public interface IAttackTimingContext
	{
		float AttackCooldown { get; set; }

		bool IsAttackOnCooldown { get; set; }

		float TimeToAttack { get; set; }

		float DistanceToAttack { get; set; }
	}
}
