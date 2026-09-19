using Features.ItemsModule.Scripts;
using UnityEngine;

namespace Features.ItemCollisionModule.Scripts
{
	public class ItemCollisionService : IItemCollisionService
	{
		private readonly ItemCollisionModel _itemCollisionModel;

		public ItemCollisionService(ItemCollisionModel itemCollisionModel)
		{
			_itemCollisionModel = itemCollisionModel;
		}

		public void ProcessIItemCollision(IItem item, float force, float itemMass, Collision other)
		{
			_itemCollisionModel.AddItemCollision(item, force, itemMass, other);
		}
	}
}
