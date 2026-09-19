namespace Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Core
{
	public interface ISessionEndAnalyticsService
	{
		void PrepareForApplicationQuit();

		bool TrySendSessionEnd(bool treatUnknownReasonAsDisconnect = false);

		bool ShouldSendOnShutdown();
	}
}
