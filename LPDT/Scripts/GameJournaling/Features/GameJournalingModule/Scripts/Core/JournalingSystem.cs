using System.Collections.Generic;

namespace Features.GameJournalingModule.Scripts.Core
{
	public class JournalingSystem : IJournalingSystem
	{
		private readonly IAnalyticsEventSendService _analyticsEventSendService;

		private readonly List<IJournalingSubsystem> _subsystems = new List<IJournalingSubsystem>();

		public JournalingSystem(IAnalyticsEventSendService analyticsEventSendService)
		{
			_analyticsEventSendService = analyticsEventSendService;
		}

		public void RegisterJournalingSubsystem(IJournalingSubsystem subsystem)
		{
			_subsystems.Add(subsystem);
			subsystem.OnJournalingEventSubmitted += OnJournalingEventSubmitted;
		}

		public void UnregisterJournalingSubsystem(IJournalingSubsystem subsystem)
		{
			_subsystems.Remove(subsystem);
			subsystem.OnJournalingEventSubmitted -= OnJournalingEventSubmitted;
		}

		private void OnJournalingEventSubmitted(IJournalingEvent journalingEvent)
		{
			journalingEvent.Submit(_analyticsEventSendService);
		}
	}
}
