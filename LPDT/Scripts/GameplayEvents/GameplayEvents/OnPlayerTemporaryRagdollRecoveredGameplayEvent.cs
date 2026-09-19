namespace GameplayEvents
{
	public class OnPlayerTemporaryRagdollRecoveredGameplayEvent : GameplayEvent
	{
		public readonly int PlayerId;

		public readonly bool IsLocal;

		public readonly float DurationSeconds;

		public OnPlayerTemporaryRagdollRecoveredGameplayEvent(int playerId, bool isLocal, float durationSeconds)
		{
			PlayerId = playerId;
			IsLocal = isLocal;
			DurationSeconds = durationSeconds;
		}
	}
}
