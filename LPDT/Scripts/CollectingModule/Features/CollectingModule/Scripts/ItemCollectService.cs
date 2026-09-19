using Features.CurrencyModule.Scripts;
using Features.ItemsModule.Scripts;
using Zenject;

namespace Features.CollectingModule.Scripts
{
	public class ItemCollectService : IItemCollectService
	{
		private readonly AddCurrencyNetworkEvent _addCurrencyNetworkEvent;

		[Inject]
		public ItemCollectService(AddCurrencyNetworkEvent addCurrencyNetworkEvent)
		{
			_addCurrencyNetworkEvent = addCurrencyNetworkEvent;
		}

		public void Collect(IItem item)
		{
			if (item != null && item.IsCollectable)
			{
				int currencyValue = item.CurrencyValue;
				if (currencyValue > 0)
				{
					_addCurrencyNetworkEvent.SendEvent(currencyValue);
				}
			}
		}
	}
}
