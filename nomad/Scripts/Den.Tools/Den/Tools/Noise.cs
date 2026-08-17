using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Den.Tools
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class Noise
	{
		[SerializeField]
		private int seed;

		[SerializeField]
		private int subSeed;

		public readonly int permutationCount;

		private readonly int permutationCountMinusOne;

		private readonly uint[] permutation;

		private static readonly int[] gradX = new int[8] { 1, -1, 1, -1, 1, -1, 0, 0 };

		private static readonly int[] gradZ = new int[8] { 0, 0, 1, 1, -1, -1, 1, -1 };

		public int Seed
		{
			get
			{
				return seed;
			}
			set
			{
				FillPermutation(value);
				seed = value;
			}
		}

		public int SubSeed
		{
			get
			{
				return subSeed;
			}
			set
			{
				subSeed = value & permutationCountMinusOne;
			}
		}

		public Noise(int seed, int permutationCount = 16384)
		{
			permutation = new uint[permutationCount * 2];
			this.permutationCount = permutationCount;
			permutationCountMinusOne = permutationCount - 1;
			this.seed = seed;
			FillPermutation(seed);
		}

		public Noise(Noise noise)
		{
			permutation = noise.permutation;
			permutationCount = noise.permutationCount;
			permutationCountMinusOne = noise.permutationCountMinusOne;
			seed = noise.seed;
			subSeed = noise.subSeed;
		}

		public Noise(Noise noise, int subSeed)
		{
			permutation = noise.permutation;
			permutationCount = noise.permutationCount;
			permutationCountMinusOne = noise.permutationCountMinusOne;
			seed = noise.seed;
			this.subSeed = subSeed & permutationCountMinusOne;
		}

		private void FillPermutation(int seed)
		{
			int num = permutation.Length / 2;
			for (int i = 0; i < num; i++)
			{
				seed = 214013 * seed + 2531011;
				float num2 = (float)((seed >> 16) & 0x7FFF) / 32768f;
				permutation[i] = (uint)(num2 * (float)num);
			}
			for (int j = 0; j < num; j++)
			{
				permutation[j + num] = permutation[j];
			}
		}

		public static int Pair(int k1, int k2)
		{
			return (k1 + k2) * (k1 + k2 + 1) / 2 + k2;
		}

		public float Random(int x)
		{
			x &= permutationCountMinusOne;
			x = (int)permutation[x + permutation[subSeed]];
			return 1f * (float)x / (float)permutationCount;
		}

		public float Random(int x, int z)
		{
			x &= permutationCountMinusOne;
			z &= permutationCountMinusOne;
			x = (int)permutation[x + permutation[z + permutation[subSeed]]];
			return 1f * (float)x / (float)permutationCount;
		}

		public float Random(int x, int y, int z)
		{
			x &= permutationCountMinusOne;
			y &= permutationCountMinusOne;
			z &= permutationCountMinusOne;
			x = (int)permutation[x + permutation[z + permutation[y + permutation[subSeed]]]];
			return 1f * (float)x / (float)permutationCount;
		}

		public float Random(int x, int y, int z, int w)
		{
			x &= permutationCountMinusOne;
			y &= permutationCountMinusOne;
			z &= permutationCountMinusOne;
			w &= permutationCountMinusOne;
			x = (int)permutation[x + permutation[z + permutation[y + permutation[w + permutation[subSeed]]]]];
			return 1f * (float)x / (float)permutationCount;
		}

		public int RandomInt(int x)
		{
			x &= permutationCountMinusOne;
			x = (int)permutation[x + permutation[subSeed]];
			return x;
		}

		public float Linear(float x, float z)
		{
			int num = ((x > 0f) ? ((int)x) : ((int)x - 1));
			int num2 = ((z > 0f) ? ((int)z) : ((int)z - 1));
			float num3 = x - (float)num;
			float num4 = z - (float)num2;
			num &= permutationCountMinusOne;
			num2 &= permutationCountMinusOne;
			uint num5 = permutation[subSeed];
			uint num6 = permutation[permutation[num5 + num] + num2];
			uint num7 = permutation[permutation[num5 + num] + num2 + 1];
			uint num8 = permutation[permutation[num5 + num + 1] + num2];
			uint num9 = permutation[permutation[num5 + num + 1] + num2 + 1];
			float num10 = 3f * num3 * num3 - 2f * num3 * num3 * num3;
			float num11 = 3f * num4 * num4 - 2f * num4 * num4 * num4;
			float num12 = (float)num6 * (1f - num10) + (float)num8 * num10;
			float num13 = (float)num7 * (1f - num10) + (float)num9 * num10;
			float num14 = num12 * (1f - num11) + num13 * num11;
			return 1f * num14 / (float)permutationCount;
		}

		public float Perlin(float x, float z)
		{
			int num = ((x > 0f) ? ((int)x) : ((int)x - 1));
			int num2 = ((z > 0f) ? ((int)z) : ((int)z - 1));
			float num3 = x - (float)num;
			float num4 = z - (float)num2;
			num &= permutationCountMinusOne;
			num2 &= permutationCountMinusOne;
			int num5 = (int)permutation[subSeed];
			int num6 = (int)(permutation[permutation[num5 + num] + num2] & 7);
			int num7 = (int)(permutation[permutation[num5 + num] + num2 + 1] & 7);
			int num8 = (int)(permutation[permutation[num5 + num + 1] + num2] & 7);
			int num9 = (int)(permutation[permutation[num5 + num + 1] + num2 + 1] & 7);
			float num10 = (float)gradX[num6] * num3 + (float)gradZ[num6] * num4;
			float num11 = (float)gradX[num7] * num3 + (float)gradZ[num7] * (num4 - 1f);
			float num12 = (float)gradX[num8] * (num3 - 1f) + (float)gradZ[num8] * num4;
			float num13 = (float)gradX[num9] * (num3 - 1f) + (float)gradZ[num9] * (num4 - 1f);
			float num14 = num3 * num3 * num3 * (num3 * (num3 * 6f - 15f) + 10f);
			float num15 = num4 * num4 * num4 * (num4 * (num4 * 6f - 15f) + 10f);
			float num16 = num10 * (1f - num14) + num12 * num14;
			float num17 = num11 * (1f - num14) + num13 * num14;
			return (num16 * (1f - num15) + num17 * num15 + 1f) / 2f;
		}

		public float Simplex(float x, float z)
		{
			float num = 0.3660254f;
			float num2 = 0.21132487f;
			float num3 = (x + z) * num;
			int num4 = ((x + num3 > 0f) ? ((int)(x + num3)) : ((int)(x + num3) - 1));
			int num5 = ((z + num3 > 0f) ? ((int)(z + num3)) : ((int)(z + num3) - 1));
			float num6 = (float)(num4 + num5) * num2;
			float num7 = (float)num4 - num6;
			float num8 = (float)num5 - num6;
			float num9 = x - num7;
			float num10 = z - num8;
			int num11 = ((num9 > num10) ? 1 : 0);
			int num12 = ((!(num9 > num10)) ? 1 : 0);
			float num13 = num9 - (float)num11 + num2;
			float num14 = num10 - (float)num12 + num2;
			float num15 = num9 - 1f + 2f * num2;
			float num16 = num10 - 1f + 2f * num2;
			int num17 = num4 & permutationCountMinusOne;
			int num18 = num5 & permutationCountMinusOne;
			uint num19 = permutation[subSeed];
			uint num20 = permutation[num17 + permutation[num18 + num19]] % 8;
			uint num21 = permutation[num17 + num11 + permutation[num18 + num12 + num19]] % 8;
			uint num22 = permutation[num17 + 1 + permutation[num18 + 1 + num19]] % 8;
			float num23 = 0.5f - num9 * num9 - num10 * num10;
			float num24 = ((!(num23 < 0f)) ? (num23 * num23 * num23 * num23 * ((float)gradX[num20] * num9 + (float)gradZ[num20] * num10)) : 0f);
			float num25 = 0.5f - num13 * num13 - num14 * num14;
			float num26 = ((!(num25 < 0f)) ? (num25 * num25 * num25 * num25 * ((float)gradX[num21] * num13 + (float)gradZ[num21] * num14)) : 0f);
			float num27 = 0.5f - num15 * num15 - num16 * num16;
			float num28 = ((!(num27 < 0f)) ? (num27 * num27 * num27 * num27 * ((float)gradX[num22] * num15 + (float)gradZ[num22] * num16)) : 0f);
			return (float)(70.0 * (double)(num24 + num26 + num28) + 1.0) / 2f;
		}

		[DllImport("NativePlugins", EntryPoint = "NoiseFractal")]
		public static extern float NativeFractal(uint[] p, int pc, float x, float y, float size, int iterations, float detail, float turbulence, int type, int subSeed);

		public float Fractal(float x, float y, float size, int iterations = -1, float detail = 0.5f, float turbulence = 0f, int type = 2, bool native = true)
		{
			float num = 0f;
			float num2 = size;
			float num3 = 1f;
			float num4 = ((turbulence > 0f) ? turbulence : (0f - turbulence));
			if (iterations < 0)
			{
				iterations = (int)Mathf.Log(size, 2f) + 1;
			}
			int num5 = (int)permutation[subSeed];
			num2 = size;
			for (int i = 0; i < iterations; i++)
			{
				float num6 = 0f;
				num6 = type switch
				{
					-1 => Mathf.PerlinNoise(x / num2 + (float)(permutation[num5 + i] + permutation[num5 + 1 + i]) / 1000f, y / num2 + (float)(permutation[num5 + 2 + i] + permutation[num5 + 3 + i]) / 1000f), 
					0 => Mathf.PerlinNoise(x / num2 + (float)(permutation[num5 + i] + permutation[num5 + 1 + i]), y / num2 + (float)(permutation[num5 + 2 + i] + permutation[num5 + 3 + 1])), 
					1 => Linear(x / num2, y / num2), 
					2 => Perlin(x / num2, y / num2), 
					3 => Simplex(x / num2, y / num2), 
					_ => 0f, 
				};
				if (num4 > 0.001f)
				{
					float num7 = num6 * 2f - 1f;
					if (num7 < 0f)
					{
						num7 = 0f - num7;
					}
					if (turbulence > 0f)
					{
						num7 = 1f - num7;
					}
					num6 = num6 * (1f - num4) + num7 * num4;
				}
				num += num6 * num3;
				num2 /= 2f;
				num3 *= detail;
			}
			return num * (1f - detail);
		}

		[Obsolete]
		public float OverlayFractal(int x, int y, float persistence = 2f, int iterations = 10, float turbulence = 0f, int type = 0)
		{
			float num = 0.5f;
			float num2 = Mathf.Pow(2f, iterations);
			float num3 = 1f;
			for (int num4 = iterations - 1; num4 >= 0; num4--)
			{
				float num5 = 0f;
				num5 = (type switch
				{
					0 => Mathf.PerlinNoise((float)x / num2, (float)y / num2), 
					1 => Linear((float)x / num2, (float)y / num2), 
					2 => Perlin((float)x / num2, (float)y / num2), 
					3 => Simplex((float)x / num2, (float)y / num2), 
					_ => 0f, 
				} - 0.5f) * num3 + 0.5f;
				num = ((!(num > 0.5f)) ? (2f * num5 * num) : (1f - 2f * (1f - num) * (1f - num5)));
				num2 /= 2f;
				num3 /= persistence;
			}
			return num;
		}
	}
}
