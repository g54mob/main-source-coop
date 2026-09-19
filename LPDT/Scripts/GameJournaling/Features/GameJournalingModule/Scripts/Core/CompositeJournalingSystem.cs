using System.Collections.Generic;

namespace Features.GameJournalingModule.Scripts.Core
{
	public class CompositeJournalingSystem : IJournalingSystem
	{
		private readonly List<IConcreteJournalingSystem> _journalingSystems;

		public CompositeJournalingSystem(List<IConcreteJournalingSystem> journalingSystems)
		{
			_journalingSystems = journalingSystems;
		}

		public void RegisterJournalingSubsystem(IJournalingSubsystem subsystem)
		{
			subsystem.OnJournalingEventSubmitted += SubmitEvent;
		}

		public void UnregisterJournalingSubsystem(IJournalingSubsystem subsystem)
		{
			subsystem.OnJournalingEventSubmitted -= SubmitEvent;
		}

		private void SubmitEvent(IJournalingEvent journalingEvent)
		{
			foreach (IConcreteJournalingSystem journalingSystem in _journalingSystems)
			{
				if (journalingSystem.IsEnabled)
				{
					journalingEvent.Submit(journalingSystem.AnalyticsEventSendService);
					journalingEvent.MarkUser(journalingSystem.AnalyticsMarkUserService);
				}
			}
		}
	}
}
