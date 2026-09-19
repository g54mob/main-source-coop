using System;
using Features.ItemsModule.Scripts;

namespace Features.CollectingModule.Scripts
{
	public class CollectItemFellEvent
	{
		public event Action<IItem> OnItemFell;

		public void Invoke(IItem item)
		{
			this.OnItemFell?.Invoke(item);
		}
	}
}
