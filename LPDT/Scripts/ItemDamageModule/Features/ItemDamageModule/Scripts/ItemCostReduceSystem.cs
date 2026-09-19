using System;
using Features.ItemCollisionModule.Scripts;
using Zenject;

namespace Features.ItemDamageModule.Scripts
{
	public class ItemCostReduceSystem : IInitializable, IDisposable
	{
		private readonly ItemCollisionModel _itemCollisionModel;

		private readonly IItemCostReduceService _itemCostReduceService;

		public ItemCostReduceSystem(ItemCollisionModel itemCollisionModel, IItemCostReduceService itemCostReduceService)
		{
			_itemCollisionModel = itemCollisionModel;
			_itemCostReduceService = itemCostReduceService;
		}

		public void Initialize()
		{
			_itemCollisionModel.OnCollisionAdded += ProcessItemCollision;
		}

		public void Dispose()
		{
			_itemCollisionModel.OnCollisionAdded -= ProcessItemCollision;
		}

		private void ProcessItemCollision(ItemCollisionData collisionData)
		{
			_itemCostReduceService.ProcessItemCollisionData(collisionData);
		}
	}
}
