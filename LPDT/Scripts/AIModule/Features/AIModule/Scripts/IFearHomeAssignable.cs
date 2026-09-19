using UnityEngine;

namespace Features.AIModule.Scripts
{
	public interface IFearHomeAssignable : IEnemyBehaviour, IEnemyTypeProvider
	{
		void ApplyFearHome(Vector3 homePosition, float homeMatchSqrDistance);
	}
}
