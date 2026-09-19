using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.Services
{
	public interface IEnemyTrackingService
	{
		void RegisterTarget(IEnemyTrackable target);

		void UnregisterTarget(IEnemyTrackable target);

		bool TryGetNearestTarget(Vector3 origin, float radius, out IEnemyTrackable target);

		void RegisterThreat(IEnemyTrackable threat);

		void UnregisterThreat(IEnemyTrackable threat);

		bool TryGetNearestThreat(Vector3 origin, float radius, out IEnemyTrackable threat, out float distance);
	}
}
