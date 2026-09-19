using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Features.AIModuleStateMachine.Scripts.Enemies.SleeperEnemy.Settings;
using Fusion;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.SleeperEnemy.Systems
{
	[NetworkBehaviourWeaved(0)]
	public class SleeperChaseGiveUpSystem : MonoSystem
	{
		private SleeperEnemy _sleeperEnemy;

		private SleeperEnemySettings _settings;

		private bool _isEnabled;

		private float _chaseTimer;

		public override bool IsEnabled => _isEnabled;

		[Inject]
		public void InjectDependencies(SleeperEnemy sleeperEnemy, SleeperEnemySettings settings)
		{
			_sleeperEnemy = sleeperEnemy;
			_settings = settings;
		}

		public override void Enable()
		{
			_isEnabled = true;
			_chaseTimer = 0f;
		}

		public override void Disable()
		{
			_isEnabled = false;
			Clear();
		}

		public override void Clear()
		{
			_chaseTimer = 0f;
		}

		public override void FixedUpdateNetwork()
		{
			if (base.Initialized && _isEnabled && base.HasStateAuthority)
			{
				_chaseTimer += base.Runner.DeltaTime;
				if (_chaseTimer >= _settings.ChaseGiveUpTime)
				{
					_sleeperEnemy.TriggerEvent(SleeperEvent.OnChaseTimeout);
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
