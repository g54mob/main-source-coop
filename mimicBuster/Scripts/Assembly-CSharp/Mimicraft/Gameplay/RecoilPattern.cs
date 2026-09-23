using System;
using UnityEngine;

namespace Mimicraft.Gameplay
{
	public sealed class RecoilPattern
	{
		private struct Lcg
		{
			private uint state;

			public Lcg(int seed)
			{
				state = (uint)(seed * 747796405 + -1403630843);
			}

			public float Range(float min, float max)
			{
				state = state * 1664525 + 1013904223;
				float num = (float)(state >> 8) / 16777216f;
				return min + (max - min) * num;
			}
		}

		public const int TableSize = 64;

		private readonly Vector2[] entries;

		public Vector2 this[int shot]
		{
			get
			{
				int num = shot % 64;
				if (num < 0)
				{
					num += 64;
				}
				return entries[num];
			}
		}

		private RecoilPattern(Vector2[] entries)
		{
			this.entries = entries;
		}

		public static RecoilPattern Build(int seed, float angle, float angleVariance, float magnitude, float magnitudeVariance)
		{
			Vector2[] array = new Vector2[64];
			Lcg lcg = new Lcg(seed);
			for (int i = 0; i < 64; i++)
			{
				float num = angle + lcg.Range(0f - angleVariance, angleVariance);
				float num2 = Mathf.Max(0f, magnitude + lcg.Range(0f - magnitudeVariance, magnitudeVariance));
				float f = num * (MathF.PI / 180f);
				array[i] = new Vector2((0f - Mathf.Cos(f)) * num2, Mathf.Sin(f) * num2);
			}
			return new RecoilPattern(array);
		}
	}
}
