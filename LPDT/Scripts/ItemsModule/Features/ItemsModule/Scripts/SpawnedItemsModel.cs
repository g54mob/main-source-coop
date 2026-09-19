using System;
using System.Collections.Generic;
using Features.GameCycle.Scripts.SessionCleanup;

namespace Features.ItemsModule.Scripts
{
	public class SpawnedItemsModel : ISessionCleanup
	{
		private readonly HashSet<IItem> _items = new HashSet<IItem>();

		public IReadOnlyCollection<IItem> Items => _items;

		public event Action<IItem> OnItemRegistered;

		public event Action<IItem> OnItemUnregistered;

		public void Register(IItem item)
		{
			if (_items.Add(item))
			{
				this.OnItemRegistered?.Invoke(item);
			}
		}

		public void Unregister(IItem item)
		{
			if (_items.Remove(item))
			{
				this.OnItemUnregistered?.Invoke(item);
			}
		}

		public void Cleanup()
		{
			_items.Clear();
		}
	}
}
