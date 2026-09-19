using System;
using Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy.Settings;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy
{
	public class PorterCartSteering
	{
		private const float MIN_FORWARD_SQR = 0.0001f;

		private const float MIN_CART_DISTANCE = 0.05f;

		private readonly MonkeyPorterContext _context;

		private readonly MonkeyPorterSettings _settings;

		private readonly Transform _transform;

		private float _turnRate;

		private float _tangentialSpeed;

		private Vector3 _smoothedDirection;

		private bool _hasSmoothedDirection;

		private Vector3 _aimTarget;

		private bool _hasAimTarget;

		private float _headingError;

		public float TangentialSpeed => _tangentialSpeed;

		public bool IsAimed
		{
			get
			{
				if (_hasAimTarget)
				{
					return Mathf.Abs(_headingError) <= _settings.DeliverAimTolerance;
				}
				return false;
			}
		}

		public void SetAimTarget(Vector3 worldPosition)
		{
			_hasAimTarget = true;
			_aimTarget = worldPosition;
		}

		public void ClearAimTarget()
		{
			_hasAimTarget = false;
			_headingError = 0f;
		}

		public PorterCartSteering(MonkeyPorterContext context, MonkeyPorterSettings settings)
		{
			_context = context;
			_settings = settings;
			_transform = context.transform;
		}

		public void Tick(float deltaTime, float cartFollowDistance)
		{
			_tangentialSpeed = 0f;
			if (_context.IsAgentOnOffMeshLink)
			{
				Reset();
				return;
			}
			if (!TryResolveDesiredDirection(deltaTime, out var direction))
			{
				Reset();
				return;
			}
			Vector3 forward = _transform.forward;
			forward.y = 0f;
			if (forward.sqrMagnitude < 0.0001f)
			{
				Reset();
				return;
			}
			forward.Normalize();
			_headingError = Vector3.SignedAngle(forward, direction, Vector3.up);
			UpdateTurnRate(deltaTime, _headingError);
			if (!_hasAimTarget)
			{
				ApplyTravelSpeed(_headingError);
			}
			SteerAroundAxle(deltaTime, _headingError, cartFollowDistance);
		}

		private bool TryResolveDesiredDirection(float deltaTime, out Vector3 direction)
		{
			if (_hasAimTarget)
			{
				direction = _aimTarget - _transform.position;
				direction.y = 0f;
				if (direction.sqrMagnitude < 0.0001f)
				{
					return false;
				}
				direction.Normalize();
				return true;
			}
			if (!_context.TryGetSteeringDirection(_settings.LookAheadDistance, deltaTime, out direction))
			{
				return false;
			}
			direction = SmoothDirection(direction, deltaTime);
			return true;
		}

		public void Reset()
		{
			_turnRate = 0f;
			_tangentialSpeed = 0f;
			_hasSmoothedDirection = false;
			_context.ApplySpeedScale(1f);
		}

		private Vector3 SmoothDirection(Vector3 rawDirection, float deltaTime)
		{
			if (!_hasSmoothedDirection)
			{
				_hasSmoothedDirection = true;
				_smoothedDirection = rawDirection;
				return _smoothedDirection;
			}
			float t = 1f - Mathf.Exp((0f - _settings.SteeringSmoothing) * deltaTime);
			Vector3 vector = Vector3.Slerp(_smoothedDirection, rawDirection, t);
			if (vector.sqrMagnitude > 0.0001f)
			{
				_smoothedDirection = vector.normalized;
			}
			return _smoothedDirection;
		}

		private void UpdateTurnRate(float deltaTime, float headingError)
		{
			float target = Mathf.Clamp(headingError * _settings.RotationSpeed, 0f - _settings.MaxTurnSpeed, _settings.MaxTurnSpeed);
			_turnRate = Mathf.MoveTowards(_turnRate, target, _settings.TurnAcceleration * deltaTime);
		}

		private void ApplyTravelSpeed(float headingError)
		{
			float num = 0.5f * (1f + Mathf.Cos(headingError * (MathF.PI / 180f)));
			_context.ApplySpeedScale(Mathf.Max(_settings.MinTurnSpeedScale, num * num));
		}

		private void SteerAroundAxle(float deltaTime, float headingError, float cartFollowDistance)
		{
			float num = Mathf.Abs(headingError);
			float num2 = Mathf.Clamp(_turnRate * deltaTime, 0f - num, num);
			if (!Mathf.Approximately(num2, 0f))
			{
				Vector3 forward = _transform.forward;
				forward.y = 0f;
				forward.Normalize();
				Vector3 position = _transform.position;
				Quaternion quaternion = Quaternion.AngleAxis(num2, Vector3.up);
				_transform.rotation = quaternion * _transform.rotation;
				if (!(cartFollowDistance < 0.05f))
				{
					Vector3 vector = position + forward * cartFollowDistance;
					Vector3 offset = vector + quaternion * (position - vector) - position;
					_context.MoveAgentBy(offset);
					_tangentialSpeed = Mathf.Abs(_turnRate) * (MathF.PI / 180f) * cartFollowDistance;
				}
			}
		}
	}
}
