using System;
using System.Collections.Generic;

namespace Features.StoreModule.Scripts
{
	public class CardsOnTableModel
	{
		public List<StoreCardBehaviour> CardsOnTable { get; } = new List<StoreCardBehaviour>();

		public event Action<StoreCardBehaviour> OnCardRegistered;

		public event Action<StoreCardBehaviour> OnCardUnregistered;

		public void RegisterCard(StoreCardBehaviour storeCardBehaviour)
		{
			if (!CardsOnTable.Contains(storeCardBehaviour))
			{
				CardsOnTable.Add(storeCardBehaviour);
				this.OnCardRegistered?.Invoke(storeCardBehaviour);
			}
		}

		public void UnregisterCard(StoreCardBehaviour storeCardBehaviour)
		{
			if (CardsOnTable.Contains(storeCardBehaviour))
			{
				CardsOnTable.Remove(storeCardBehaviour);
				this.OnCardUnregistered?.Invoke(storeCardBehaviour);
			}
		}
	}
}
