using System.Collections.Generic;
using Features.CompositeItemModule.Scripts;
using Features.GrabModule.Scripts;
using Features.ItemsModule.Scripts;
using UnityEngine;
using UnityEngine.AI;

namespace Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy
{
	public class PorterCartIntake : MonoBehaviour
	{
		private const float HELD_INSIDE_GRACE_SECONDS = 2f;

		[SerializeField]
		private Transform _holdPoint;

		[SerializeField]
		private float _weightThreshold = 10f;

		[SerializeField]
		private float _maxItemSize = 1.6f;

		[SerializeField]
		private float _maxItemVolume = 0.3f;

		[SerializeField]
		private float _inCartRadius = 0.6f;

		private static readonly Vector3[] _boundsCornerSigns = new Vector3[8]
		{
			new Vector3(-1f, -1f, -1f),
			new Vector3(-1f, -1f, 1f),
			new Vector3(-1f, 1f, -1f),
			new Vector3(-1f, 1f, 1f),
			new Vector3(1f, -1f, -1f),
			new Vector3(1f, -1f, 1f),
			new Vector3(1f, 1f, -1f),
			new Vector3(1f, 1f, 1f)
		};

		private readonly HashSet<IPointGrabable> _inside = new HashSet<IPointGrabable>();

		private readonly HashSet<IPointGrabable> _heldWhileInside = new HashSet<IPointGrabable>();

		private readonly HashSet<IPointGrabable> _offered = new HashSet<IPointGrabable>();

		private readonly Dictionary<IPointGrabable, float> _recentlyHeldInside = new Dictionary<IPointGrabable, float>();

		private readonly List<IPointGrabable> _recentlyHeldPrune = new List<IPointGrabable>();

		private readonly Dictionary<IPointGrabable, bool> _loadableCache = new Dictionary<IPointGrabable, bool>();

		public Transform HoldPoint
		{
			get
			{
				if (!(_holdPoint != null))
				{
					return base.transform;
				}
				return _holdPoint;
			}
		}

		private void Update()
		{
			_inside.RemoveWhere((IPointGrabable grabable) => grabable == null || grabable.NetworkObject == null);
			PruneRecentlyHeld();
			foreach (IPointGrabable item in _inside)
			{
				if (item.GrabbedByPlayers.Count > 0)
				{
					_heldWhileInside.Add(item);
					_recentlyHeldInside[item] = Time.time;
					_offered.Remove(item);
				}
				else if (_heldWhileInside.Remove(item))
				{
					_offered.Add(item);
				}
				else if (!_offered.Contains(item) && _recentlyHeldInside.ContainsKey(item))
				{
					_offered.Add(item);
				}
			}
		}

		public IPointGrabable TryGetDroppedItem()
		{
			_offered.RemoveWhere((IPointGrabable grabable) => grabable == null || grabable.NetworkObject == null || !_inside.Contains(grabable));
			foreach (IPointGrabable item in _offered)
			{
				if (!item.InCart && !item.IsHeavyItem && !(item.Weight > _weightThreshold) && item.GrabbedByPlayers.Count <= 0 && item.GrabbedBySomethingCount <= 0 && HasQuotaItem(item) && IsLoadable(item))
				{
					return item;
				}
			}
			return null;
		}

		private static bool HasQuotaItem(IPointGrabable grabable)
		{
			if (grabable.NetworkObject == null)
			{
				return false;
			}
			MonoItem componentInChildren = grabable.NetworkObject.GetComponentInChildren<MonoItem>();
			if (componentInChildren != null)
			{
				return componentInChildren.IsCollectable;
			}
			return false;
		}

		private bool IsLoadable(IPointGrabable grabable)
		{
			if (_loadableCache.TryGetValue(grabable, out var value))
			{
				return value;
			}
			if (IsCompositePiece(grabable))
			{
				_loadableCache[grabable] = false;
				return false;
			}
			Vector3 itemSize = GetItemSize(grabable);
			float num = Mathf.Max(itemSize.x, itemSize.y, itemSize.z);
			float num2 = itemSize.x * itemSize.y * itemSize.z;
			value = grabable.NetworkObject.GetComponentInChildren<NavMeshObstacle>(includeInactive: true) == null && num <= _maxItemSize && num2 <= _maxItemVolume;
			_loadableCache[grabable] = value;
			return value;
		}

		private static bool IsCompositePiece(IPointGrabable grabable)
		{
			if (!(grabable.NetworkObject.GetComponentInChildren<CompositeItem>(includeInactive: true) != null) && !(grabable.NetworkObject.GetComponentInParent<CompositeItem>(includeInactive: true) != null))
			{
				return grabable.NetworkObject.GetComponentInParent<CompositeItemGroup>(includeInactive: true) != null;
			}
			return true;
		}

		private Vector3 GetItemSize(IPointGrabable grabable)
		{
			Transform transform = grabable.NetworkObject.transform;
			Matrix4x4 worldToLocalMatrix = transform.worldToLocalMatrix;
			bool flag = false;
			Bounds bounds = default(Bounds);
			Collider[] componentsInChildren = grabable.NetworkObject.GetComponentsInChildren<Collider>(includeInactive: true);
			foreach (Collider collider in componentsInChildren)
			{
				if (collider.isTrigger || !TryGetColliderLocalBounds(collider, out var bounds2))
				{
					continue;
				}
				Vector3[] boundsCornerSigns = _boundsCornerSigns;
				foreach (Vector3 b in boundsCornerSigns)
				{
					Vector3 position = bounds2.center + Vector3.Scale(bounds2.extents, b);
					Vector3 vector = worldToLocalMatrix.MultiplyPoint3x4(collider.transform.TransformPoint(position));
					if (!flag)
					{
						bounds = new Bounds(vector, Vector3.zero);
						flag = true;
					}
					else
					{
						bounds.Encapsulate(vector);
					}
				}
			}
			if (!flag)
			{
				return Vector3.zero;
			}
			Vector3 vector2 = Vector3.Scale(bounds.size, transform.lossyScale);
			return new Vector3(Mathf.Abs(vector2.x), Mathf.Abs(vector2.y), Mathf.Abs(vector2.z));
		}

		private static bool TryGetColliderLocalBounds(Collider itemCollider, out Bounds bounds)
		{
			if (!(itemCollider is BoxCollider boxCollider))
			{
				if (!(itemCollider is SphereCollider sphereCollider))
				{
					if (!(itemCollider is CapsuleCollider capsuleCollider))
					{
						if (itemCollider is MeshCollider meshCollider && meshCollider.sharedMesh != null)
						{
							bounds = meshCollider.sharedMesh.bounds;
							return true;
						}
						bounds = default(Bounds);
						return false;
					}
					bounds = new Bounds(capsuleCollider.center, GetCapsuleSize(capsuleCollider));
					return true;
				}
				bounds = new Bounds(sphereCollider.center, Vector3.one * (sphereCollider.radius * 2f));
				return true;
			}
			bounds = new Bounds(boxCollider.center, boxCollider.size);
			return true;
		}

		private static Vector3 GetCapsuleSize(CapsuleCollider capsuleCollider)
		{
			float num = capsuleCollider.radius * 2f;
			float num2 = Mathf.Max(capsuleCollider.height, num);
			Vector3 result = Vector3.one * num;
			if (capsuleCollider.direction == 0)
			{
				result.x = num2;
			}
			else if (capsuleCollider.direction == 1)
			{
				result.y = num2;
			}
			else
			{
				result.z = num2;
			}
			return result;
		}

		public IPointGrabable TryGetItemRestingInCart()
		{
			_inside.RemoveWhere((IPointGrabable grabable) => grabable == null || grabable.NetworkObject == null);
			foreach (IPointGrabable item in _inside)
			{
				if (!item.InCart && item.GrabbedByPlayers.Count <= 0 && item.GrabbedBySomethingCount <= 0 && IsRestingInCart(item) && HasQuotaItem(item) && IsLoadable(item))
				{
					return item;
				}
			}
			return null;
		}

		public bool IsRestingInCart(IPointGrabable grabable)
		{
			if (grabable == null || grabable.NetworkObject == null)
			{
				return false;
			}
			Vector3 vector = ((grabable.Rigidbody != null) ? grabable.Rigidbody.worldCenterOfMass : grabable.NetworkObject.transform.position) - HoldPoint.position;
			vector.y = 0f;
			return vector.sqrMagnitude <= _inCartRadius * _inCartRadius;
		}

		public void Forget(IPointGrabable grabable)
		{
			if (grabable != null)
			{
				_offered.Remove(grabable);
				_heldWhileInside.Remove(grabable);
				_recentlyHeldInside.Remove(grabable);
			}
		}

		private void PruneRecentlyHeld()
		{
			_recentlyHeldPrune.Clear();
			foreach (KeyValuePair<IPointGrabable, float> item in _recentlyHeldInside)
			{
				if (item.Key == null || item.Key.NetworkObject == null || Time.time - item.Value > 2f)
				{
					_recentlyHeldPrune.Add(item.Key);
				}
			}
			foreach (IPointGrabable item2 in _recentlyHeldPrune)
			{
				_recentlyHeldInside.Remove(item2);
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			IPointGrabable pointGrabable = FindGrabable(other);
			if (pointGrabable != null)
			{
				_inside.Add(pointGrabable);
			}
		}

		private void OnTriggerExit(Collider other)
		{
			IPointGrabable pointGrabable = FindGrabable(other);
			if (pointGrabable != null)
			{
				_inside.Remove(pointGrabable);
				_heldWhileInside.Remove(pointGrabable);
				_offered.Remove(pointGrabable);
				_loadableCache.Remove(pointGrabable);
			}
		}

		private IPointGrabable FindGrabable(Collider other)
		{
			IPointGrabable pointGrabable = other.gameObject.GetComponent<IPointGrabable>();
			if (pointGrabable == null)
			{
				pointGrabable = other.gameObject.GetComponentInParent<IPointGrabable>();
			}
			if (pointGrabable == null)
			{
				pointGrabable = other.gameObject.GetComponentInChildren<IPointGrabable>();
			}
			return pointGrabable;
		}
	}
}
