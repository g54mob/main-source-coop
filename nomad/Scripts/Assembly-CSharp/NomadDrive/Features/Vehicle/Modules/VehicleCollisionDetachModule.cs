using System.Collections;
using System.Collections.Generic;
using EvilCore.Networking.Parenting;
using Mirror;
using NomadDrive.Features.Attachables;
using NomadDrive.Features.Vehicle.Collision;
using NomadDrive.Features.Vehicle.Modules.Slots;
using NomadDrive.Features.Vehicle.Networking;
using UnityEngine;

namespace NomadDrive.Features.Vehicle.Modules
{
	public class VehicleCollisionDetachModule : VehicleModule
	{
		[Header("Detach Radius")]
		[SerializeField]
		private float baseDetachRadius = 1f;

		[SerializeField]
		private float radiusPerForceUnit = 0.0005f;

		[SerializeField]
		private float maxDetachRadius = 6f;

		[Header("Force Thresholds")]
		[SerializeField]
		private float forceDetachThreshold = 70000f;

		[SerializeField]
		private float brokenPartForceMultiplier = 0.7f;

		[Header("Condition Damage")]
		[SerializeField]
		private float minDamageForce = 70000f;

		[SerializeField]
		private float maxDamageForce = 125000f;

		[SerializeField]
		private float minConditionDamage = 10f;

		[SerializeField]
		private float maxConditionDamage = 80f;

		[Header("Detach Chance")]
		[SerializeField]
		[Range(0f, 1f)]
		private float detachChanceAtFullCondition = 0.2f;

		[SerializeField]
		[Range(0f, 1f)]
		private float detachChanceAtZeroCondition = 0.8f;

		[Header("Ejection")]
		[SerializeField]
		private float ejectionForceMultiplier = 1f;

		[SerializeField]
		private float ejectionUpwardBias = 2f;

		[Header("Settle")]
		[SerializeField]
		private float settleDelay = 3f;

		[Header("Ground Settle")]
		[Tooltip("Layers the detached part rests on. If left empty, falls back to Terrain + Default + Ground at runtime.")]
		[SerializeField]
		private LayerMask groundMask;

		[SerializeField]
		private float maxEjectionSpeed = 6f;

		[SerializeField]
		private float restVelocityThreshold = 0.15f;

		[SerializeField]
		private float restConfirmTime = 0.35f;

		[SerializeField]
		private float groundProbeHeight = 50f;

		[SerializeField]
		private float maxGroundDropDistance = 60f;

		[SerializeField]
		private float surfaceClearance = 0.02f;

		private LayerMask _resolvedGroundMask;

		private bool _groundMaskResolved;

		private static readonly Dictionary<VehicleSlotId, Vector3> SlotEjectionDirections = new Dictionary<VehicleSlotId, Vector3>
		{
			{
				VehicleSlotId.DoorLeft,
				new Vector3(-1f, 0.3f, 0f)
			},
			{
				VehicleSlotId.DoorRight,
				new Vector3(1f, 0.3f, 0f)
			},
			{
				VehicleSlotId.Hood,
				new Vector3(0f, 1f, 1f)
			},
			{
				VehicleSlotId.HeadlightLeft,
				new Vector3(-0.5f, 0.3f, 1f)
			},
			{
				VehicleSlotId.HeadlightRight,
				new Vector3(0.5f, 0.3f, 1f)
			},
			{
				VehicleSlotId.BrakelightLeft,
				new Vector3(-0.5f, 0.3f, -1f)
			},
			{
				VehicleSlotId.BrakelightRight,
				new Vector3(0.5f, 0.3f, -1f)
			},
			{
				VehicleSlotId.TireFrontLeft,
				new Vector3(-1f, 0.2f, 0.3f)
			},
			{
				VehicleSlotId.TireFrontRight,
				new Vector3(1f, 0.2f, 0.3f)
			},
			{
				VehicleSlotId.TireRearLeft,
				new Vector3(-1f, 0.2f, -0.3f)
			},
			{
				VehicleSlotId.TireRearRight,
				new Vector3(1f, 0.2f, -0.3f)
			}
		};

		[Header("Detachable Slots")]
		[SerializeField]
		private List<VehicleSlotId> detachableSlotIds = new List<VehicleSlotId>
		{
			VehicleSlotId.Hood,
			VehicleSlotId.DoorLeft,
			VehicleSlotId.DoorRight,
			VehicleSlotId.HeadlightLeft,
			VehicleSlotId.HeadlightRight,
			VehicleSlotId.BrakelightLeft,
			VehicleSlotId.BrakelightRight,
			VehicleSlotId.TireFrontLeft,
			VehicleSlotId.TireFrontRight,
			VehicleSlotId.TireRearLeft,
			VehicleSlotId.TireRearRight
		};

		private LayerMask GroundMask
		{
			get
			{
				if (_groundMaskResolved)
				{
					return _resolvedGroundMask;
				}
				_resolvedGroundMask = ((groundMask.value != 0) ? groundMask : ((LayerMask)LayerMask.GetMask("Terrain", "Default", "Ground")));
				_groundMaskResolved = true;
				return _resolvedGroundMask;
			}
		}

		protected override void SubscribeEvents()
		{
			base.EventBus.OnCollisionServer += OnCollisionServer;
		}

		protected override void UnsubscribeEvents()
		{
			base.EventBus.OnCollisionServer -= OnCollisionServer;
		}

		private void OnCollisionServer(VehicleCollisionData data)
		{
			if (!NetworkServer.active || data.Severity == CollisionSeverity.Light)
			{
				return;
			}
			float num = Mathf.Min(baseDetachRadius + data.Force * radiusPerForceUnit, maxDetachRadius);
			foreach (VehicleSlotId detachableSlotId in detachableSlotIds)
			{
				if (!base.VehicleManager.NetworkSync.IsSlotOccupied(detachableSlotId) || !base.VehicleManager.NetworkSync.TryGetSlot((byte)detachableSlotId, out var slot))
				{
					continue;
				}
				float num2 = Vector3.Distance(data.ImpactPoint, slot.transform.position);
				if (num2 > num)
				{
					continue;
				}
				float num3 = 1f - num2 / num;
				AttachableObject attachedObject = GetAttachedObject(slot);
				if (attachedObject == null)
				{
					continue;
				}
				attachedObject.TryGetComponent<ConditionComponent>(out var component);
				float num4 = ((component != null) ? component.Condition : 100f);
				float num5 = data.Force * num3;
				if (component != null && component.IsBroken)
				{
					num5 /= brokenPartForceMultiplier;
				}
				if (num5 < forceDetachThreshold)
				{
					continue;
				}
				float num6 = data.Force * num3;
				float num7 = 0f;
				if (num6 >= minDamageForce)
				{
					float t = Mathf.InverseLerp(minDamageForce, maxDamageForce, num6);
					num7 = Mathf.Lerp(minConditionDamage, maxConditionDamage, t);
				}
				float t2 = Mathf.Clamp01(num4 / 100f);
				float num8 = Mathf.Lerp(detachChanceAtZeroCondition, detachChanceAtFullCondition, t2);
				if (Random.value <= num8)
				{
					if (component != null && num7 > 0f)
					{
						component.ServerSetCondition(component.Condition - num7);
					}
					ForceDetachPart(slot, attachedObject, data);
				}
			}
		}

		private AttachableObject GetAttachedObject(VehicleSlot slot)
		{
			uint slotAttachment = base.VehicleManager.NetworkSync.GetSlotAttachment(slot.SlotId);
			if (slotAttachment == 0)
			{
				return null;
			}
			if (NetworkServer.spawned.TryGetValue(slotAttachment, out var value))
			{
				return value.GetComponent<AttachableObject>();
			}
			return null;
		}

		private void ForceDetachPart(VehicleSlot slot, AttachableObject part, VehicleCollisionData data)
		{
			part.NetworkedTransform.ServerSetParent(null, default(NetworkedTransformParentingConfig), 0);
			slot.ServerDetach();
			part.ServerDetach();
			part.SetRigidCollidersTriggered(newValue: false);
			part.SetRigidCollidersEnabled(newValue: true);
			part.SetVehicleColliderIsolated(newValue: false);
			part.SetInteractionAvailability(newValue: true);
			part.TryGetComponent<Rigidbody>(out var component);
			if (component != null)
			{
				component.isKinematic = false;
				component.useGravity = true;
				component.interpolation = RigidbodyInterpolation.Interpolate;
				component.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
				Vector3 value;
				Vector3 vector = ((!SlotEjectionDirections.TryGetValue(slot.SlotId, out value)) ? (part.transform.position - data.ImpactPoint).normalized : base.VehicleManager.transform.TransformDirection(value).normalized);
				vector += Vector3.up * ejectionUpwardBias;
				vector.Normalize();
				float num = Mathf.Min(ejectionForceMultiplier, maxEjectionSpeed);
				component.linearVelocity = vector * num;
				component.angularVelocity = Vector3.zero;
			}
			part.ServerOpenTransformStreamingWindow(settleDelay + 1f);
			StartCoroutine(SettleWhenAtRest(part, component));
		}

		private IEnumerator SettleWhenAtRest(AttachableObject part, Rigidbody rb)
		{
			float elapsed = 0f;
			float restTimer = 0f;
			float restVelSqr = restVelocityThreshold * restVelocityThreshold;
			WaitForFixedUpdate wait = new WaitForFixedUpdate();
			while (elapsed < settleDelay)
			{
				yield return wait;
				if (!NetworkServer.active || part == null || part.IsAttached)
				{
					yield break;
				}
				elapsed += Time.fixedDeltaTime;
				if (rb == null || rb.IsSleeping() || (rb.linearVelocity.sqrMagnitude <= restVelSqr && rb.angularVelocity.sqrMagnitude <= restVelSqr))
				{
					restTimer += Time.fixedDeltaTime;
					if (restTimer >= restConfirmTime)
					{
						break;
					}
				}
				else
				{
					restTimer = 0f;
				}
			}
			if (NetworkServer.active && !(part == null) && !part.IsAttached)
			{
				FinalizeSettle(part, rb);
			}
		}

		private void FinalizeSettle(AttachableObject part, Rigidbody rb)
		{
			Quaternion rotation = part.transform.rotation;
			if (TrySnapToGround(part, out var safePos))
			{
				part.transform.position = safePos;
				Physics.SyncTransforms();
			}
			Vector3 position = part.transform.position;
			if (rb != null)
			{
				rb.linearVelocity = Vector3.zero;
				rb.angularVelocity = Vector3.zero;
				rb.isKinematic = true;
			}
			part.SetInteractionAvailability(newValue: true);
			base.VehicleManager.NetworkSync.RpcSettleDetachedPart(part.netId, position, rotation);
			part.ServerSetTransformStreaming(active: false);
		}

		private bool TrySnapToGround(AttachableObject part, out Vector3 safePos)
		{
			Vector3 vector = (safePos = part.transform.position);
			float num = Mathf.Max(vector.y, base.VehicleManager.transform.position.y) + groundProbeHeight;
			if (!Physics.Raycast(new Vector3(vector.x, num, vector.z), maxDistance: num - vector.y + maxGroundDropDistance, direction: Vector3.down, hitInfo: out var hitInfo, layerMask: GroundMask, queryTriggerInteraction: QueryTriggerInteraction.Ignore))
			{
				return false;
			}
			float colliderBottomOffset = GetColliderBottomOffset(part);
			float num2 = hitInfo.point.y + colliderBottomOffset + surfaceClearance;
			if (vector.y >= num2)
			{
				return false;
			}
			safePos = new Vector3(vector.x, num2, vector.z);
			return true;
		}

		private static float GetColliderBottomOffset(AttachableObject part)
		{
			Collider[] componentsInChildren = part.GetComponentsInChildren<Collider>();
			bool flag = false;
			float num = 1f / 0f;
			Collider[] array = componentsInChildren;
			foreach (Collider collider in array)
			{
				if (!(collider == null) && !collider.isTrigger)
				{
					flag = true;
					num = Mathf.Min(num, collider.bounds.min.y);
				}
			}
			if (!flag)
			{
				return 0f;
			}
			return part.transform.position.y - num;
		}
	}
}
