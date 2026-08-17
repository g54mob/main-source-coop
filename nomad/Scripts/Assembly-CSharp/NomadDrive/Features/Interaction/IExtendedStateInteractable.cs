namespace NomadDrive.Features.Interaction
{
	public interface IExtendedStateInteractable
	{
		void ApplyExtendedStateFromManager(byte stateData, float normalizedValue, bool skipAnimation);
	}
}
