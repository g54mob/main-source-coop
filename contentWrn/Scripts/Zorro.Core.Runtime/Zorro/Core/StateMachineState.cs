namespace Zorro.Core
{
	public class StateMachineState
	{
		protected bool activeState;

		public virtual void Enter()
		{
			activeState = true;
		}

		public virtual void Exit()
		{
			activeState = false;
		}
	}
}
