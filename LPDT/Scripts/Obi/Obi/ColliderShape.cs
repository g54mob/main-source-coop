using UnityEngine;

namespace Obi
{
	public struct ColliderShape
	{
		public enum ShapeType
		{
			Sphere = 0,
			Box = 1,
			Capsule = 2,
			Heightmap = 3,
			TriangleMesh = 4,
			EdgeMesh = 5,
			SignedDistanceField = 6
		}

		public Vector4 center;

		public Vector4 size;

		public ShapeType type;

		public float contactOffset;

		public int dataIndex;

		public int rigidbodyIndex;

		public int materialIndex;

		public int forceZoneIndex;

		public int filter;

		public int flags;

		public bool is2D
		{
			get
			{
				return (flags & 1) != 0;
			}
			set
			{
				flags |= (value ? 1 : 0);
			}
		}

		public bool isTrigger
		{
			get
			{
				if ((flags & 2) == 0)
				{
					return forceZoneIndex >= 0;
				}
				return true;
			}
			set
			{
				flags |= (value ? 2 : 0);
			}
		}

		public float sign => ((flags & 4) == 0) ? 1 : (-1);

		public void SetSign(bool inverted)
		{
			if (inverted)
			{
				flags |= 4;
			}
			else
			{
				flags &= -5;
			}
		}
	}
}
