namespace GameplayEvents
{
	public class OnPlayerTemporaryRagdollStuckGameplayEvent : GameplayEvent
	{
		public readonly int PlayerId;

		public readonly bool IsLocal;

		public readonly string Reasons;

		public readonly float DurationSeconds;

		public OnPlayerTemporaryRagdollStuckGameplayEvent(int playerId, bool isLocal, string reasons, float durationSeconds)
		{
			PlayerId = playerId;
			IsLocal = isLocal;
			Reasons = reasons;
			DurationSeconds = durationSeconds;
		}
	}
}
