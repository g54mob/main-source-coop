using System;

[Serializable]
public class State
{
	protected CharacterStateController controller;

	protected StateMachine machine;

	public CharacterStates _state;

	private float movementTimer;

	public State(CharacterStateController controller, StateMachine machine)
	{
		this.controller = controller;
		this.machine = machine;
	}

	public void Initialize(CharacterStateController controller, StateMachine machine)
	{
		this.controller = controller;
		this.machine = machine;
	}

	public virtual void Enter()
	{
	}

	public virtual void HandleInput(Inputs input)
	{
	}

	public virtual void LogicUpdate()
	{
	}

	public virtual void PhysicsUpdate()
	{
	}

	public virtual void Exit()
	{
	}
}
