namespace Features.BeachInteractableCommonModule.Scripts
{
	public interface IBeachInteractableController
	{
		BeachInteractableIdentifier BeachInteractableIdentifier { get; }

		void DespawnInteractable();
	}
}
