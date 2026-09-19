namespace Features.GameJournalingModule.Scripts.Core
{
	public interface IAnalyticsEventSendService
	{
		void NewDesignEvent(string eventName);

		void NewDesignEvent(string eventName, float value);

		void TrackMainMenuOpened();

		void TrackSessionStart(bool isFirstSession, int partySize);

		void TrackLocationEntered();

		void TrackEnemyEncountered();

		void TrackItemLoaded();

		void TrackItemPickedUp();
	}
}
