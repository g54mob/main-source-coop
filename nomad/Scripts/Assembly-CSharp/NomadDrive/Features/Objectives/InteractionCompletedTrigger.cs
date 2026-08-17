using System;
using NomadDrive.Features.Interaction;
using UnityEngine;

namespace NomadDrive.Features.Objectives
{
	[Serializable]
	public class InteractionCompletedTrigger : ObjectiveTrigger
	{
		[Tooltip("Interactable being acted on must carry this Component type.")]
		public ComponentTypeReference TargetType = new ComponentTypeReference();

		protected override void OnActivate()
		{
			if (Context?.InteractionManager != null)
			{
				Context.InteractionManager.OnInteractionAnimTriggered += HandleInteraction;
			}
		}

		protected override void OnDeactivate()
		{
			if (Context?.InteractionManager != null)
			{
				Context.InteractionManager.OnInteractionAnimTriggered -= HandleInteraction;
			}
		}

		private void HandleInteraction(IInteractable interactable)
		{
			if (interactable != null && TargetType.IsAssigned && TargetType.IsMatch(interactable.gameObject))
			{
				Fire?.Invoke();
			}
		}
	}
}
