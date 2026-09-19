namespace GameplayEvents
{
	public class OnItemGrabbedStateChangeEvent : GameplayEvent
	{
		public readonly int PlayerId;

		public readonly bool IsLocal;

		public readonly string LineArmTypeName;

		public readonly string ItemType;

		public OnItemGrabbedStateChangeEvent(int playerId, bool isLocal, string lineArmTypeName, string itemType)
		{
			PlayerId = playerId;
			IsLocal = isLocal;
			LineArmTypeName = lineArmTypeName;
			ItemType = itemType;
		}
	}
}
