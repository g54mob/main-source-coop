namespace GameplayEvents
{
	public class OnPlayerDiedGameplayEvent : GameplayEvent
	{
		public readonly int PlayerId;

		public readonly bool IsLocal;

		public OnPlayerDiedGameplayEvent(int playerId, bool isLocal)
		{
			PlayerId = playerId;
			IsLocal = isLocal;
		}
	}
}
