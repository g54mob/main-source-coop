namespace GameplayEvents
{
	public class OnLineArmGrabNullResolveGameplayEvent : GameplayEvent
	{
		public readonly int GrabberPlayerId;

		public readonly int ObserverPlayerId;

		public readonly long GrabbedObjectRawId;

		public OnLineArmGrabNullResolveGameplayEvent(int grabberPlayerId, int observerPlayerId, long grabbedObjectRawId)
		{
			GrabberPlayerId = grabberPlayerId;
			ObserverPlayerId = observerPlayerId;
			GrabbedObjectRawId = grabbedObjectRawId;
		}
	}
}
