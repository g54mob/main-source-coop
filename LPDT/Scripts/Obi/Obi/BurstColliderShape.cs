using Unity.Mathematics;

namespace Obi
{
	public struct BurstColliderShape
	{
		public float4 center;

		public float4 size;

		public ColliderShape.ShapeType type;

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
	}
}
