using UnityEngine;

namespace Obi
{
	public struct Edge : IBounded
	{
		public int i1;

		public int i2;

		private Aabb b;

		public Edge(int i1, int i2, Vector2 v1, Vector2 v2)
		{
			this.i1 = i1;
			this.i2 = i2;
			b = new Aabb(v1);
			b.Encapsulate(v2);
		}

		public Aabb GetBounds()
		{
			return b;
		}
	}
}
