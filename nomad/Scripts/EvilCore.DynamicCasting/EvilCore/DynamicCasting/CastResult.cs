using UnityEngine;

namespace EvilCore.DynamicCasting
{
	public struct CastResult
	{
		public struct HitEnumerator
		{
			private readonly CastResult _result;

			private int _index;

			public RaycastHit Current => _result.Hits[_index];

			public HitEnumerator(CastResult result)
			{
				_result = result;
				_index = -1;
			}

			public bool MoveNext()
			{
				_index++;
				if (_result.Hits != null)
				{
					return _index < _result.HitCount;
				}
				return false;
			}

			public void Reset()
			{
				_index = -1;
			}
		}

		public struct ColliderEnumerator
		{
			private readonly CastResult _result;

			private int _index;

			public Collider Current => _result.Colliders[_index];

			public ColliderEnumerator(CastResult result)
			{
				_result = result;
				_index = -1;
			}

			public bool MoveNext()
			{
				_index++;
				if (_result.Colliders != null)
				{
					return _index < _result.HitCount;
				}
				return false;
			}

			public void Reset()
			{
				_index = -1;
			}
		}

		public struct MeshHitEnumerator
		{
			private readonly CastResult _result;

			private int _index;

			public MeshRayHit Current => _result.MeshHits[_index];

			public MeshHitEnumerator(CastResult result)
			{
				_result = result;
				_index = -1;
			}

			public bool MoveNext()
			{
				_index++;
				if (_result.MeshHits != null)
				{
					return _index < _result.HitCount;
				}
				return false;
			}

			public void Reset()
			{
				_index = -1;
			}
		}

		public bool DidHit;

		public int HitCount;

		public CastType Type;

		public RaycastHit[] Hits;

		public Collider[] Colliders;

		public MeshRayHit[] MeshHits;

		private bool IsMeshRay => Type == CastType.MeshRay;

		public RaycastHit FirstHit
		{
			get
			{
				if (HitCount <= 0 || Hits == null)
				{
					return default(RaycastHit);
				}
				return Hits[0];
			}
		}

		public MeshRayHit FirstMeshHit
		{
			get
			{
				if (HitCount <= 0 || MeshHits == null)
				{
					return default(MeshRayHit);
				}
				return MeshHits[0];
			}
		}

		public Collider FirstOverlapCollider
		{
			get
			{
				if (HitCount <= 0 || Colliders == null)
				{
					return null;
				}
				return Colliders[0];
			}
		}

		public Vector3 HitPoint
		{
			get
			{
				if (!IsMeshRay)
				{
					return FirstHit.point;
				}
				return FirstMeshHit.Point;
			}
		}

		public Vector3 HitNormal
		{
			get
			{
				if (!IsMeshRay)
				{
					return FirstHit.normal;
				}
				return FirstMeshHit.Normal;
			}
		}

		public float HitDistance
		{
			get
			{
				if (!IsMeshRay)
				{
					return FirstHit.distance;
				}
				return FirstMeshHit.Distance;
			}
		}

		public GameObject HitGameObject
		{
			get
			{
				if (!IsMeshRay)
				{
					if (!(HitCollider != null))
					{
						return null;
					}
					return HitCollider.gameObject;
				}
				return FirstMeshHit.HitGameObject;
			}
		}

		public Transform HitTransform
		{
			get
			{
				if (!IsMeshRay)
				{
					if (!(HitCollider != null))
					{
						return null;
					}
					return HitCollider.transform;
				}
				return FirstMeshHit.HitTransform;
			}
		}

		public Collider HitCollider => FirstHit.collider;

		public Vector2 HitUV => FirstMeshHit.TextureCoord;

		public int HitTriangleIndex => FirstMeshHit.TriangleIndex;

		public Vector3 HitBarycentricCoord => FirstMeshHit.BarycentricCoord;

		public MeshFilter HitMeshFilter => FirstMeshHit.MeshFilter;

		public MeshRenderer HitMeshRenderer => FirstMeshHit.Renderer;

		public bool TryGetComponent<T>(out T component) where T : Component
		{
			component = null;
			if (!DidHit)
			{
				return false;
			}
			if (IsMeshRay)
			{
				MeshRayHit firstMeshHit = FirstMeshHit;
				if (firstMeshHit.HitTransform != null)
				{
					return firstMeshHit.HitTransform.TryGetComponent<T>(out component);
				}
				return false;
			}
			if (HitCollider != null)
			{
				return HitCollider.TryGetComponent<T>(out component);
			}
			if (FirstOverlapCollider != null)
			{
				return FirstOverlapCollider.TryGetComponent<T>(out component);
			}
			return false;
		}

		public bool TryGetComponentInParent<T>(out T component) where T : Component
		{
			component = null;
			if (!DidHit)
			{
				return false;
			}
			Transform transform = HitTransform;
			if (transform == null && FirstOverlapCollider != null)
			{
				transform = FirstOverlapCollider.transform;
			}
			if (transform == null)
			{
				return false;
			}
			component = transform.GetComponentInParent<T>();
			return component != null;
		}

		public T GetComponent<T>() where T : Component
		{
			TryGetComponent<T>(out var component);
			return component;
		}

		public HitEnumerator GetEnumerator()
		{
			return new HitEnumerator(this);
		}

		public ColliderEnumerator GetColliderEnumerator()
		{
			return new ColliderEnumerator(this);
		}

		public MeshHitEnumerator GetMeshHitEnumerator()
		{
			return new MeshHitEnumerator(this);
		}
	}
}
