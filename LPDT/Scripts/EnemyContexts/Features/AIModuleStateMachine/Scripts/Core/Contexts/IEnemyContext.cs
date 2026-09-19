namespace Features.AIModuleStateMachine.Scripts.Core.Contexts
{
	public interface IEnemyContext : IMovementContext, IDetectionContext, IAttackTimingContext, IStateTimingContext, IStatContext
	{
		void Initialize();
	}
}
