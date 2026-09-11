using System;
using System.Collections.Generic;
using UnityEngine;

namespace Zorro.Core
{
	public class StateMachine<StateType> where StateType : StateMachineState
	{
		public StateType CurrentState;

		private Dictionary<Type, StateType> m_states = new Dictionary<Type, StateType>();

		public void RegisterState(StateType state)
		{
			Type type = state.GetType();
			if (m_states.ContainsKey(type))
			{
				m_states[type] = state;
			}
			else
			{
				m_states.Add(type, state);
			}
		}

		public T SwitchState<T>() where T : StateType
		{
			return SwitchState(typeof(T)) as T;
		}

		public StateType SwitchState(Type gamestateType)
		{
			if (m_states.ContainsKey(gamestateType))
			{
				if (CurrentState != null && CurrentState.GetType() != gamestateType)
				{
					CurrentState.Exit();
					CurrentState = m_states[gamestateType];
					Debug.Log("Switched State to: " + gamestateType.Name);
					CurrentState.Enter();
				}
				else if (CurrentState == null)
				{
					CurrentState = m_states[gamestateType];
					Debug.Log("Switched State to: " + gamestateType.Name);
					CurrentState.Enter();
				}
				return CurrentState;
			}
			if (CurrentState != null)
			{
				CurrentState.Exit();
			}
			CurrentState = null;
			Debug.LogError("Tried to transistion to state of type: " + gamestateType.Name + " but no such game state as registered");
			return null;
		}
	}
}
