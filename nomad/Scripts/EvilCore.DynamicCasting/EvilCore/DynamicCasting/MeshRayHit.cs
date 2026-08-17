using UnityEngine;

namespace EvilCore.DynamicCasting
{
	public struct MeshRayHit
	{
		public Vector3 Point;

		public Vector3 Normal;

		public float Distance;

		public int TriangleIndex;

		public Vector3 BarycentricCoord;

		public Vector2 TextureCoord;

		public Transform HitTransform;

		public MeshFilter MeshFilter;

		public MeshRenderer Renderer;

		public GameObject HitGameObject
		{
			get
			{
				if (!(HitTransform != null))
				{
					return null;
				}
				return HitTransform.gameObject;
			}
		}

		public Mesh Mesh
		{
			get
			{
				if (!(MeshFilter != null))
				{
					return null;
				}
				return MeshFilter.sharedMesh;
			}
		}

		public T GetComponent<T>() where T : class
		{
			if (!(HitTransform != null))
			{
				return null;
			}
			return HitTransform.GetComponent<T>();
		}

		public T GetComponentInParent<T>() where T : class
		{
			if (!(HitTransform != null))
			{
				return null;
			}
			return HitTransform.GetComponentInParent<T>();
		}

		public bool TryGetComponent<T>(out T component) where T : class
		{
			component = GetComponent<T>();
			return component != null;
		}

		public bool TryGetComponentInParent<T>(out T component) where T : class
		{
			component = GetComponentInParent<T>();
			return component != null;
		}
	}
}
