using System;
using System.Collections.Generic;

namespace Features.QuotaModule.Scripts
{
	public class QuotaContainerModel
	{
		private List<QuotaContainerItemData> _allContainerItems = new List<QuotaContainerItemData>();

		private List<QuotaContainerItemData> _freeContainerItems = new List<QuotaContainerItemData>();

		public List<QuotaContainerItemData> AllContainerItems => _allContainerItems;

		public List<QuotaContainerItemData> FreeContainerItems => _freeContainerItems;

		public event Action OnContainerItemsUpdated;

		public event Action OnFreeContainerItemsUpdated;

		public void UpdateContainerItems(List<QuotaContainerItemData> containerItems)
		{
			_allContainerItems = containerItems;
			this.OnContainerItemsUpdated?.Invoke();
		}

		public void UpdateFreeContainerItems(List<QuotaContainerItemData> containerItems)
		{
			_freeContainerItems = containerItems;
			this.OnFreeContainerItemsUpdated?.Invoke();
		}
	}
}
