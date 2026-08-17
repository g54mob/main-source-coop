using System;
using EvilCore;
using EvilCore.Networking;

namespace NomadDrive.Features.Interaction
{
	public interface IInteractionManager : IInitialize, IUniqueNetworkComponent
	{
		IInteractable ActiveInteractable { get; set; }

		event Action<IInteractable> OnInteractableHovered;

		event Action<IInteractable> OnInteractableUnhovered;

		event Action<IInteractable> OnInteractionAnimTriggered;

		void EnableInteraction();

		void DisableInteraction();

		void HandleInteractionKeyPressed(InteractionKey interactionKey);

		void HandleInteractionKeyReleased(InteractionKey interactionKey);
	}
}
