using UnityEngine;

namespace NomadDrive.Features.Interaction
{
	public class HoldInteraction : Interaction
	{
		[SerializeField]
		[Range(0f, 10f)]
		private float interactionDuration;

		private float _elapsedInteractionTime;

		private float _startTime;

		private bool _isInteracting;

		public float InteractionDuration => interactionDuration;

		private void Awake()
		{
			base.InteractionType = InteractionType.Hold;
		}

		private void Update()
		{
			if (_isInteracting)
			{
				_elapsedInteractionTime = Time.time - _startTime;
				if (_elapsedInteractionTime >= interactionDuration)
				{
					Complete();
				}
			}
		}

		public void SetInteractionDuration(float duration)
		{
			interactionDuration = duration;
		}

		public void StartInteraction()
		{
			base.StateHandler.SetState(InteractionState.Started);
			OnInteractionStarted.Invoke();
			StartInteractionProcess();
		}

		public void StartInteractionProcess()
		{
			_elapsedInteractionTime = 0f;
			_startTime = Time.time;
			_isInteracting = true;
		}

		public void Cancel()
		{
			base.StateHandler.SetState(InteractionState.Cancelled);
			_isInteracting = false;
			OnInteractionCancelled.Invoke();
		}

		public void Complete()
		{
			base.StateHandler.SetState(InteractionState.Completed);
			_isInteracting = false;
			OnInteractionCompleted.Invoke();
		}

		public void Reset()
		{
			if (!base.StateHandler.IsInState(InteractionState.Deactivated))
			{
				base.StateHandler.SetState(InteractionState.Ready);
				_elapsedInteractionTime = 0f;
				_isInteracting = false;
			}
		}
	}
}
