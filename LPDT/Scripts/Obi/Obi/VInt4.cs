using System;
using Unity.Mathematics;

namespace Obi
{
	[Serializable]
	public struct VInt4
	{
		public int x;

		public int y;

		public int z;

		public int w;

		public VInt4(int x, int y, int z, int w)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.w = w;
		}

		public VInt4(int x)
		{
			this.x = x;
			y = x;
			z = x;
			w = x;
		}

		public static implicit operator VInt4(int4 i)
		{
			return new VInt4(i.x, i.y, i.z, i.w);
		}

		public static implicit operator int4(VInt4 i)
		{
			return new int4(i.x, i.y, i.z, i.w);
		}
	}
}
