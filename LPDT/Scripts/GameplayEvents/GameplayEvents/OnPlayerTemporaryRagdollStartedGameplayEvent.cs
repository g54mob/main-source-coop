namespace GameplayEvents
{
	public class OnPlayerTemporaryRagdollStartedGameplayEvent : GameplayEvent
	{
		public readonly int PlayerId;

		public readonly bool IsLocal;

		public readonly string Reasons;

		public OnPlayerTemporaryRagdollStartedGameplayEvent(int playerId, bool isLocal, string reasons)
		{
			PlayerId = playerId;
			IsLocal = isLocal;
			Reasons = reasons;
		}
	}
}
