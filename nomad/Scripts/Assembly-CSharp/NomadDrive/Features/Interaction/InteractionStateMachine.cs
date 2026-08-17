using System;
using System.Collections.Generic;
using UnityEngine;

namespace NomadDrive.Features.Interaction
{
	public class InteractionStateMachine<TState> : IInteractionStateMachine where TState : Enum
	{
		private readonly Interactable _owner;

		private readonly Dictionary<int, InteractionStateConfig> _stateConfigs = new Dictionary<int, InteractionStateConfig>();

		private TState _currentState;

		private bool _isInitialized;

		public TState CurrentState => _currentState;

		public int CurrentStateValue => Convert.ToInt32(_currentState);

		public bool IsInitialized => _isInitialized;

		public event Action<TState, TState> OnStateChanged;

		public InteractionStateMachine(Interactable owner)
		{
			_owner = owner;
		}

		public InteractionStateMachine<TState> RegisterState(TState state, InteractionStateConfig config)
		{
			_stateConfigs[Convert.ToInt32(state)] = config;
			return this;
		}

		public InteractionStateMachine<TState> RegisterState(TState state, Func<InteractionStateConfig, InteractionStateConfig> configBuilder)
		{
			InteractionStateConfig arg = InteractionStateConfig.Create();
			_stateConfigs[Convert.ToInt32(state)] = configBuilder(arg);
			return this;
		}

		public void ClearAllStates()
		{
			_stateConfigs.Clear();
		}

		public void Initialize(TState initialState)
		{
			_currentState = initialState;
			_isInitialized = true;
			ApplyCurrentState();
		}

		public bool TransitionTo(TState newState)
		{
			if (!_isInitialized)
			{
				Initialize(newState);
				return true;
			}
			if (EqualityComparer<TState>.Default.Equals(_currentState, newState))
			{
				return false;
			}
			TState currentState = _currentState;
			_currentState = newState;
			ApplyCurrentState();
			this.OnStateChanged?.Invoke(currentState, newState);
			return true;
		}

		public void RefreshCurrentState()
		{
			if (_isInitialized)
			{
				ApplyCurrentState();
			}
		}

		public bool HasState(TState state)
		{
			return _stateConfigs.ContainsKey(Convert.ToInt32(state));
		}

		public void UpdateStateConfig(TState state, InteractionStateConfig config)
		{
			_stateConfigs[Convert.ToInt32(state)] = config;
			if (_isInitialized && EqualityComparer<TState>.Default.Equals(_currentState, state))
			{
				ApplyCurrentState();
			}
		}

		private void ApplyCurrentState()
		{
			int key = Convert.ToInt32(_currentState);
			if (!_stateConfigs.TryGetValue(key, out var value))
			{
				return;
			}
			RemoveAllInteractions();
			_owner.CrosshairType = value.CrosshairType;
			_owner.isNameLabelVisible = value.IsNameLabelVisible;
			_owner.isInteractionLabelVisible = value.IsInteractionLabelVisible;
			foreach (InteractionDefinition interaction2 in value.Interactions)
			{
				Interaction interaction = null;
				if (interaction2.Type == InteractionType.Basic)
				{
					interaction = _owner.CreateBasicInteraction(interaction2.Action, interaction2.DisplayText, interaction2.Key, overrideIfExists: true);
				}
				else if (interaction2.Type == InteractionType.Hold)
				{
					interaction = _owner.CreateHoldInteraction(interaction2.Action, interaction2.DisplayText, interaction2.Duration, interaction2.Key, overrideIfExists: true);
				}
				if (interaction != null && interaction2.Condition != null)
				{
					interaction.Condition = interaction2.Condition;
				}
			}
			_owner.OnInteractionActivityPerformed.Invoke();
		}

		private void RemoveAllInteractions()
		{
			Interaction[] array = _owner.ActiveInteractions.ToArray();
			foreach (Interaction interaction in array)
			{
				_owner.ActiveInteractions.Remove(interaction);
				UnityEngine.Object.Destroy(interaction);
			}
		}
	}
}
