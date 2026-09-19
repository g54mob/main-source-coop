using System.Collections.Generic;
using Features.ItemsModule.Scripts;
using UnityEngine;
using Zenject;

namespace Features.QuotaModule.Scripts.PhysicsContainer
{
	public class QuotaPhysicsContainer : MonoBehaviour
	{
		private readonly List<QuotaContainerItemData> _itemsData = new List<QuotaContainerItemData>();

		private readonly Dictionary<MonoItem, QuotaContainerItemData> _items = new Dictionary<MonoItem, QuotaContainerItemData>();

		private QuotaContainerModel _quotaContainerModel;

		[Inject]
		private void InjectDependencies(QuotaContainerModel quotaContainerModel)
		{
			_quotaContainerModel = quotaContainerModel;
		}

		public void AddQuotaItem(QuotaContainerItemData itemData)
		{
			if (!_items.ContainsKey(itemData.Item))
			{
				_itemsData.Add(itemData);
				_items.Add(itemData.Item, itemData);
				_quotaContainerModel.UpdateContainerItems(_itemsData);
			}
		}

		public void RemoveQuotaItem(MonoItem item)
		{
			if (_items.ContainsKey(item))
			{
				QuotaContainerItemData item2 = _items[item];
				_itemsData.Remove(item2);
				_items.Remove(item);
				_quotaContainerModel.UpdateContainerItems(_itemsData);
			}
		}

		public QuotaContainerItemData GetQuotaItem(MonoItem item)
		{
			return _items.GetValueOrDefault(item);
		}
	}
}
