using Features.ItemCollisionModule.Scripts;
using Fusion;

namespace Features.ItemDamageModule.Scripts
{
	public interface IItemCostReduceService
	{
		bool ProcessItemCollisionData(ItemCollisionData collisionData, PlayerRef? playerThatDamage = null, bool isIgnoreLimits = false);

		bool BreakItemCompletely(ItemCollisionData collisionData, PlayerRef? playerThatDamage = null);
	}
}
