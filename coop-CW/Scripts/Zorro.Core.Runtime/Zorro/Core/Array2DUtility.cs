using Unity.Mathematics;

namespace Zorro.Core
{
	public static class Array2DUtility
	{
		public static int2 Get2DCoordFrom1D(int index, int width, int height)
		{
			int2 zero = int2.zero;
			zero.y = index / width;
			zero.x = index % width;
			return zero;
		}
	}
}
