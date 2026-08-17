using System;
using UnityEngine;

namespace NomadDrive.Features.Objectives
{
	[Serializable]
	public class ObjectiveSignalTrigger : ObjectiveTrigger
	{
		[SerializeField]
		[Tooltip("Fires when this ObjectiveSignal is raised on the ObjectivesEventBus. The producing mechanic must call ObjectivesEventBus.Raise(signal) at the moment the action completes (usually inside a SyncVar hook that runs on all clients).")]
		private ObjectiveSignal signal;

		protected override void OnActivate()
		{
			ObjectivesEventBus.SignalRaised += HandleSignal;
		}

		protected override void OnDeactivate()
		{
			ObjectivesEventBus.SignalRaised -= HandleSignal;
		}

		private void HandleSignal(ObjectiveSignal raised, object payload)
		{
			if (raised == signal)
			{
				Fire?.Invoke();
			}
		}
	}
}
