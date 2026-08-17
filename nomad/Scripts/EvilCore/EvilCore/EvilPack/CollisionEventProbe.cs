using System;
using System.Collections.Generic;
using UnityEngine;

namespace EvilCore.EvilPack
{
	public sealed class CollisionEventProbe : MonoBehaviour
	{
		public static Action<CollisionEventRecord> EventDispatched;

		public HashSet<Collider> SourceFilter;

		private void OnCollisionEnter(Collision c)
		{
			Forward(c, CollisionEventKind.CollisionEnter);
		}

		private void OnCollisionStay(Collision c)
		{
			Forward(c, CollisionEventKind.CollisionStay);
		}

		private void OnCollisionExit(Collision c)
		{
			Forward(c, CollisionEventKind.CollisionExit);
		}

		private void OnTriggerEnter(Collider other)
		{
			ForwardTrigger(other, CollisionEventKind.TriggerEnter);
		}

		private void OnTriggerStay(Collider other)
		{
			ForwardTrigger(other, CollisionEventKind.TriggerStay);
		}

		private void OnTriggerExit(Collider other)
		{
			ForwardTrigger(other, CollisionEventKind.TriggerExit);
		}

		private void Forward(Collision c, CollisionEventKind kind)
		{
			if (EventDispatched != null && c != null && !(c.collider == null))
			{
				Collider preferredSelf = ((c.contactCount > 0) ? c.GetContact(0).thisCollider : null);
				Collider collider = ResolveSourceCollider(c.collider, preferredSelf);
				if (!(collider == null))
				{
					Vector3 contactPoint = ((c.contactCount > 0) ? c.GetContact(0).point : collider.bounds.center);
					Vector3 direction = ((c.contactCount > 0) ? c.GetContact(0).normal : Vector3.up);
					EventDispatched(new CollisionEventRecord
					{
						Kind = kind,
						Source = collider,
						Other = c.collider,
						ContactPoint = contactPoint,
						Direction = direction,
						Magnitude = c.impulse.magnitude,
						Frame = Time.frameCount,
						Time = Time.realtimeSinceStartup
					});
				}
			}
		}

		private void ForwardTrigger(Collider other, CollisionEventKind kind)
		{
			if (EventDispatched != null && !(other == null))
			{
				Collider collider = ResolveSourceCollider(other, null);
				if (!(collider == null))
				{
					EventDispatched(new CollisionEventRecord
					{
						Kind = kind,
						Source = collider,
						Other = other,
						ContactPoint = collider.bounds.ClosestPoint(other.bounds.center),
						Direction = Vector3.zero,
						Magnitude = 0f,
						Frame = Time.frameCount,
						Time = Time.realtimeSinceStartup
					});
				}
			}
		}

		private Collider ResolveSourceCollider(Collider other, Collider preferredSelf)
		{
			if (preferredSelf != null && SourceFilter != null && SourceFilter.Contains(preferredSelf))
			{
				return preferredSelf;
			}
			Collider[] components = GetComponents<Collider>();
			for (int i = 0; i < components.Length; i++)
			{
				if (SourceFilter != null && SourceFilter.Contains(components[i]))
				{
					return components[i];
				}
			}
			if (SourceFilter != null)
			{
				Collider[] componentsInChildren = GetComponentsInChildren<Collider>(includeInactive: true);
				for (int j = 0; j < componentsInChildren.Length; j++)
				{
					if (SourceFilter.Contains(componentsInChildren[j]))
					{
						return componentsInChildren[j];
					}
				}
			}
			if (components.Length == 0)
			{
				return other;
			}
			return components[0];
		}
	}
}
