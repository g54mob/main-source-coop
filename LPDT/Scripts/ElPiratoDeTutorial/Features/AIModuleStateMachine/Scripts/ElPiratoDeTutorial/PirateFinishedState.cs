using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.ElPiratoDeTutorial
{
	public class PirateFinishedState : StateBase<PirateStateId>
	{
		private readonly PirateEnemyContext _pirateEnemyContext;

		public PirateFinishedState(PirateEnemyContext pirateEnemyContext)
			: base(false, false)
		{
			_pirateEnemyContext = pirateEnemyContext;
		}

		public override void OnEnter()
		{
			_pirateEnemyContext.EnemyMovableBase.ResetPath();
			_pirateEnemyContext.ApplyWalkingSpeed();
			_pirateEnemyContext.TargetEnemy = null;
		}
	}
}
