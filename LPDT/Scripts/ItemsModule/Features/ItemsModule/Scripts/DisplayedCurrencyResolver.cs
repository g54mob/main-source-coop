using System.Collections.Generic;

namespace Features.ItemsModule.Scripts
{
	public static class DisplayedCurrencyResolver
	{
		public static int Resolve(MonoItem monoItem, HashSet<object> countedClusters)
		{
			if (!monoItem.TryGetComponent<IDisplayedCurrencyOverride>(out var component) || !component.TryGetDisplayedCurrency(out var currency))
			{
				return monoItem.CurrencyValue;
			}
			object displayedCurrencyGroupKey = component.DisplayedCurrencyGroupKey;
			if (displayedCurrencyGroupKey != null && !countedClusters.Add(displayedCurrencyGroupKey))
			{
				return 0;
			}
			return currency;
		}
	}
}
