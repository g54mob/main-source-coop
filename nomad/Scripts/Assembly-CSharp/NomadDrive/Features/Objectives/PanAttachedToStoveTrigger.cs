using System;

namespace NomadDrive.Features.Objectives
{
	[Serializable]
	public class PanAttachedToStoveTrigger : ObjectiveTrigger
	{
		protected override void OnActivate()
		{
			ObjectivesEventBus.SignalRaised += HandleSignal;
		}

		protected override void OnDeactivate()
		{
			ObjectivesEventBus.SignalRaised -= HandleSignal;
		}

		private void HandleSignal(ObjectiveSignal signal, object payload)
		{
			if (signal == ObjectiveSignal.PanAttachedToStove)
			{
				Fire?.Invoke();
			}
		}
	}
}
