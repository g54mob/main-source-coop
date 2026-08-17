using System;

namespace NomadDrive.Features.Interaction
{
	[Serializable]
	public class InteractionDefinition
	{
		public InteractionType Type;

		public InteractionKey Key;

		public string DisplayText;

		public float Duration;

		private Action _action;

		public Func<bool> Condition { get; set; }

		public Action Action => _action;

		public InteractionDefinition(InteractionType type, InteractionKey key, string displayText, Action action, float duration = 0f)
		{
			Type = type;
			Key = key;
			DisplayText = displayText;
			Duration = duration;
			_action = action;
		}

		public static InteractionDefinition Basic(InteractionKey key, string displayText, Action action)
		{
			return new InteractionDefinition(InteractionType.Basic, key, displayText, action);
		}

		public static InteractionDefinition Hold(InteractionKey key, string displayText, Action action, float duration)
		{
			return new InteractionDefinition(InteractionType.Hold, key, displayText, action, duration);
		}
	}
}
