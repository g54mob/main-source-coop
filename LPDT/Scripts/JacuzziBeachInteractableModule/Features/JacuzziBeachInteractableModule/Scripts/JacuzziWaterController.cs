using System;
using System.Collections.Generic;
using Features.GrabModule.Scripts;
using Features.GrabModule.Scripts.PhysGrab;
using Fusion;
using UnityEngine;

namespace Features.JacuzziBeachInteractableModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class JacuzziWaterController : NetworkBehaviour
	{
		private sealed class TrackedItemState
		{
			public int ColliderRefCount;

			public Vector3 HitPosition;

			public PhysGrabber Grabber;

			public Vector3 WanderTarget;

			public Vector3 WanderDirection;

			public float SwingPhase;
		}

		[SerializeField]
		private LayerMask _itemsLayerMask;

		[Tooltip("Spawned on the peer holding state authority over the item — GrabObject only pulls when the grabber's authority matches the item's.")]
		[SerializeField]
		private PhysGrabber _grabberPrefab;

		[Header("Wander")]
		[Tooltip("Radius of the circle around this transform the grabber picks its targets in, on the XZ plane.")]
		[SerializeField]
		private float _wanderRadius = 1f;

		[SerializeField]
		private float _wanderMoveSpeed = 1f;

		[Tooltip("Degrees per second the heading may turn. Lower values make wider arcs when a new target is picked.")]
		[SerializeField]
		private float _wanderTurnSpeed = 180f;

		[SerializeField]
		private float _wanderTargetReachedDistance = 0.05f;

		[Tooltip("Vertical swing around the wander depth, in meters up and down.")]
		[SerializeField]
		private float _swingAmplitude = 0.1f;

		[SerializeField]
		private float _swingSpeed = 2f;

		private readonly Dictionary<SimplePointGrabable, TrackedItemState> _statesByItem = new Dictionary<SimplePointGrabable, TrackedItemState>();

		private readonly List<SimplePointGrabable> _itemsToRemove = new List<SimplePointGrabable>();

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			foreach (KeyValuePair<SimplePointGrabable, TrackedItemState> item in _statesByItem)
			{
				DespawnGrabber(item.Key, item.Value);
			}
			_statesByItem.Clear();
			base.Despawned(runner, hasState);
		}

		private void FixedUpdate()
		{
			if (_statesByItem.Count == 0)
			{
				return;
			}
			_itemsToRemove.Clear();
			foreach (KeyValuePair<SimplePointGrabable, TrackedItemState> item in _statesByItem)
			{
				SimplePointGrabable key = item.Key;
				TrackedItemState value = item.Value;
				if (key == null || key.NetworkObject == null)
				{
					_itemsToRemove.Add(key);
				}
				else if (value.Grabber != null)
				{
					if (key.GrabbedByPlayersCount > 0 || !key.NetworkObject.HasStateAuthority)
					{
						DespawnGrabber(key, value);
					}
					else
					{
						TickWander(value);
					}
				}
				else if (key.NetworkObject.HasStateAuthority && !IsGrabbedBySomeone(key))
				{
					SpawnGrabber(key, value);
				}
			}
			for (int i = 0; i < _itemsToRemove.Count; i++)
			{
				_statesByItem.Remove(_itemsToRemove[i]);
			}
		}

		public void NotifyItemEntered(Collider other, Vector3 hitPosition)
		{
			if (TryResolveItem(other, out var item))
			{
				if (!_statesByItem.TryGetValue(item, out var value))
				{
					value = new TrackedItemState();
					_statesByItem[item] = value;
				}
				value.ColliderRefCount++;
				value.HitPosition = hitPosition;
			}
		}

		public void NotifyItemExited(Collider other)
		{
			if (TryResolveItem(other, out var item) && _statesByItem.TryGetValue(item, out var value))
			{
				value.ColliderRefCount--;
				if (value.ColliderRefCount <= 0)
				{
					DespawnGrabber(item, value);
					_statesByItem.Remove(item);
				}
			}
		}

		private void SpawnGrabber(SimplePointGrabable item, TrackedItemState state)
		{
			Transform nearestHandle = item.GetNearestHandle(state.HitPosition);
			PhysGrabber physGrabber = base.Runner.Spawn(_grabberPrefab, state.HitPosition, Quaternion.identity);
			physGrabber.physGrabPointPullerPosition = physGrabber.transform;
			physGrabber.physGrabPoints[item.GrabObject] = nearestHandle;
			physGrabber.IsProcessPhysGrabbing = true;
			item.GrabObject.Grabbers.Add(physGrabber);
			state.Grabber = physGrabber;
			state.WanderTarget = GetRandomWanderTarget(state.HitPosition.y);
			state.WanderDirection = GetFlatDirection(state.WanderTarget - physGrabber.transform.position);
			state.SwingPhase = UnityEngine.Random.Range(0f, MathF.PI * 2f);
		}

		private void TickWander(TrackedItemState state)
		{
			Transform transform = state.Grabber.transform;
			float fixedDeltaTime = Time.fixedDeltaTime;
			Vector3 flatDirection = GetFlatDirection(state.WanderTarget - transform.position);
			state.WanderDirection = Vector3.RotateTowards(state.WanderDirection, flatDirection, _wanderTurnSpeed * (MathF.PI / 180f) * fixedDeltaTime, 0f).normalized;
			state.SwingPhase += _swingSpeed * fixedDeltaTime;
			Vector3 vector = transform.position + state.WanderDirection * (_wanderMoveSpeed * fixedDeltaTime);
			vector.y = state.WanderTarget.y + Mathf.Sin(state.SwingPhase) * _swingAmplitude;
			transform.position = vector;
			Vector3 vector2 = state.WanderTarget - vector;
			vector2.y = 0f;
			if (!(vector2.magnitude > _wanderTargetReachedDistance))
			{
				state.WanderTarget = GetRandomWanderTarget(state.WanderTarget.y);
			}
		}

		private Vector3 GetFlatDirection(Vector3 direction)
		{
			direction.y = 0f;
			if (!(direction.sqrMagnitude > Mathf.Epsilon))
			{
				return Vector3.forward;
			}
			return direction.normalized;
		}

		private Vector3 GetRandomWanderTarget(float height)
		{
			Vector2 vector = UnityEngine.Random.insideUnitCircle * _wanderRadius;
			Vector3 position = base.transform.position;
			return new Vector3(position.x + vector.x, height, position.z + vector.y);
		}

		private void DespawnGrabber(SimplePointGrabable item, TrackedItemState state)
		{
			if (!(state.Grabber == null))
			{
				if (item != null && item.GrabObject != null)
				{
					item.GrabObject.Grabbers.Remove(state.Grabber);
					state.Grabber.physGrabPoints.Remove(item.GrabObject);
				}
				if (state.Grabber.Object != null && state.Grabber.Object.IsValid)
				{
					base.Runner.Despawn(state.Grabber.Object);
				}
				state.Grabber = null;
			}
		}

		private bool TryResolveItem(Collider other, out SimplePointGrabable item)
		{
			item = null;
			if ((_itemsLayerMask.value & (1 << other.gameObject.layer)) == 0)
			{
				return false;
			}
			Rigidbody attachedRigidbody = other.attachedRigidbody;
			if (attachedRigidbody == null)
			{
				return false;
			}
			return attachedRigidbody.TryGetComponent<SimplePointGrabable>(out item);
		}

		private bool IsGrabbedBySomeone(SimplePointGrabable item)
		{
			if (item.GrabbedByPlayersCount <= 0 && item.GrabbedByExternalsCount <= 0)
			{
				return item.GrabbedBySomethingCount > 0;
			}
			return true;
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
