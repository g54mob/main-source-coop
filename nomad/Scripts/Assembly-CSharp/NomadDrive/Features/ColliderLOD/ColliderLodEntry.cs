using System;
using UnityEngine;

namespace NomadDrive.Features.ColliderLOD
{
	public sealed class ColliderLodEntry
	{
		public readonly IColliderLodTarget Target;

		public readonly Transform Transform;

		public readonly Collider[] RigidColliders;

		public readonly Vector3 LocalBoundsCenter;

		public readonly float BoundsRadius;

		public float ActivationRadius;

		public float DeactivationRadius;

		public Vector3 LastBucketedPosition;

		public long GridCellKey;

		public bool LodActive = true;

		public bool PendingDisable;

		public bool Removed;

		public Vector3 WorldCenter
		{
			get
			{
				if (!(Transform != null))
				{
					return Vector3.zero;
				}
				return Transform.TransformPoint(LocalBoundsCenter);
			}
		}

		public bool IsAlive
		{
			get
			{
				if (Target is MonoBehaviour monoBehaviour)
				{
					return monoBehaviour != null;
				}
				return false;
			}
		}

		public ColliderLodEntry(IColliderLodTarget target)
		{
			Target = target;
			Transform = target.transform;
			RigidColliders = target.GetRigidColliders() ?? Array.Empty<Collider>();
			if (RigidColliders.Length != 0)
			{
				bool flag = false;
				Bounds bounds = default(Bounds);
				Collider[] rigidColliders = RigidColliders;
				foreach (Collider collider in rigidColliders)
				{
					if (!(collider == null))
					{
						if (!flag)
						{
							bounds = collider.bounds;
							flag = true;
						}
						else
						{
							bounds.Encapsulate(collider.bounds);
						}
					}
				}
				if (flag)
				{
					LocalBoundsCenter = Transform.InverseTransformPoint(bounds.center);
					BoundsRadius = bounds.extents.magnitude;
				}
				else
				{
					LocalBoundsCenter = Vector3.zero;
					BoundsRadius = 0f;
				}
			}
			else
			{
				LocalBoundsCenter = Vector3.zero;
				BoundsRadius = 0f;
			}
			LastBucketedPosition = ((Transform != null) ? Transform.position : Vector3.zero);
		}
	}
}
