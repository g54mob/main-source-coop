using System.Collections.Generic;
using Features.DamageableTrackModule.Scripts;
using UnityEngine;

namespace Features.PhysicsVolumeModule.Scripts
{
	public sealed class VirtualColliderRig : MonoBehaviour, IFallDamageSurface
	{
		private const int IGNORE_RAYCAST_LAYER = 2;

		private Rigidbody _body;

		private Transform _carrier;

		private readonly List<Collider> _mirrors = new List<Collider>();

		private int _contentMask;

		public static VirtualColliderRig Create(Transform carrier, IReadOnlyList<Collider> carrierColliders, IReadOnlyList<Collider> shapeSources = null)
		{
			GameObject gameObject = new GameObject(carrier.name + "_VirtualColliders");
			gameObject.layer = 2;
			gameObject.transform.SetPositionAndRotation(carrier.position, carrier.rotation);
			VirtualColliderRig virtualColliderRig = gameObject.AddComponent<VirtualColliderRig>();
			virtualColliderRig._carrier = carrier;
			virtualColliderRig._body = gameObject.AddComponent<Rigidbody>();
			virtualColliderRig._body.isKinematic = true;
			virtualColliderRig._body.useGravity = false;
			foreach (Collider item in (shapeSources != null && shapeSources.Count > 0) ? shapeSources : carrierColliders)
			{
				if (item == null || item.isTrigger)
				{
					continue;
				}
				Collider collider = virtualColliderRig.CloneCollider(item);
				if (collider == null)
				{
					continue;
				}
				virtualColliderRig._mirrors.Add(collider);
				foreach (Collider carrierCollider in carrierColliders)
				{
					if (carrierCollider != null && !carrierCollider.isTrigger)
					{
						Physics.IgnoreCollision(collider, carrierCollider, ignore: true);
					}
				}
			}
			virtualColliderRig.ApplyContentMask();
			return virtualColliderRig;
		}

		public void IncludeContentLayers(int layerMask)
		{
			int num = _contentMask | layerMask;
			if (num != _contentMask)
			{
				_contentMask = num;
				ApplyContentMask();
			}
		}

		public void SetContentIgnored(Collider[] contentColliders, bool ignored)
		{
			if (contentColliders == null)
			{
				return;
			}
			foreach (Collider mirror in _mirrors)
			{
				if (mirror == null)
				{
					continue;
				}
				foreach (Collider collider in contentColliders)
				{
					if (collider != null && !collider.isTrigger && collider.enabled && collider.gameObject.activeInHierarchy)
					{
						Physics.IgnoreCollision(mirror, collider, ignored);
					}
				}
			}
		}

		public void Follow(Vector3 stampOffset = default(Vector3))
		{
			if (!(_carrier == null) && !(_body == null))
			{
				_body.MovePosition(_carrier.position + stampOffset);
				_body.MoveRotation(_carrier.rotation);
			}
		}

		public void Dispose()
		{
			if (this != null)
			{
				Object.Destroy(base.gameObject);
			}
		}

		private void ApplyContentMask()
		{
			foreach (Collider mirror in _mirrors)
			{
				if (!(mirror == null))
				{
					mirror.includeLayers = _contentMask;
					mirror.excludeLayers = ~_contentMask;
				}
			}
		}

		private Collider CloneCollider(Collider source)
		{
			Transform transform = source.transform;
			GameObject gameObject = new GameObject(source.name);
			gameObject.layer = 2;
			Transform obj = gameObject.transform;
			obj.SetParent(base.transform, worldPositionStays: false);
			obj.localPosition = _carrier.InverseTransformPoint(transform.position);
			obj.localRotation = Quaternion.Inverse(_carrier.rotation) * transform.rotation;
			obj.localScale = transform.lossyScale;
			if (!(source is BoxCollider boxCollider))
			{
				if (!(source is SphereCollider sphereCollider))
				{
					if (!(source is CapsuleCollider capsuleCollider))
					{
						if (source is MeshCollider meshCollider)
						{
							MeshCollider meshCollider2 = gameObject.AddComponent<MeshCollider>();
							meshCollider2.sharedMesh = meshCollider.sharedMesh;
							meshCollider2.convex = true;
							meshCollider2.sharedMaterial = meshCollider.sharedMaterial;
							return meshCollider2;
						}
						Object.Destroy(gameObject);
						return null;
					}
					CapsuleCollider capsuleCollider2 = gameObject.AddComponent<CapsuleCollider>();
					capsuleCollider2.center = capsuleCollider.center;
					capsuleCollider2.radius = capsuleCollider.radius;
					capsuleCollider2.height = capsuleCollider.height;
					capsuleCollider2.direction = capsuleCollider.direction;
					capsuleCollider2.sharedMaterial = capsuleCollider.sharedMaterial;
					return capsuleCollider2;
				}
				SphereCollider sphereCollider2 = gameObject.AddComponent<SphereCollider>();
				sphereCollider2.center = sphereCollider.center;
				sphereCollider2.radius = sphereCollider.radius;
				sphereCollider2.sharedMaterial = sphereCollider.sharedMaterial;
				return sphereCollider2;
			}
			BoxCollider boxCollider2 = gameObject.AddComponent<BoxCollider>();
			boxCollider2.center = boxCollider.center;
			boxCollider2.size = boxCollider.size;
			boxCollider2.sharedMaterial = boxCollider.sharedMaterial;
			return boxCollider2;
		}
	}
}
