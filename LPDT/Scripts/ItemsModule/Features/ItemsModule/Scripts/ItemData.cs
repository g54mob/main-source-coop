using System;
using UnityEngine;

namespace Features.ItemsModule.Scripts
{
	[Serializable]
	public class ItemData
	{
		public ItemType ItemType;

		public int CurrencyValue;

		public int MaxCurrencyValue;

		public bool IsCollectable;

		public ItemData(ItemType itemType, int currencyValue, int maxCurrencyValue, bool isCollectable = true)
		{
			ItemType = itemType;
			CurrencyValue = currencyValue;
			MaxCurrencyValue = maxCurrencyValue;
			IsCollectable = isCollectable;
		}

		public ItemData(ItemConfig config)
		{
			ItemType = config.ItemType;
			IsCollectable = config.IsCollectable;
			MaxCurrencyValue = CalculateNaxValue(config);
			CurrencyValue = MaxCurrencyValue;
		}

		private int CalculateNaxValue(ItemConfig config)
		{
			int currencyValue = config.CurrencyValue;
			bool flag = UnityEngine.Random.value > 0.5f;
			if (flag && config.MaxAdditionalCurrencyPercent > 0f)
			{
				float num = UnityEngine.Random.Range(0f, config.MaxAdditionalCurrencyPercent);
				int num2 = Mathf.RoundToInt((float)currencyValue * num);
				return Mathf.Max(1, currencyValue + num2);
			}
			if (!flag && config.MaxReduceCurrencyPercent > 0f)
			{
				float num3 = UnityEngine.Random.Range(0f, config.MaxReduceCurrencyPercent);
				int num4 = Mathf.RoundToInt((float)currencyValue * num3);
				return Mathf.Max(1, currencyValue - num4);
			}
			return Mathf.Max(1, currencyValue);
		}
	}
}
