using System;
using Features.MultiplayerSessionServices.Scripts;
using Zenject;

namespace Features.CurrencyModule.Scripts
{
	public class AddCurrencySystem : IInitializable, IDisposable
	{
		private readonly AddCurrencyNetworkEvent _addCurrencyNetworkEvent;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly CurrencyModel _currencyModel;

		public AddCurrencySystem(AddCurrencyNetworkEvent addCurrencyNetworkEvent, MultiplayerModel multiplayerModel, CurrencyModel currencyModel)
		{
			_addCurrencyNetworkEvent = addCurrencyNetworkEvent;
			_multiplayerModel = multiplayerModel;
			_currencyModel = currencyModel;
		}

		public void Initialize()
		{
			_addCurrencyNetworkEvent.OnNetworkEventSend += AddCurrency;
		}

		public void Dispose()
		{
			_addCurrencyNetworkEvent.OnNetworkEventSend -= AddCurrency;
		}

		private void AddCurrency(AddCurrencyNetworkEvent networkEvent)
		{
			if (_multiplayerModel.NetworkRunner.IsSharedModeMasterClient)
			{
				int currency = _currencyModel.CurrentCurrency + networkEvent.AdditionalCurrency;
				_currencyModel.SetCurrency(currency);
				_currencyModel.Synchronize();
			}
		}
	}
}
