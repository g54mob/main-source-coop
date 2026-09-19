using System;
using Features.CustomNetworkEventsModule.Scripts;
using UnityEngine;

namespace Features.SceneTransitionsModule.Scripts
{
	[Serializable]
	public class StartFadeTransitionNetworkEvent : NetworkEventBase<StartFadeTransitionNetworkEvent>
	{
		[field: SerializeField]
		public FadeTransitionType TransitionType { get; private set; }

		[field: SerializeField]
		public bool OnlyOnClient { get; private set; }

		[field: SerializeField]
		public bool Unscaled { get; private set; }

		[field: SerializeField]
		public int Owner { get; private set; }

		public void SendEvent(FadeTransitionType transitionType, bool unscaled, bool onlyOnClient, int owner)
		{
			TransitionType = transitionType;
			Unscaled = unscaled;
			OnlyOnClient = onlyOnClient;
			Owner = owner;
			Send();
		}
	}
}
