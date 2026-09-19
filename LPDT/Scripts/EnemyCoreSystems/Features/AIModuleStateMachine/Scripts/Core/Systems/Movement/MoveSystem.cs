using Features.AIModuleStateMachine.Scripts.Core.Contexts;
using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Core.Systems.Movement
{
	[NetworkBehaviourWeaved(0)]
	public class MoveSystem : MonoSystem
	{
		private IMovementContext _context;

		private IFacingSpeedLimitContext _facingSpeedLimitContext;

		private bool _isEnabled;

		private Vector3 _lastPos;

		public override bool IsEnabled => _isEnabled;

		[Inject]
		private void InjectDependencies(IMovementContext context)
		{
			_context = context;
			_facingSpeedLimitContext = context as IFacingSpeedLimitContext;
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
			_lastPos = default(Vector3);
		}

		private void Update()
		{
			if (base.Initialized && _isEnabled)
			{
				_context.NavMeshAgent.speed = GetEffectiveMoveSpeed();
				if (!_context.TargetPositionCompleted && !(_lastPos == _context.TargetPosition))
				{
					_context.NavMeshAgent.SetDestination(_context.TargetPosition);
					_lastPos = _context.TargetPosition;
				}
			}
		}

		public override void FixedUpdateNetwork()
		{
			if (base.Initialized && _isEnabled && base.HasStateAuthority)
			{
				float smoothedVelocity = Mathf.Lerp(_context.SmoothedVelocity, _context.NavMeshAgent.velocity.magnitude, _context.SmoothedVelocityLerpSpeed * base.Runner.DeltaTime);
				_context.SetSmoothedVelocity(smoothedVelocity);
			}
		}

		private float GetEffectiveMoveSpeed()
		{
			if (_facingSpeedLimitContext == null)
			{
				return _context.MoveSpeed;
			}
			return _context.MoveSpeed * _facingSpeedLimitContext.FacingMoveSpeedMultiplier;
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
