using Features.GameJournalingModule.Scripts.AnalyticsMarkService;

namespace Features.GameJournalingModule.Scripts.Core
{
	public interface IConcreteJournalingSystem
	{
		bool IsEnabled { get; }

		IAnalyticsEventSendService AnalyticsEventSendService { get; }

		IAnalyticsMarkUserService AnalyticsMarkUserService { get; }
	}
}
