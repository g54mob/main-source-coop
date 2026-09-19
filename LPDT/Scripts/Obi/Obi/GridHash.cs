using System.Runtime.InteropServices;
using Unity.Mathematics;

namespace Obi
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct GridHash
	{
		public static readonly int3[] cellOffsets3D = new int3[13]
		{
			new int3(1, 0, 0),
			new int3(0, 1, 0),
			new int3(1, 1, 0),
			new int3(0, 0, 1),
			new int3(1, 0, 1),
			new int3(0, 1, 1),
			new int3(1, 1, 1),
			new int3(-1, 1, 0),
			new int3(-1, -1, 1),
			new int3(0, -1, 1),
			new int3(1, -1, 1),
			new int3(-1, 0, 1),
			new int3(-1, 1, 1)
		};

		public static readonly int3[] cellOffsets = new int3[7]
		{
			new int3(0, 0, 0),
			new int3(-1, 0, 0),
			new int3(0, -1, 0),
			new int3(0, 0, -1),
			new int3(1, 0, 0),
			new int3(0, 1, 0),
			new int3(0, 0, 1)
		};

		public static readonly int2[] cell2DOffsets = new int2[5]
		{
			new int2(0, 0),
			new int2(-1, 0),
			new int2(0, -1),
			new int2(1, 0),
			new int2(0, 1)
		};

		public static int3 Quantize(float3 v, float cellSize)
		{
			return new int3(math.floor(v / cellSize));
		}

		public static int2 Quantize(float2 v, float cellSize)
		{
			return new int2(math.floor(v / cellSize));
		}

		public static int Hash(in int4 cellIndex, int maxCells)
		{
			return math.abs((73856093 * cellIndex.x) ^ (19349663 * cellIndex.y) ^ (83492791 * cellIndex.z) ^ (10380569 * cellIndex.w)) % maxCells;
		}

		public static int Hash(in int3 cellIndex, int maxCells)
		{
			return (((73856093 * cellIndex.x) ^ (19349663 * cellIndex.y) ^ (83492791 * cellIndex.z)) & 0x7FFFFFFF) % maxCells;
		}
	}
}
