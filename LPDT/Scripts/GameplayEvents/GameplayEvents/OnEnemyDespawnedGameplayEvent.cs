namespace GameplayEvents
{
	public class OnEnemyDespawnedGameplayEvent : GameplayEvent
	{
		public readonly string EnemyKind;

		public readonly string NetworkObjectId;

		public OnEnemyDespawnedGameplayEvent(string enemyKind, string networkObjectId)
		{
			EnemyKind = enemyKind;
			NetworkObjectId = networkObjectId;
		}
	}
}
