namespace Features.GameJournalingModule.Scripts.Core
{
	public interface IJournalingSystem
	{
		void RegisterJournalingSubsystem(IJournalingSubsystem subsystem);

		void UnregisterJournalingSubsystem(IJournalingSubsystem subsystem);
	}
}
