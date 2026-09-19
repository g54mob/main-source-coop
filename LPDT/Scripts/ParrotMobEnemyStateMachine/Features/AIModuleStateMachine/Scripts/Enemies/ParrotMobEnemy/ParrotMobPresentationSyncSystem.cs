using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Fusion;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.ParrotMobEnemy
{
	[NetworkBehaviourWeaved(0)]
	public class ParrotMobPresentationSyncSystem : MonoSystem
	{
		private ParrotMobEnemy _enemy;

		private ParrotMobEnemyContext _context;

		public override bool IsEnabled => true;

		[Inject]
		public void InjectDependencies(ParrotMobEnemy enemy, ParrotMobEnemyContext context)
		{
			_enemy = enemy;
			_context = context;
		}

		public override void Enable()
		{
		}

		public override void Disable()
		{
		}

		public override void Clear()
		{
		}

		public override void FixedUpdateNetwork()
		{
			if (base.Initialized && base.HasStateAuthority && !_context.IsDead)
			{
				ParrotMobStateId currentStateId = _enemy.CurrentStateId;
				bool flag = currentStateId == ParrotMobStateId.Alert || currentStateId == ParrotMobStateId.Scream;
				_context.SetLookAtPresentationActive(flag);
				if (flag)
				{
					_context.SetCurrentTargetFromPlayer(_context.PriorityPlayer);
				}
				else
				{
					_context.SetCurrentTargetPlayerId(-1);
				}
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			base.CopyBackingFieldsToState(P_0);
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
		}
	}
}
