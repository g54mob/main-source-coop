using System;
using UnityEngine;
using UnityEngine.Events;

namespace NomadDrive.Features.Interaction
{
	public class Interaction : MonoBehaviour
	{
		public string interactionString;

		[SerializeField]
		private InteractionKey interactionKey;

		[SerializeField]
		private InteractionStateHandler _stateHandler;

		[SerializeField]
		private InteractionType interactionType;

		public UnityEvent OnInteractionStarted = new UnityEvent();

		public UnityEvent OnInteractionCancelled = new UnityEvent();

		public UnityEvent OnInteractionCompleted = new UnityEvent();

		private Action<InteractionState> _stateChangedHandler;

		private UnityAction _startedStateAction;

		private UnityAction _cancelledStateAction;

		private UnityAction _completedStateAction;

		public InteractionKey InteractionKey
		{
			get
			{
				return interactionKey;
			}
			set
			{
				interactionKey = value;
			}
		}

		public InteractionStateHandler StateHandler
		{
			get
			{
				return _stateHandler;
			}
			private set
			{
				_stateHandler = value;
			}
		}

		public InteractionType InteractionType
		{
			get
			{
				return interactionType;
			}
			set
			{
				interactionType = value;
			}
		}

		public Func<bool> Condition { get; set; }

		public event Action<InteractionState> OnInteractionProcessStateChanged;

		private void OnEnable()
		{
			_stateHandler = new InteractionStateHandler(InteractionState.Ready);
			_stateChangedHandler = delegate(InteractionState state)
			{
				this.OnInteractionProcessStateChanged?.Invoke(state);
			};
			_stateHandler.OnStateChanged += _stateChangedHandler;
			_startedStateAction = delegate
			{
				StateHandler.SetState(InteractionState.Started);
			};
			_cancelledStateAction = delegate
			{
				StateHandler.SetState(InteractionState.Cancelled);
			};
			_completedStateAction = delegate
			{
				StateHandler.SetState(InteractionState.Completed);
			};
			OnInteractionStarted.AddListener(_startedStateAction);
			OnInteractionCancelled.AddListener(_cancelledStateAction);
			OnInteractionCompleted.AddListener(_completedStateAction);
		}

		private void OnDisable()
		{
			if (_stateHandler != null)
			{
				_stateHandler.OnStateChanged -= _stateChangedHandler;
			}
			OnInteractionStarted.RemoveListener(_startedStateAction);
			OnInteractionCancelled.RemoveListener(_cancelledStateAction);
			OnInteractionCompleted.RemoveListener(_completedStateAction);
		}

		public void SetInteractionString(string intString)
		{
			interactionString = intString;
		}

		public void SetInteractionKey(InteractionKey key)
		{
			interactionKey = key;
		}

		public void Clear()
		{
			OnInteractionStarted.RemoveAllListeners();
			OnInteractionCancelled.RemoveAllListeners();
			OnInteractionCompleted.RemoveAllListeners();
			OnInteractionStarted.AddListener(_startedStateAction);
			OnInteractionCancelled.AddListener(_cancelledStateAction);
			OnInteractionCompleted.AddListener(_completedStateAction);
		}

		public void Deactivate()
		{
			StateHandler.SetState(InteractionState.Deactivated);
		}

		public void Activate()
		{
			StateHandler.SetState(InteractionState.Ready);
		}

		public void ResetStartAction(Action action)
		{
			OnInteractionStarted = new UnityEvent();
			OnInteractionStarted.AddListener(action.Invoke);
		}

		public void ResetCancelAction(Action action)
		{
			OnInteractionCancelled = new UnityEvent();
			OnInteractionCancelled.AddListener(action.Invoke);
		}

		public void ResetCompleteAction(Action action)
		{
			OnInteractionCompleted = new UnityEvent();
			OnInteractionCompleted.AddListener(action.Invoke);
		}

		public void AddStartAction(Action action)
		{
			OnInteractionStarted.AddListener(action.Invoke);
		}

		public void AddCancelAction(Action action)
		{
			OnInteractionCancelled.AddListener(action.Invoke);
		}

		public void AddCompleteAction(Action action)
		{
			OnInteractionCompleted.AddListener(action.Invoke);
		}
	}
}
