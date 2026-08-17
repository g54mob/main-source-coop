using UnityEngine;

namespace PaintCore
{
	public struct CwHit
	{
		public RaycastHit Raw;

		private Vector2 first;

		private bool firstSet;

		private Vector2 second;

		private bool secondSet;

		public Vector3 Position;

		public Vector3 Normal;

		public Transform Transform;

		public int TriangleIndex;

		public float Distance;

		public Collider Collider;

		public Vector2 First
		{
			get
			{
				if (!firstSet)
				{
					return Raw.textureCoord;
				}
				return first;
			}
			set
			{
				first = value;
				firstSet = true;
			}
		}

		public Vector2 Second
		{
			get
			{
				if (!secondSet)
				{
					return Raw.textureCoord2;
				}
				return second;
			}
			set
			{
				second = value;
				secondSet = true;
			}
		}

		public CwHit(RaycastHit hit)
		{
			Raw = hit;
			first = default(Vector2);
			firstSet = false;
			second = default(Vector2);
			secondSet = false;
			Position = hit.point;
			Normal = hit.normal;
			Transform = hit.transform;
			TriangleIndex = hit.triangleIndex;
			Distance = hit.distance;
			Collider = hit.collider;
		}
	}
}
