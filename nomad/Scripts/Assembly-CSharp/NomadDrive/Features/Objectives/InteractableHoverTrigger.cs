using System;
using NomadDrive.Features.Interaction;
using UnityEngine;

namespace NomadDrive.Features.Objectives
{
	[Serializable]
	public class InteractableHoverTrigger : ObjectiveTrigger
	{
		[Tooltip("Hovered IInteractable must carry this Component type.")]
		public ComponentTypeReference TargetType = new ComponentTypeReference();

		protected override void OnActivate()
		{
			if (Context?.InteractionManager != null)
			{
				Context.InteractionManager.OnInteractableHovered += HandleHovered;
			}
		}

		protected override void OnDeactivate()
		{
			if (Context?.InteractionManager != null)
			{
				Context.InteractionManager.OnInteractableHovered -= HandleHovered;
			}
		}

		private void HandleHovered(IInteractable interactable)
		{
			if (interactable != null && TargetType.IsAssigned && TargetType.IsMatch(interactable.gameObject))
			{
				Fire?.Invoke();
			}
		}
	}
}
