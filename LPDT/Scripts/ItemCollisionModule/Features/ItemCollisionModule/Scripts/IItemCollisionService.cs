using Features.ItemsModule.Scripts;
using UnityEngine;

namespace Features.ItemCollisionModule.Scripts
{
	public interface IItemCollisionService
	{
		void ProcessIItemCollision(IItem item, float force, float itemMass, Collision other);
	}
}
