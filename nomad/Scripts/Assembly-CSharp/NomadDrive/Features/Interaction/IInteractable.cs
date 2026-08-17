using System.Collections.Generic;
using EvilCore.UI.Scripts;
using UnityEngine;
using UnityEngine.Events;

namespace NomadDrive.Features.Interaction
{
	public interface IInteractable
	{
		string interactableName { get; }

		GameObject gameObject { get; }

		Transform transform { get; }

		Transform InteractionDisplayPoint { get; }

		CrosshairType CrosshairType { get; set; }

		bool isNameLabelVisible { get; set; }

		bool isInteractionLabelVisible { get; set; }

		bool AvailableForInteraction { get; }

		List<Interaction> ActiveInteractions { get; }

		UnityEvent onHovered { get; }

		UnityEvent onUnhovered { get; }

		UnityEvent OnInteractionAvailabilityChanged { get; }

		UnityEvent OnInteractionActivityPerformed { get; }

		Interaction[] GetInteractions();

		bool IsHoveringIgnored();

		void EnableOutline();

		void DisableOutline();
	}
}
