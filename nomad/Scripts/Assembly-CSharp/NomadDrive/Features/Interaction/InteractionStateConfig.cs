using System;
using System.Collections.Generic;
using EvilCore.UI.Scripts;

namespace NomadDrive.Features.Interaction
{
	public class InteractionStateConfig
	{
		public List<InteractionDefinition> Interactions { get; } = new List<InteractionDefinition>();

		public CrosshairType CrosshairType { get; private set; } = CrosshairType.Interact;

		public bool IsNameLabelVisible { get; private set; } = true;

		public bool IsInteractionLabelVisible { get; private set; } = true;

		public InteractionStateConfig WithBasicInteraction(InteractionKey key, string displayText, Action action)
		{
			Interactions.Add(InteractionDefinition.Basic(key, displayText, action));
			return this;
		}

		public InteractionStateConfig WithHoldInteraction(InteractionKey key, string displayText, Action action, float duration)
		{
			Interactions.Add(InteractionDefinition.Hold(key, displayText, action, duration));
			return this;
		}

		public InteractionStateConfig WithCrosshair(CrosshairType crosshairType)
		{
			CrosshairType = crosshairType;
			return this;
		}

		public InteractionStateConfig WithNameLabelVisibility(bool visible)
		{
			IsNameLabelVisible = visible;
			return this;
		}

		public InteractionStateConfig WithInteractionLabelVisibility(bool visible)
		{
			IsInteractionLabelVisible = visible;
			return this;
		}

		public InteractionStateConfig WithCondition(Func<bool> condition)
		{
			if (Interactions.Count > 0)
			{
				List<InteractionDefinition> interactions = Interactions;
				interactions[interactions.Count - 1].Condition = condition;
			}
			return this;
		}

		public static InteractionStateConfig Create()
		{
			return new InteractionStateConfig();
		}
	}
}
