using System;
using UnityEngine;

namespace NomadDrive.Features.WorldGeneration
{
	public class SeedManager
	{
		public int Seed { get; set; }

		public void GenerateRandomSeed()
		{
			Seed = UnityEngine.Random.Range(0, 10000000);
		}

		public void SetSeed(int seed)
		{
			Seed = seed;
		}

		public int GetSubSeed(string identifier)
		{
			if (string.IsNullOrEmpty(identifier))
			{
				return Seed;
			}
			int hashCode = identifier.GetHashCode();
			return Seed ^ hashCode;
		}

		public int GetPositionBasedSeed(Vector3 position, float precision = 0.1f)
		{
			int num = Mathf.RoundToInt(position.x / precision);
			int num2 = Mathf.RoundToInt(position.y / precision);
			int num3 = Mathf.RoundToInt(position.z / precision);
			int num4 = num.GetHashCode() ^ (num2.GetHashCode() << 2) ^ (num3.GetHashCode() >> 2);
			return Seed ^ num4;
		}

		public int GetStableKey(Vector2Int chunk, int indexInChunk)
		{
			return (((((Seed * 397) ^ chunk.x) * 397) ^ chunk.y) * 397) ^ indexInChunk;
		}

		public static int CombineSeed(int a, int b)
		{
			return (a * 397) ^ b;
		}

		public int GetLocationSeed(Vector3 position, string identifier, float precision = 0.1f)
		{
			int positionBasedSeed = GetPositionBasedSeed(position, precision);
			int num = identifier?.GetHashCode() ?? 0;
			return Seed ^ positionBasedSeed ^ num;
		}

		public System.Random CreateRandom(string identifier)
		{
			return new System.Random(GetSubSeed(identifier));
		}

		public System.Random CreateRandomFromPosition(Vector3 position, float precision = 0.1f)
		{
			return new System.Random(GetPositionBasedSeed(position, precision));
		}

		public static int GetPositionHash(Vector3 position, float precision = 0.1f)
		{
			int num = Mathf.RoundToInt(position.x / precision);
			int num2 = Mathf.RoundToInt(position.y / precision);
			int num3 = Mathf.RoundToInt(position.z / precision);
			return num.GetHashCode() ^ (num2.GetHashCode() << 2) ^ (num3.GetHashCode() >> 2);
		}
	}
}
