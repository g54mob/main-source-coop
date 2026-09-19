namespace Features.AIModule.Scripts.AttractionZone
{
	public interface IEnemyAttractionZoneCallbackListener
	{
		EnemyType EnemyType { get; }

		int EnemyInstants { get; }

		void OnAttractionZoneRaised(AttractionZoneData zone);

		void OnAttractionZoneEnded(int zoneId);
	}
}
