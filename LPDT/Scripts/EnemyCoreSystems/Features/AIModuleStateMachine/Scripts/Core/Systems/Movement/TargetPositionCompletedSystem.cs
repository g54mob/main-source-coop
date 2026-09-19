using Features.AIModuleStateMachine.Scripts.Core.Contexts;
using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Core.Systems.Movement
{
	[NetworkBehaviourWeaved(0)]
	public class TargetPositionCompletedSystem : MonoSystem
	{
		private IMovementContext _context;

		private float _currentDistanceToTarget;

		private bool _isEnabled;

		public override bool IsEnabled => _isEnabled;

		[Inject]
		private void InjectDependencies(IMovementContext context)
		{
			_context = context;
		}

		public override void Enable()
		{
			_isEnabled = true;
		}

		public override void Disable()
		{
			_isEnabled = false;
			Clear();
		}

		public override void Clear()
		{
			_context.SetTargetPositionCompleted(isCompleted: false);
		}

		private void Update()
		{
			if (base.Initialized && _isEnabled && !_context.TargetPositionCompleted)
			{
				_currentDistanceToTarget = Vector3.Distance(_context.NavMeshAgent.transform.position, _context.TargetPosition);
				if (_currentDistanceToTarget <= _context.CompletePointMinDistance)
				{
					_context.SetTargetPositionCompleted(isCompleted: true);
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
