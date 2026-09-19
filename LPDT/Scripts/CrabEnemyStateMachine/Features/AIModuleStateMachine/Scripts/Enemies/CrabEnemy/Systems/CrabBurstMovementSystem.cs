using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.CrabEnemy.Systems
{
	[NetworkBehaviourWeaved(0)]
	public class CrabBurstMovementSystem : MonoSystem
	{
		[SerializeField]
		private float _pauseMinDuration = 0.6f;

		[SerializeField]
		private float _pauseMaxDuration = 1.4f;

		[SerializeField]
		private float _dashMinDuration = 0.4f;

		[SerializeField]
		private float _dashMaxDuration = 0.9f;

		private CrabEnemyContext _context;

		private bool _isEnabled;

		private bool _isPausing;

		private float _stateTimeRemaining;

		public override bool IsEnabled => _isEnabled;

		[Inject]
		private void InjectDependencies(CrabEnemyContext crabEnemyContext)
		{
			_context = crabEnemyContext;
		}

		public override void Enable()
		{
			_isEnabled = true;
			_isPausing = true;
			_stateTimeRemaining = Random.Range(_pauseMinDuration, _pauseMaxDuration);
			_context.SetMoveSpeed(0f);
		}

		public override void Disable()
		{
			_isEnabled = false;
			Clear();
		}

		public override void Clear()
		{
		}

		public override void FixedUpdateNetwork()
		{
			if (!base.Initialized || !_isEnabled || !base.HasStateAuthority)
			{
				return;
			}
			_stateTimeRemaining -= base.Runner.DeltaTime;
			if (!(_stateTimeRemaining > 0f))
			{
				_isPausing = !_isPausing;
				if (_isPausing)
				{
					_stateTimeRemaining = Random.Range(_pauseMinDuration, _pauseMaxDuration);
					_context.SetMoveSpeed(0f);
				}
				else
				{
					_stateTimeRemaining = Random.Range(_dashMinDuration, _dashMaxDuration);
					_context.SetMoveSpeed(_context.TargetMoveSpeed);
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
