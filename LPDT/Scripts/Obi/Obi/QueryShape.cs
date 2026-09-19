using UnityEngine;

namespace Obi
{
	public struct QueryShape
	{
		public enum QueryType
		{
			Sphere = 0,
			Box = 1,
			Ray = 2
		}

		public Vector4 center;

		public Vector4 size;

		public QueryType type;

		public float contactOffset;

		public float maxDistance;

		public int filter;

		public QueryShape(QueryType type, Vector3 center, Vector3 size, float contactOffset, float distance, int filter)
		{
			this.type = type;
			this.center = center;
			this.size = size;
			this.contactOffset = contactOffset;
			maxDistance = distance;
			this.filter = filter;
		}
	}
}
