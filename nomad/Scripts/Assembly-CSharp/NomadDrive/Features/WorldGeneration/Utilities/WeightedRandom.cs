using System;

namespace NomadDrive.Features.WorldGeneration.Utilities
{
	public static class WeightedRandom
	{
		public static T Select<T>(T[] items, Func<T, float> weightGetter, Random random)
		{
			if (items == null || items.Length == 0)
			{
				throw new ArgumentException("Items array cannot be null or empty", "items");
			}
			if (weightGetter == null)
			{
				throw new ArgumentNullException("weightGetter");
			}
			if (random == null)
			{
				throw new ArgumentNullException("random");
			}
			float num = 0f;
			for (int i = 0; i < items.Length; i++)
			{
				num += weightGetter(items[i]);
			}
			if (num <= 0f)
			{
				throw new InvalidOperationException("Total weight must be greater than 0");
			}
			float num2 = (float)(random.NextDouble() * (double)num);
			float num3 = 0f;
			for (int j = 0; j < items.Length; j++)
			{
				num3 += weightGetter(items[j]);
				if (num2 < num3)
				{
					return items[j];
				}
			}
			return items[items.Length - 1];
		}

		public static T SelectNormalized<T>(T[] items, Func<T, float> weightGetter, Random random)
		{
			if (items == null || items.Length == 0)
			{
				throw new ArgumentException("Items array cannot be null or empty", "items");
			}
			if (weightGetter == null)
			{
				throw new ArgumentNullException("weightGetter");
			}
			if (random == null)
			{
				throw new ArgumentNullException("random");
			}
			float num = (float)(random.NextDouble() * 100.0);
			float num2 = 0f;
			for (int i = 0; i < items.Length; i++)
			{
				num2 += weightGetter(items[i]);
				if (num < num2)
				{
					return items[i];
				}
			}
			return items[items.Length - 1];
		}
	}
}
