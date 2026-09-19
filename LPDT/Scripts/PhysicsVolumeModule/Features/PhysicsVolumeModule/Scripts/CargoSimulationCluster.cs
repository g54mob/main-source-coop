using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.PhysicsVolumeModule.Scripts
{
	public sealed class CargoSimulationCluster
	{
		private readonly Dictionary<Rigidbody, Rigidbody> _copies = new Dictionary<Rigidbody, Rigidbody>();

		private readonly List<Rigidbody> _departed = new List<Rigidbody>();

		private readonly HashSet<Rigidbody> _arrived = new HashSet<Rigidbody>();

		private readonly Transform _root;

		private readonly Rigidbody _basket;

		private readonly int _basketColliderCount;

		private readonly Quaternion _referenceRotation;

		private readonly Vector3 _carrierScale;

		private const float BASKET_MASS_KG = 1000f;

		private const float MIN_DRIVE_ANGLE_DEG = 0.01f;

		private static int _nextId = 1;

		private static readonly List<Collider> _floorScratch = new List<Collider>();

		private static readonly Collider[] _oneScratch = new Collider[1];

		private const float RIM_CLEARANCE_M = 0.1f;

		public int Id { get; } = _nextId++;

		public Vector3 Origin => _root.position;

		public int BodyCount => _copies.Count;

		public int BasketColliderCount => _basketColliderCount;

		public CargoSimulationCluster(Vector3 origin, Transform carrier, IReadOnlyList<Collider> basketColliders)
		{
			GameObject gameObject = new GameObject("CargoSimulationCluster");
			gameObject.transform.position = origin;
			gameObject.transform.rotation = Quaternion.identity;
			gameObject.transform.localScale = Vector3.one;
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			_root = gameObject.transform;
			_carrierScale = carrier.lossyScale;
			GameObject gameObject2 = new GameObject("Basket");
			gameObject2.transform.SetParent(_root, worldPositionStays: false);
			_basket = gameObject2.AddComponent<Rigidbody>();
			_basket.isKinematic = false;
			_basket.useGravity = false;
			_basket.constraints = RigidbodyConstraints.FreezePosition;
			_basket.interpolation = RigidbodyInterpolation.None;
			_basket.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
			_basket.mass = 1000f;
			_basketColliderCount = CopyColliders(basketColliders, carrier, _root, gameObject2.transform);
			_referenceRotation = carrier.rotation;
		}

		public void Add(Rigidbody source, Transform carrier, IReadOnlyList<Collider> colliders)
		{
			if (!(source == null) && !_copies.ContainsKey(source))
			{
				Transform transform = _basket.transform;
				GameObject gameObject = new GameObject($"Cargo_{source.GetInstanceID()}");
				gameObject.transform.SetParent(_root, worldPositionStays: false);
				gameObject.transform.position = transform.TransformPoint(MultiplyScale(carrier.InverseTransformPoint(source.position), carrier.lossyScale));
				gameObject.transform.rotation = transform.rotation * (Quaternion.Inverse(carrier.rotation) * source.rotation);
				CopyColliders(colliders, carrier, transform, gameObject.transform);
				Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
				rigidbody.mass = source.mass;
				rigidbody.linearDamping = source.linearDamping;
				rigidbody.angularDamping = source.angularDamping;
				rigidbody.useGravity = source.useGravity;
				rigidbody.interpolation = RigidbodyInterpolation.None;
				rigidbody.collisionDetectionMode = source.collisionDetectionMode;
				rigidbody.maxDepenetrationVelocity = source.maxDepenetrationVelocity;
				rigidbody.linearVelocity = transform.TransformDirection(carrier.InverseTransformDirection(source.linearVelocity));
				rigidbody.angularVelocity = transform.TransformDirection(carrier.InverseTransformDirection(source.angularVelocity));
				_copies[source] = rigidbody;
			}
		}

		public void Remove(Rigidbody source)
		{
			if (!(source == null) && _copies.TryGetValue(source, out var value))
			{
				if (value != null)
				{
					UnityEngine.Object.Destroy(value.gameObject);
				}
				_copies.Remove(source);
				_arrived.Remove(source);
			}
		}

		public bool Contains(Rigidbody source)
		{
			if (source != null)
			{
				return _copies.ContainsKey(source);
			}
			return false;
		}

		public bool TryGetBasketFloor(out float floorY, out string lowest, out int measured)
		{
			_floorScratch.Clear();
			_basket.GetComponentsInChildren(includeInactive: true, _floorScratch);
			if (!LowestPointIn(_root, _floorScratch, out floorY, out lowest, out measured))
			{
				return false;
			}
			floorY /= _carrierScale.y;
			return true;
		}

		public bool TryDescribeBasketShapes(ICollection<string> shapes)
		{
			_floorScratch.Clear();
			_basket.GetComponentsInChildren(includeInactive: true, _floorScratch);
			return DescribeShapesIn(_basket.transform, _floorScratch, shapes, _carrierScale);
		}

		public static bool DescribeShapesIn(Transform frame, IReadOnlyList<Collider> colliders, ICollection<string> shapes)
		{
			return DescribeShapesIn(frame, colliders, shapes, Vector3.one);
		}

		public static bool DescribeShapesIn(Transform frame, IReadOnlyList<Collider> colliders, ICollection<string> shapes, Vector3 unitScale)
		{
			bool result = false;
			for (int i = 0; i < colliders.Count; i++)
			{
				Collider collider = colliders[i];
				if (!(collider == null) && !collider.isTrigger)
				{
					_oneScratch[0] = collider;
					if (ExtentsIn(frame, _oneScratch, out var bounds))
					{
						Vector3 vector = DivideScale(bounds.center, unitScale);
						Vector3 vector2 = DivideScale(bounds.size, unitScale);
						shapes.Add(collider.GetType().Name + ":" + collider.name + " " + $"c=({vector.x:F3},{vector.y:F3},{vector.z:F3}) " + $"s=({vector2.x:F3},{vector2.y:F3},{vector2.z:F3})");
						result = true;
					}
				}
			}
			return result;
		}

		public static bool LowestPointIn(Transform frame, IReadOnlyList<Collider> colliders, out float floorY)
		{
			string lowest;
			int measured;
			return LowestPointIn(frame, colliders, out floorY, out lowest, out measured);
		}

		public static bool LowestPointIn(Transform frame, IReadOnlyList<Collider> colliders, out float floorY, out string lowest, out int measured)
		{
			floorY = 0f;
			lowest = null;
			measured = 0;
			bool flag = false;
			for (int i = 0; i < colliders.Count; i++)
			{
				Collider collider = colliders[i];
				if (collider == null || collider.isTrigger)
				{
					continue;
				}
				_oneScratch[0] = collider;
				if (ExtentsIn(frame, _oneScratch, out var bounds))
				{
					measured++;
					if (!flag || !(bounds.min.y >= floorY))
					{
						floorY = bounds.min.y;
						lowest = collider.GetType().Name + ":" + collider.name;
						flag = true;
					}
				}
			}
			return flag;
		}

		public static bool ExtentsIn(Transform frame, IReadOnlyList<Collider> colliders, out Bounds bounds)
		{
			bounds = default(Bounds);
			bool flag = false;
			for (int i = 0; i < colliders.Count; i++)
			{
				Collider collider = colliders[i];
				if (collider == null || collider.isTrigger || !TryGetLocalBounds(collider, out var bounds2))
				{
					continue;
				}
				Vector3 min = bounds2.min;
				Vector3 max = bounds2.max;
				for (int j = 0; j < 8; j++)
				{
					Vector3 position = new Vector3(((j & 1) == 0) ? min.x : max.x, ((j & 2) == 0) ? min.y : max.y, ((j & 4) == 0) ? min.z : max.z);
					Vector3 vector = frame.InverseTransformPoint(collider.transform.TransformPoint(position));
					if (!flag)
					{
						bounds = new Bounds(vector, Vector3.zero);
					}
					else
					{
						bounds.Encapsulate(vector);
					}
					flag = true;
				}
			}
			return flag;
		}

		public bool TryGetBasketExtents(out Vector3 size)
		{
			size = Vector3.zero;
			_floorScratch.Clear();
			_basket.GetComponentsInChildren(includeInactive: true, _floorScratch);
			if (!ExtentsIn(_root, _floorScratch, out var bounds))
			{
				return false;
			}
			size = DivideScale(bounds.size, _carrierScale);
			return true;
		}

		public bool TryCollectDeparted(ICollection<Rigidbody> departed)
		{
			departed.Clear();
			_floorScratch.Clear();
			_basket.GetComponentsInChildren(includeInactive: true, _floorScratch);
			Transform transform = _basket.transform;
			if (!ExtentsIn(transform, _floorScratch, out var bounds))
			{
				return false;
			}
			bounds.Expand(0.2f);
			foreach (KeyValuePair<Rigidbody, Rigidbody> copy in _copies)
			{
				if (!(copy.Key == null) && !(copy.Value == null))
				{
					if (IsInsideBasin(bounds, transform.InverseTransformPoint(copy.Value.position)))
					{
						_arrived.Add(copy.Key);
					}
					else if (_arrived.Contains(copy.Key))
					{
						departed.Add(copy.Key);
					}
				}
			}
			return departed.Count > 0;
		}

		private static bool IsInsideBasin(Bounds basin, Vector3 localPoint)
		{
			if (localPoint.x >= basin.min.x && localPoint.x <= basin.max.x && localPoint.z >= basin.min.z && localPoint.z <= basin.max.z)
			{
				return localPoint.y >= basin.min.y;
			}
			return false;
		}

		public bool TryGetFirstCargoInBasket(out Vector3 cargoLocal, out Vector3 basketCentre, out Vector3 basketSize)
		{
			cargoLocal = Vector3.zero;
			basketCentre = Vector3.zero;
			basketSize = Vector3.zero;
			_floorScratch.Clear();
			_basket.GetComponentsInChildren(includeInactive: true, _floorScratch);
			Transform transform = _basket.transform;
			if (!ExtentsIn(transform, _floorScratch, out var bounds))
			{
				return false;
			}
			basketCentre = DivideScale(bounds.center, _carrierScale);
			basketSize = DivideScale(bounds.size, _carrierScale);
			foreach (KeyValuePair<Rigidbody, Rigidbody> copy in _copies)
			{
				if (!(copy.Value == null))
				{
					cargoLocal = DivideScale(transform.InverseTransformPoint(copy.Value.position), _carrierScale);
					return true;
				}
			}
			return false;
		}

		public bool TryGetFirstCargoExtents(out Vector3 size)
		{
			size = Vector3.zero;
			foreach (KeyValuePair<Rigidbody, Rigidbody> copy in _copies)
			{
				if (!(copy.Value == null))
				{
					_floorScratch.Clear();
					copy.Value.GetComponentsInChildren(includeInactive: true, _floorScratch);
					if (ExtentsIn(_root, _floorScratch, out var bounds))
					{
						size = bounds.size;
						return true;
					}
				}
			}
			return false;
		}

		private static bool TryGetLocalBounds(Collider collider, out Bounds bounds)
		{
			bounds = default(Bounds);
			if (collider is BoxCollider boxCollider)
			{
				bounds = new Bounds(boxCollider.center, boxCollider.size);
				return true;
			}
			if (collider is SphereCollider sphereCollider)
			{
				bounds = new Bounds(sphereCollider.center, Vector3.one * (sphereCollider.radius * 2f));
				return true;
			}
			if (collider is CapsuleCollider capsuleCollider)
			{
				float num = capsuleCollider.radius * 2f;
				float num2 = Mathf.Max(capsuleCollider.height, num);
				Vector3 size = new Vector3(num, num, num);
				if (capsuleCollider.direction == 0)
				{
					size.x = num2;
				}
				else if (capsuleCollider.direction == 1)
				{
					size.y = num2;
				}
				else
				{
					size.z = num2;
				}
				bounds = new Bounds(capsuleCollider.center, size);
				return true;
			}
			if (collider is MeshCollider meshCollider && meshCollider.sharedMesh != null)
			{
				bounds = meshCollider.sharedMesh.bounds;
				return true;
			}
			return false;
		}

		public bool TryGetCarrierLocalPose(Rigidbody source, out Vector3 localPosition, out Quaternion localRotation)
		{
			localPosition = Vector3.zero;
			localRotation = Quaternion.identity;
			if (source == null || !_copies.TryGetValue(source, out var value) || value == null)
			{
				return false;
			}
			Transform transform = _basket.transform;
			localPosition = DivideScale(transform.InverseTransformPoint(value.transform.position), _carrierScale);
			localRotation = Quaternion.Inverse(transform.rotation) * value.transform.rotation;
			return true;
		}

		public void SetCarrierRotation(Quaternion carrierRotation)
		{
			if (_basket == null)
			{
				return;
			}
			Quaternion quaternion = _root.rotation * (Quaternion.Inverse(_referenceRotation) * carrierRotation);
			if (_basket.isKinematic)
			{
				_basket.MoveRotation(quaternion);
				return;
			}
			(quaternion * Quaternion.Inverse(_basket.rotation)).ToAngleAxis(out var angle, out var axis);
			if (angle > 180f)
			{
				angle -= 360f;
			}
			if (axis.sqrMagnitude < 1E-08f || float.IsNaN(axis.x) || float.IsInfinity(axis.x) || Mathf.Abs(angle) < 0.01f)
			{
				_basket.angularVelocity = Vector3.zero;
			}
			else
			{
				_basket.angularVelocity = axis.normalized * (angle * (MathF.PI / 180f) / Time.fixedDeltaTime);
			}
		}

		public void PruneMissing(ICollection<Rigidbody> stillPresent)
		{
			_departed.Clear();
			foreach (KeyValuePair<Rigidbody, Rigidbody> copy in _copies)
			{
				if (copy.Key == null || !stillPresent.Contains(copy.Key))
				{
					_departed.Add(copy.Key);
				}
			}
			for (int i = 0; i < _departed.Count; i++)
			{
				Remove(_departed[i]);
			}
		}

		public void Dispose()
		{
			_copies.Clear();
			if (_root != null)
			{
				UnityEngine.Object.Destroy(_root.gameObject);
			}
		}

		private static int CopyColliders(IReadOnlyList<Collider> source, Transform carrier, Transform root, Transform parent)
		{
			if (source == null)
			{
				return 0;
			}
			int num = 0;
			for (int i = 0; i < source.Count; i++)
			{
				Collider collider = source[i];
				if (!(collider == null) && !collider.isTrigger)
				{
					GameObject gameObject = new GameObject($"Shape_{num}_{collider.name}");
					gameObject.transform.SetParent(parent, worldPositionStays: false);
					gameObject.transform.position = root.TransformPoint(MultiplyScale(carrier.InverseTransformPoint(collider.transform.position), carrier.lossyScale));
					gameObject.transform.rotation = root.rotation * (Quaternion.Inverse(carrier.rotation) * collider.transform.rotation);
					gameObject.transform.localScale = collider.transform.lossyScale;
					if (CopyShape(collider, gameObject))
					{
						num++;
					}
					else
					{
						UnityEngine.Object.Destroy(gameObject);
					}
				}
			}
			return num;
		}

		private static Vector3 MultiplyScale(Vector3 value, Vector3 by)
		{
			return new Vector3(value.x * by.x, value.y * by.y, value.z * by.z);
		}

		private static Vector3 DivideScale(Vector3 value, Vector3 by)
		{
			return new Vector3(value.x / by.x, value.y / by.y, value.z / by.z);
		}

		private static bool CopyShape(Collider source, GameObject target)
		{
			if (source is BoxCollider boxCollider)
			{
				BoxCollider boxCollider2 = target.AddComponent<BoxCollider>();
				boxCollider2.center = boxCollider.center;
				boxCollider2.size = boxCollider.size;
				return true;
			}
			if (source is SphereCollider sphereCollider)
			{
				SphereCollider sphereCollider2 = target.AddComponent<SphereCollider>();
				sphereCollider2.center = sphereCollider.center;
				sphereCollider2.radius = sphereCollider.radius;
				return true;
			}
			if (source is CapsuleCollider capsuleCollider)
			{
				CapsuleCollider capsuleCollider2 = target.AddComponent<CapsuleCollider>();
				capsuleCollider2.center = capsuleCollider.center;
				capsuleCollider2.radius = capsuleCollider.radius;
				capsuleCollider2.height = capsuleCollider.height;
				capsuleCollider2.direction = capsuleCollider.direction;
				return true;
			}
			if (source is MeshCollider meshCollider && meshCollider.sharedMesh != null)
			{
				MeshCollider meshCollider2 = target.AddComponent<MeshCollider>();
				meshCollider2.sharedMesh = meshCollider.sharedMesh;
				meshCollider2.convex = true;
				return true;
			}
			return false;
		}
	}
}
