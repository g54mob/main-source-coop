using System;
using System.Collections.Generic;
using UnityEngine;

namespace NomadDrive.Features.WorldGeneration.POISpawning
{
	public class WeightedRandomSelector<T>
	{
		private List<WeightedItem<T>> items = new List<WeightedItem<T>>();

		private float totalWeight;

		public int Count => items.Count;

		public float TotalWeight => totalWeight;

		public void AddItem(T item, float weight)
		{
			if (!(weight <= 0f))
			{
				items.Add(new WeightedItem<T>
				{
					Item = item,
					Weight = weight
				});
				totalWeight += weight;
			}
		}

		public T Select()
		{
			if (items.Count == 0)
			{
				throw new InvalidOperationException("[WeightedRandomSelector] No items added to selector!");
			}
			float randomValue = UnityEngine.Random.Range(0f, totalWeight);
			return SelectInternal(randomValue);
		}

		public T Select(System.Random random)
		{
			if (items.Count == 0)
			{
				throw new InvalidOperationException("[WeightedRandomSelector] No items added to selector!");
			}
			float randomValue = (float)(random.NextDouble() * (double)totalWeight);
			return SelectInternal(randomValue);
		}

		private T SelectInternal(float randomValue)
		{
			float num = 0f;
			foreach (WeightedItem<T> item in items)
			{
				num += item.Weight;
				if (randomValue <= num)
				{
					return item.Item;
				}
			}
			return items[items.Count - 1].Item;
		}

		public void Clear()
		{
			items.Clear();
			totalWeight = 0f;
		}
	}
}
