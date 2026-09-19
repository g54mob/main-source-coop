using Features.HingeModule.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.AI;

namespace Features.AIModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class EnemyDoorOpenSystem : NetworkBehaviour
	{
		[SerializeField]
		private HingeDespawner _hingeDespawner;

		[SerializeField]
		private Transform _forcePoint;

		[SerializeField]
		private float _sampleVelocityFallback = 0.25f;

		[SerializeField]
		private float _openForce = 20f;

		[SerializeField]
		private float _frontSideMultiplier = 1f;

		[SerializeField]
		private float _backSideMultiplier = 1.2f;

		[SerializeField]
		private float _speedMultiplier = 0.2f;

		[SerializeField]
		private float _cooldown = 0.25f;

		private Rigidbody _rb;

		private float _nextAllowedTime;

		private bool _doorBroken;

		private void Start()
		{
			_rb = GetComponent<Rigidbody>();
		}

		private void OnEnable()
		{
			NetworkBehaviourUtils.InternalOnEnable(this);
			_hingeDespawner.OnBreakEvent += ChangeGrabbable;
		}

		private void OnDisable()
		{
			NetworkBehaviourUtils.InternalOnDisable(this);
			_hingeDespawner.OnBreakEvent -= ChangeGrabbable;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (base.Object == null || !base.Object.HasStateAuthority || _doorBroken || Time.time < _nextAllowedTime || !other.TryGetComponent<NavMeshAgent>(out var component))
			{
				return;
			}
			_nextAllowedTime = Time.time + _cooldown;
			Vector3 rhs = component.transform.position - base.transform.position;
			rhs.y = 0f;
			Vector3 forward = base.transform.forward;
			forward.y = 0f;
			if (!(rhs.sqrMagnitude < 0.0001f) && !(forward.sqrMagnitude < 0.0001f))
			{
				rhs.Normalize();
				forward.Normalize();
				bool num = Vector3.Dot(forward, rhs) > 0f;
				Vector3 vector = (num ? (-forward) : forward);
				float num2 = (num ? _frontSideMultiplier : _backSideMultiplier);
				float num3 = component.velocity.magnitude;
				if (num3 < 0.01f)
				{
					num3 = component.desiredVelocity.magnitude;
				}
				if (num3 < 0.01f)
				{
					num3 = _sampleVelocityFallback;
				}
				float num4 = 1f + num3 * _speedMultiplier;
				Vector3 force = vector * (_openForce * num2 * num4);
				_rb.AddForceAtPosition(force, _forcePoint.position, ForceMode.Impulse);
			}
		}

		private void ChangeGrabbable()
		{
			_doorBroken = true;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}
	}
}
