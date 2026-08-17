public class StateMachine
{
	public State CurrentState;

	public void Initialize(State initialSteate)
	{
		CurrentState = initialSteate;
		initialSteate.Enter();
	}

	public void ChangeState(State newState, CharacterStates newStateEnum)
	{
		CurrentState.Exit();
		CurrentState = newState;
		CurrentState._state = newStateEnum;
		newState.Enter();
	}
}
