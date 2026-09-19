using System;
using Unity.Mathematics;

namespace Obi
{
	public struct Poly6Kernel
	{
		public float norm;

		public bool norm2D;

		public Poly6Kernel(bool norm2D)
		{
			this.norm2D = norm2D;
			if (norm2D)
			{
				norm = 4f / MathF.PI;
			}
			else
			{
				norm = 1.5666814f;
			}
		}

		public float W(float r, float h)
		{
			float num = h * h;
			float num2 = num * num;
			float num3 = num2 * num2;
			float num4 = math.min(r, h);
			float num5 = num - num4 * num4;
			if (norm2D)
			{
				return norm / num3 * num5 * num5 * num5;
			}
			return norm / (num3 * h) * num5 * num5 * num5;
		}
	}
}
