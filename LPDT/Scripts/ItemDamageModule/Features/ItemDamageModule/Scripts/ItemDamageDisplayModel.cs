using System;
using System.Collections.Generic;
using Features.ItemsModule.Scripts;

namespace Features.ItemDamageModule.Scripts
{
	public class ItemDamageDisplayModel
	{
		private int _currentCurrency;

		public List<MonoItem> CurrentDisplayedItem { get; private set; } = new List<MonoItem>();

		public int CurrentCurrency
		{
			get
			{
				return _currentCurrency;
			}
			set
			{
				int currentCurrency = _currentCurrency;
				_currentCurrency = value;
				if (_currentCurrency != currentCurrency)
				{
					this.OnCurrentCurrencyChanged?.Invoke(_currentCurrency);
				}
			}
		}

		public bool IsWithChildCurrency { get; set; }

		public event Action OnDisplayedItemChanged;

		public event Action<int> OnCurrentCurrencyChanged;

		public void OverrideList(List<MonoItem> item)
		{
			CurrentDisplayedItem = item;
			this.OnDisplayedItemChanged?.Invoke();
		}

		public void ClearItems()
		{
			CurrentDisplayedItem.Clear();
			this.OnDisplayedItemChanged?.Invoke();
		}
	}
}
