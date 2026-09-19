using Features.NavigationModule.Scripts;
using UnityEngine;

namespace Features.AIModule.Scripts.AttractionZone
{
	public interface IEnemyAttractionZoneService
	{
		void RaiseZone(AttractionZoneData zone);

		void EndZone(int zoneId);

		bool TrySampleApproachPoint(INavigationService navigationService, Vector3 origin, float radius, int attempts, out Vector3 point);

		bool TrySampleApproachPointDirect(Vector3 origin, float radius, int attempts, out Vector3 point);
	}
}
