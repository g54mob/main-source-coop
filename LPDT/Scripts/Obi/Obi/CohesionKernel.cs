using System;
using System.Runtime.InteropServices;
using Unity.Mathematics;

namespace Obi
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct CohesionKernel
	{
		public float W(float r, float h)
		{
			return math.cos(math.min(r, h) * 3f * MathF.PI / (2f * h));
		}
	}
}
