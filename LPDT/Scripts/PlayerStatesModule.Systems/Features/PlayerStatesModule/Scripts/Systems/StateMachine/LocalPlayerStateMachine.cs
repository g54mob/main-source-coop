using System;
using System.Collections.Generic;
using Core.StateMachineModule.Scripts;

namespace Features.PlayerStatesModule.Scripts.Systems.StateMachine
{
	public class LocalPlayerStateMachine : StateMachineBehaviour<LocalPlayerStateBase>
	{
		private readonly Dictionary<PlayerState, Type> _enumToType = new Dictionary<PlayerState, Type>();

		public PlayerState NextStateInTransition { get; private set; }

		public PlayerState CurrentStateEnum => ActiveStateBase?.StateEnum ?? PlayerState.None;

		public void Initialize(IReadOnlyList<LocalPlayerStateBase> states)
		{
			Dictionary<Type, LocalPlayerStateBase> dictionary = new Dictionary<Type, LocalPlayerStateBase>();
			foreach (LocalPlayerStateBase state in states)
			{
				Type type = state.GetType();
				dictionary[type] = state;
				_enumToType[state.StateEnum] = type;
			}
			SetStates(dictionary);
		}

		public void EnterByEnum(PlayerState newState)
		{
			if (_enumToType.TryGetValue(newState, out var value))
			{
				NextStateInTransition = newState;
				ActiveStateBase?.Exit();
				ActiveStateBase = States[value];
				ActiveStateBase.Enter();
				NextStateInTransition = PlayerState.None;
			}
		}
	}
}
