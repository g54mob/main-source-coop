using System;
using System.Collections.Generic;
using Features.ItemsModule.Scripts;
using UnityEngine;

namespace Features.ItemCollisionModule.Scripts
{
	public class ItemCollisionModel
	{
		public readonly List<ItemCollisionData> _collisionsList = new List<ItemCollisionData>();

		public event Action<ItemCollisionData> OnCollisionAdded;

		public event Action<ItemCollisionData> OnItemDamaged;

		public void InvokeOnItemDamaged(ItemCollisionData collisionData)
		{
			this.OnItemDamaged?.Invoke(collisionData);
		}

		public void AddItemCollision(IItem item, float force, float itemMass, Collision collision)
		{
			ItemCollisionData itemCollisionData = new ItemCollisionData(item, force, itemMass, collision.collider);
			_collisionsList.Add(itemCollisionData);
			this.OnCollisionAdded?.Invoke(itemCollisionData);
		}
	}
}
