using System;
using Unity.Mathematics;

namespace Obi
{
	public struct SpikyKernel
	{
		public float norm;

		public bool norm2D;

		public SpikyKernel(bool norm2D)
		{
			this.norm2D = norm2D;
			if (norm2D)
			{
				norm = -30f / MathF.PI;
			}
			else
			{
				norm = -45f / MathF.PI;
			}
		}

		public float W(float r, float h)
		{
			float num = h * h;
			float num2 = num * num;
			float num3 = math.min(r, h);
			float num4 = h - num3;
			if (norm2D)
			{
				return norm / (num2 * h) * num4 * num4;
			}
			return norm / (num2 * num) * num4 * num4;
		}
	}
}
