namespace NomadDrive.Features.Interaction
{
	public interface IStaticInteractableManager
	{
		bool Register(StaticInteractable interactable);

		void Unregister(StaticInteractable interactable);

		void RequestStateChange(int id, byte newState);

		void RequestExtendedStateChange(int id, byte newState, byte normalizedValue);

		StaticInteractableState? GetState(int id);

		StaticInteractableExtendedState? GetExtendedState(int id);
	}
}
