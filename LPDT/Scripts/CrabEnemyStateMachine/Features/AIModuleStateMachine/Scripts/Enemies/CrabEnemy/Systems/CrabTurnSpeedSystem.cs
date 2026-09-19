using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.CrabEnemy.Systems
{
	[NetworkBehaviourWeaved(0)]
	public class CrabTurnSpeedSystem : MonoSystem
	{
		[SerializeField]
		private GameObject _bodyObject;

		[SerializeField]
		private float _fullSpeedAngle = 15f;

		[SerializeField]
		private float _maxSlowAngle = 90f;

		[SerializeField]
		private float _minSpeedFactor = 0.4f;

		[SerializeField]
		private float _minDesiredSpeed = 0.1f;

		[SerializeField]
		private float _minVelocityForHeading = 0.1f;

		private CrabEnemyContext _context;

		private bool _isEnabled;

		public override bool IsEnabled => _isEnabled;

		[Inject]
		private void InjectDependencies(CrabEnemyContext crabEnemyContext)
		{
			_context = crabEnemyContext;
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
		}

		public override void FixedUpdateNetwork()
		{
			if (!base.Initialized || !_isEnabled || !base.HasStateAuthority || _context.MoveSpeed <= 0f)
			{
				return;
			}
			Vector3 to = Flatten(_context.NavMeshAgent.desiredVelocity);
			if (to.sqrMagnitude < _minDesiredSpeed * _minDesiredSpeed)
			{
				_context.SetMoveSpeed(_context.TargetMoveSpeed);
				return;
			}
			Vector3 vector = Flatten(_context.NavMeshAgent.velocity);
			if (vector.sqrMagnitude < _minVelocityForHeading * _minVelocityForHeading)
			{
				if (_bodyObject == null)
				{
					return;
				}
				vector = Flatten(_bodyObject.transform.forward);
				if (vector.sqrMagnitude <= 0f)
				{
					return;
				}
			}
			float value = Vector3.Angle(vector, to);
			float t = Mathf.InverseLerp(_fullSpeedAngle, _maxSlowAngle, value);
			float num = Mathf.Lerp(1f, _minSpeedFactor, t);
			_context.SetMoveSpeed(_context.TargetMoveSpeed * num);
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
