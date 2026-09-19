using System;

namespace Global.StateMachinesModule.Scripts
{
	public class StateMachine<T> where T : Enum
	{
		public T CurrentState;

		public event Action<T> OnStateEnter;

		public event Action<T> OnStateExit;

		public void EnterState(T state)
		{
			this.OnStateExit?.Invoke(CurrentState);
			CurrentState = state;
			this.OnStateEnter?.Invoke(CurrentState);
		}
	}
}
