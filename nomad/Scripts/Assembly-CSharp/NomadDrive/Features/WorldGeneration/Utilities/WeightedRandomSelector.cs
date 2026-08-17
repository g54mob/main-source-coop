using System;

namespace NomadDrive.Features.WorldGeneration.Utilities
{
	public class WeightedRandomSelector<T>
	{
		private readonly T[] _items;

		private readonly Func<T, float> _weightGetter;

		private float _totalWeight;

		public float TotalWeight => _totalWeight;

		public int ItemCount => _items.Length;

		public WeightedRandomSelector(T[] items, Func<T, float> weightGetter)
		{
			if (items == null || items.Length == 0)
			{
				throw new ArgumentException("Items array cannot be null or empty", "items");
			}
			if (weightGetter == null)
			{
				throw new ArgumentNullException("weightGetter");
			}
			_items = items;
			_weightGetter = weightGetter;
			_totalWeight = 0f;
			T[] items2 = _items;
			foreach (T arg in items2)
			{
				_totalWeight += _weightGetter(arg);
			}
			if (_totalWeight <= 0f)
			{
				throw new InvalidOperationException("Total weight must be greater than 0");
			}
		}

		public T Select(Random random)
		{
			if (random == null)
			{
				throw new ArgumentNullException("random");
			}
			float num = (float)(random.NextDouble() * (double)_totalWeight);
			float num2 = 0f;
			for (int i = 0; i < _items.Length; i++)
			{
				num2 += _weightGetter(_items[i]);
				if (num < num2)
				{
					return _items[i];
				}
			}
			T[] items = _items;
			return items[items.Length - 1];
		}

		public T SelectNormalized(Random random)
		{
			if (random == null)
			{
				throw new ArgumentNullException("random");
			}
			float num = (float)(random.NextDouble() * 100.0);
			float num2 = 0f;
			T[] items = _items;
			foreach (T val in items)
			{
				num2 += _weightGetter(val);
				if (num < num2)
				{
					return val;
				}
			}
			return _items[_items.Length - 1];
		}

		public float GetNormalizedWeight(T item)
		{
			return _weightGetter(item) / _totalWeight * 100f;
		}
	}
}
