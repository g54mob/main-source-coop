using System;
using Features.AIModuleStateMachine.Scripts.Core.Contexts;
using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Core.Systems.Movement
{
	[NetworkBehaviourWeaved(0)]
	public class RotateTowardsDirectionSystem : MonoSystem
	{
		private IMovementContext _context;

		private IFacingSpeedLimitContext _facingSpeedLimitContext;

		[SerializeField]
		private GameObject _bodyObject;

		[SerializeField]
		private float _bodyRotationSpeed = 2f;

		[SerializeField]
		private float _minVelocityForNavMeshDirection = 0.1f;

		[SerializeField]
		private bool _useDesiredVelocity;

		[Header("Facing Speed Limit")]
		[SerializeField]
		private bool _updateFacingSpeedMultiplier = true;

		[SerializeField]
		private float _fullSpeedAngle = 20f;

		[SerializeField]
		private float _stopSpeedAngle = 90f;

		[SerializeField]
		private float _minFacingSpeedMultiplier;

		private float _currentBodyYRotation;

		private float _targetBodyYRotation;

		private float _currentRotationSpeed;

		private bool _hasRotationTarget;

		private bool _isEnabled;

		private Vector3 _previousPosition;

		private bool _hasPreviousPosition;

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
			_previousPosition = _context.NavMeshAgent.transform.position;
			_hasPreviousPosition = true;
			_currentBodyYRotation = _bodyObject.transform.eulerAngles.y;
		}

		public override void Disable()
		{
			_isEnabled = false;
			_hasPreviousPosition = false;
			_hasRotationTarget = false;
			Clear();
		}

		public override void FixedUpdateNetwork()
		{
			if (base.HasStateAuthority && base.Initialized && _isEnabled)
			{
				Vector3 position = _context.NavMeshAgent.transform.position;
				Vector3 vector = (_useDesiredVelocity ? _context.NavMeshAgent.desiredVelocity : _context.NavMeshAgent.velocity);
				float rotationSpeedMultiplayer = 1f;
				Vector3 direction2;
				if (ShouldUseFacingSpeedLimit() && TryGetNavigationDirection(position, out var direction))
				{
					direction2 = direction;
				}
				else if (vector.sqrMagnitude >= _minVelocityForNavMeshDirection * _minVelocityForNavMeshDirection)
				{
					direction2 = vector;
				}
				else if (_hasPreviousPosition)
				{
					direction2 = position - _previousPosition;
					rotationSpeedMultiplayer = 2f;
				}
				else
				{
					direction2 = Vector3.zero;
				}
				_previousPosition = position;
				_hasPreviousPosition = true;
				UpdateRotationTarget(direction2, rotationSpeedMultiplayer);
			}
		}

		private void Update()
		{
			if (base.Initialized && _isEnabled && base.HasStateAuthority && _hasRotationTarget)
			{
				_currentBodyYRotation = Mathf.LerpAngle(_currentBodyYRotation, _targetBodyYRotation, Time.deltaTime * _currentRotationSpeed);
				_bodyObject.transform.rotation = Quaternion.Euler(0f, _currentBodyYRotation, 0f);
			}
		}

		public override void Clear()
		{
			SetFacingSpeedMultiplier(1f, 0f);
		}

		public void ValidateBodyObjectAssigned()
		{
			if (_bodyObject == null)
			{
				throw new InvalidOperationException("RotateTowardsDirectionSystem._bodyObject is not assigned on '" + base.name + "'. Assign the visual body GameObject on the prefab (see BaseEnemy / MimicEnemy).");
			}
		}

		private void UpdateRotationTarget(Vector3 direction, float rotationSpeedMultiplayer)
		{
			if (direction == Vector3.zero)
			{
				SetFacingSpeedMultiplier(1f, 0f);
				return;
			}
			direction.Normalize();
			_targetBodyYRotation = Mathf.Atan2(direction.x, direction.z) * 57.29578f;
			_currentRotationSpeed = _bodyRotationSpeed * rotationSpeedMultiplayer;
			_hasRotationTarget = true;
			float num = Vector3.Angle(Flatten(_bodyObject.transform.forward), Flatten(direction));
			float b = Mathf.InverseLerp(Mathf.Max(_fullSpeedAngle + 0.01f, _stopSpeedAngle), _fullSpeedAngle, num);
			b = Mathf.Max(Mathf.Clamp01(_minFacingSpeedMultiplier), b);
			SetFacingSpeedMultiplier(b, num);
		}

		private void SetFacingSpeedMultiplier(float multiplier, float angle)
		{
			if (_facingSpeedLimitContext != null)
			{
				if (!_updateFacingSpeedMultiplier)
				{
					_facingSpeedLimitContext.SetFacingMoveSpeedMultiplier(1f, 0f);
				}
				else
				{
					_facingSpeedLimitContext.SetFacingMoveSpeedMultiplier(multiplier, angle);
				}
			}
		}

		private bool ShouldUseFacingSpeedLimit()
		{
			if (_updateFacingSpeedMultiplier)
			{
				return _facingSpeedLimitContext != null;
			}
			return false;
		}

		private bool TryGetNavigationDirection(Vector3 currentPos, out Vector3 direction)
		{
			direction = Flatten(_context.NavMeshAgent.desiredVelocity);
			if (direction.sqrMagnitude >= _minVelocityForNavMeshDirection * _minVelocityForNavMeshDirection)
			{
				return true;
			}
			if (!_context.NavMeshAgent.hasPath)
			{
				direction = Vector3.zero;
				return false;
			}
			direction = Flatten(_context.NavMeshAgent.steeringTarget - currentPos);
			return direction.sqrMagnitude >= _minVelocityForNavMeshDirection * _minVelocityForNavMeshDirection;
		}

		private static Vector3 Flatten(Vector3 value)
		{
			value.y = 0f;
			return value;
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
