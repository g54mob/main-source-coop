using UnityEngine;

namespace Features.AIModule.Scripts
{
	public interface IEnemyTargetProvider
	{
		Transform HitTarget { get; }
	}
}
