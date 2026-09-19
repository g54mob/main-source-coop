using System;
using Features.ItemsModule.Scripts;
using UnityEngine;

namespace Features.ItemCollisionModule.Scripts
{
	[Serializable]
	public class ItemCollisionData
	{
		public IItem Item;

		public float Force;

		public float ItemMass;

		public Collider CollisionCollider;

		public ItemCollisionData(IItem item, float force, float itemMass, Collider collisionCollider)
		{
			Item = item;
			Force = force;
			ItemMass = itemMass;
			CollisionCollider = collisionCollider;
		}
	}
}
