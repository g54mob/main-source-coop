namespace GameplayEvents
{
	public class OnPlayerResurrectedGameplayEvent : GameplayEvent
	{
		public readonly int PlayerId;

		public readonly bool IsLocal;

		public OnPlayerResurrectedGameplayEvent(int playerId, bool isLocal)
		{
			PlayerId = playerId;
			IsLocal = isLocal;
		}
	}
}
