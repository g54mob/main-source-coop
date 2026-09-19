using UnityEngine;

namespace Features.AIModule.Scripts.AttractionZone
{
	public readonly struct AttractionZoneData
	{
		public readonly int ZoneId;

		public readonly EnemyType SourceEnemyType;

		public readonly Vector3 Origin;

		public readonly float AttractRadius;

		public readonly float ApproachRadius;

		public AttractionZoneData(int zoneId, EnemyType sourceEnemyType, Vector3 origin, float attractRadius, float approachRadius)
		{
			ZoneId = zoneId;
			SourceEnemyType = sourceEnemyType;
			Origin = origin;
			AttractRadius = attractRadius;
			ApproachRadius = approachRadius;
		}
	}
}
