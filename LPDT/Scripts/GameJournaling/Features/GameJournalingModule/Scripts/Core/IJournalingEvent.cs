using Features.GameJournalingModule.Scripts.AnalyticsMarkService;

namespace Features.GameJournalingModule.Scripts.Core
{
	public interface IJournalingEvent
	{
		void Submit(IAnalyticsEventSendService analyticsEventSendService)
		{
		}

		void MarkUser(IAnalyticsMarkUserService analyticsMarkUserService)
		{
		}
	}
}
