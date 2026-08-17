namespace NomadDrive.Features.Interaction
{
	public class BasicInteraction : Interaction
	{
		protected void Awake()
		{
			base.InteractionType = InteractionType.Basic;
		}

		public void StartInteraction()
		{
			base.StateHandler.SetState(InteractionState.Started);
			OnInteractionStarted.Invoke();
		}

		public void CompleteInteraction()
		{
			base.StateHandler.SetState(InteractionState.Completed);
			OnInteractionCompleted.Invoke();
			ResetInteraction();
		}

		public void CancelInteraction()
		{
			base.StateHandler.SetState(InteractionState.Cancelled);
			OnInteractionCancelled.Invoke();
		}

		public void ResetInteraction()
		{
			base.StateHandler.SetState(InteractionState.Ready);
		}
	}
}
