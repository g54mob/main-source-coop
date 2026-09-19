using System;
using Features.CustomNetworkEventsModule.Scripts;
using UnityEngine;

namespace Features.CurrencyModule.Scripts
{
	[Serializable]
	public class AddCurrencyNetworkEvent : NetworkEventBase<AddCurrencyNetworkEvent>
	{
		[field: SerializeField]
		public int AdditionalCurrency { get; private set; }

		public void SendEvent(int additionalExperience)
		{
			AdditionalCurrency = additionalExperience;
			Send();
		}
	}
}
