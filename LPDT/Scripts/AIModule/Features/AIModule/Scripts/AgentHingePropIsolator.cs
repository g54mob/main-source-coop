using System.Collections.Generic;
using Features.HingeModule.Scripts;
using UnityEngine;

namespace Features.AIModule.Scripts
{
	public class AgentHingePropIsolator : MonoBehaviour
	{
		private const int OVERLAP_BUFFER_SIZE = 128;

		[SerializeField]
		private float _scanRadius = 2.5f;

		[SerializeField]
		private float _scanInterval = 0.2f;

		[SerializeField]
		private float _releaseDelay = 1f;

		[SerializeField]
		private float _hingeSideRadius = 0.75f;

		[SerializeField]
		private float _doorApproachDistance = 2.5f;

		private readonly Dictionary<Collider, float> _isolatedColliders = new Dictionary<Collider, float>();

		private readonly List<Collider> _releasedColliders = new List<Collider>();

		private readonly List<Collider> _agentColliders = new List<Collider>();

		private Collider[] _overlapBuffer;

		private float _nextScanTime;

		private void Awake()
		{
			_overlapBuffer = new Collider[128];
			CollectAgentColliders();
		}

		private void OnDisable()
		{
			RestoreAll();
		}

		private void Update()
		{
			if (_agentColliders.Count != 0 && !(Time.time < _nextScanTime))
			{
				_nextScanTime = Time.time + _scanInterval;
				IsolateNearbyHingeProps();
				ReleaseExpiredColliders();
			}
		}

		private void CollectAgentColliders()
		{
			Collider[] componentsInChildren = GetComponentsInChildren<Collider>(includeInactive: true);
			foreach (Collider collider in componentsInChildren)
			{
				if (!collider.isTrigger)
				{
					_agentColliders.Add(collider);
				}
			}
		}

		private void IsolateNearbyHingeProps()
		{
			int num = Physics.OverlapSphereNonAlloc(base.transform.position, _scanRadius, _overlapBuffer, -1, QueryTriggerInteraction.Ignore);
			for (int i = 0; i < num; i++)
			{
				Collider collider = _overlapBuffer[i];
				Rigidbody rigidbody = ((collider == null) ? null : collider.attachedRigidbody);
				if (rigidbody == null || !rigidbody.TryGetComponent<HingeDespawner>(out var _))
				{
					continue;
				}
				if (!rigidbody.TryGetComponent<HingeJoint>(out var component2))
				{
					Isolate(collider);
					continue;
				}
				Vector3 vector = rigidbody.transform.TransformPoint(component2.anchor);
				if (IsOpenableDoorInReach(rigidbody, vector))
				{
					Isolate(collider);
				}
				else if (collider.bounds.SqrDistance(vector) <= _hingeSideRadius * _hingeSideRadius)
				{
					Isolate(collider);
				}
			}
		}

		private bool IsOpenableDoorInReach(Rigidbody doorRigidbody, Vector3 hingePoint)
		{
			if (!doorRigidbody.TryGetComponent<EnemyDoorOpenSystem>(out var _))
			{
				return false;
			}
			Vector3 vector = base.transform.position - hingePoint;
			vector.y = 0f;
			return vector.sqrMagnitude <= _doorApproachDistance * _doorApproachDistance;
		}

		private void Isolate(Collider target)
		{
			SetIgnoreCollision(target, isIgnored: true);
			_isolatedColliders[target] = Time.time;
		}

		private void ReleaseExpiredColliders()
		{
			_releasedColliders.Clear();
			foreach (KeyValuePair<Collider, float> isolatedCollider in _isolatedColliders)
			{
				if (!(isolatedCollider.Key != null) || !(Time.time - isolatedCollider.Value < _releaseDelay))
				{
					_releasedColliders.Add(isolatedCollider.Key);
				}
			}
			foreach (Collider releasedCollider in _releasedColliders)
			{
				SetIgnoreCollision(releasedCollider, isIgnored: false);
				_isolatedColliders.Remove(releasedCollider);
			}
		}

		private void RestoreAll()
		{
			foreach (KeyValuePair<Collider, float> isolatedCollider in _isolatedColliders)
			{
				SetIgnoreCollision(isolatedCollider.Key, isIgnored: false);
			}
			_isolatedColliders.Clear();
		}

		private void SetIgnoreCollision(Collider target, bool isIgnored)
		{
			if (target == null || !target.enabled)
			{
				return;
			}
			foreach (Collider agentCollider in _agentColliders)
			{
				if (!(agentCollider == null) && agentCollider.enabled)
				{
					Physics.IgnoreCollision(agentCollider, target, isIgnored);
				}
			}
		}
	}
}
