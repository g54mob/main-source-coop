using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Features.EntitiesSoundOcclusionModule.Scripts.EnemiesSoundOcclusion;
using Fusion;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.SleeperEnemy.Systems
{
	[NetworkBehaviourWeaved(0)]
	public class SleeperSoundAggroSystem : MonoSystem
	{
		private SleeperEnemyContext _context;

		private SleeperEnemy _sleeperEnemy;

		private bool _isEnabled;

		public override bool IsEnabled => _isEnabled;

		[Inject]
		public void InjectDependencies(SleeperEnemyContext context, SleeperEnemy sleeperEnemy)
		{
			_context = context;
			_sleeperEnemy = sleeperEnemy;
		}

		public override void Enable()
		{
			if (base.HasStateAuthority)
			{
				_isEnabled = true;
				_context.OnEnemyTriggeredBySound += OnSoundHeard;
			}
		}

		public override void Disable()
		{
			if (base.HasStateAuthority)
			{
				_isEnabled = false;
				_context.OnEnemyTriggeredBySound -= OnSoundHeard;
			}
		}

		public override void Clear()
		{
		}

		private void OnSoundHeard(HeardSound heardSound)
		{
			_context.LastHeardSoundPosition = heardSound.Position;
			_sleeperEnemy.TriggerEvent(SleeperEvent.OnSoundHeard);
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
