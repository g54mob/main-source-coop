using System;

namespace Features.GameJournalingModule.Scripts.Core
{
	public interface IJournalingSubsystem
	{
		event Action<IJournalingEvent> OnJournalingEventSubmitted;
	}
}
