using System;
using UnityEngine;

namespace NomadDrive.Features.Interaction
{
	[Serializable]
	public class InteractionStateHandler
	{
		private InteractionState _currentState;

		[SerializeField]
		public InteractionState CurrentState => _currentState;

		public event Action<InteractionState> OnStateChanged;

		public InteractionStateHandler(InteractionState initialState)
		{
			_currentState = initialState;
		}

		public void SetState(InteractionState newState)
		{
			if (_currentState != newState)
			{
				_currentState = newState;
				this.OnStateChanged?.Invoke(_currentState);
			}
		}

		public bool IsInState(InteractionState state)
		{
			return _currentState == state;
		}

		public void Reset()
		{
			SetState(InteractionState.Ready);
		}
	}
}
