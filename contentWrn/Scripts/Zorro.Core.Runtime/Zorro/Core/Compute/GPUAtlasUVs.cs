using System;
using Unity.Mathematics;

namespace Zorro.Core.Compute
{
	[Serializable]
	public struct GPUAtlasUVs
	{
		public const int Stride = 16;

		public float2 Offset;

		public float2 Size;

		public GPUAtlasUVs(float2 offset, float2 size)
		{
			Offset = offset;
			Size = size;
		}
	}
}
