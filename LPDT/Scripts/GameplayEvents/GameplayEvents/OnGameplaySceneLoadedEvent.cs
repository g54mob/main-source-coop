namespace GameplayEvents
{
	public class OnGameplaySceneLoadedEvent : GameplayEvent
	{
		public readonly string LoadedScenePathOrName;

		public readonly string ActiveScenePathOrName;

		public OnGameplaySceneLoadedEvent(string loadedScenePathOrName, string activeScenePathOrName = null)
		{
			LoadedScenePathOrName = loadedScenePathOrName;
			ActiveScenePathOrName = activeScenePathOrName ?? loadedScenePathOrName;
		}
	}
}
