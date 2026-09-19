using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.Core
{
	public interface IReadyHoleLocator
	{
		bool TryGetNearestReadyHolePosition(Vector3 worldPosition, out Vector3 holePosition);
	}
}
