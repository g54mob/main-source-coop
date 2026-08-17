namespace NomadDrive.Features.Interaction
{
	public interface IStaticInteractionStateMachine
	{
		int CurrentStateValue { get; }

		bool IsInitialized { get; }

		void RefreshCurrentState();

		void ClearAllStates();
	}
}
