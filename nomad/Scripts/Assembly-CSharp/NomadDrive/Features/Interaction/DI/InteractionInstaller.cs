using System;
using EvilCore.DI.Core;
using NomadDrive.Features.Interaction.UI;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.Interaction.DI
{
	[Serializable]
	public class InteractionInstaller : MonoInstaller
	{
		[SerializeField]
		private StaticInteractableManager staticInteractableManagerReference;

		[SerializeField]
		private InteractionUI interactionUIReference;

		[SerializeField]
		private InputActionPromptsPanel inputActionPromptsPanelReference;

		public override void Install(IContainerBuilder builder)
		{
			RegisterIfNotNull(builder, staticInteractableManagerReference, delegate(RegistrationBuilder c)
			{
				c.As<IStaticInteractableManager>();
			});
			RegisterIfNotNull(builder, interactionUIReference, delegate(RegistrationBuilder c)
			{
				c.As<InteractionUI>();
			});
			RegisterIfNotNull(builder, inputActionPromptsPanelReference);
		}
	}
}
