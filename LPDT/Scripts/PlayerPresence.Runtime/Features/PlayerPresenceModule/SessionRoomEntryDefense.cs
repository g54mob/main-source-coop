namespace Features.PlayerPresenceModule
{
	public static class SessionRoomEntryDefense
	{
		public static bool ShouldSelfDisconnect(bool isInScope, bool hasOwnedObject, int framesInScope, int graceFrames)
		{
			if (!isInScope || hasOwnedObject)
			{
				return false;
			}
			return framesInScope >= graceFrames;
		}
	}
}
