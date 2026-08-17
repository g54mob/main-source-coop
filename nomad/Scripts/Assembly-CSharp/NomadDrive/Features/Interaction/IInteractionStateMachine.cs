namespace NomadDrive.Features.Interaction
{
	public interface IInteractionStateMachine
	{
		int CurrentStateValue { get; }

		bool IsInitialized { get; }

		void RefreshCurrentState();

		void ClearAllStates();
	}
}
