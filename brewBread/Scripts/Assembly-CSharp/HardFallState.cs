using System;

[Serializable]
public class HardFallState : State
{
	public HardFallState(CharacterStateController controller, StateMachine machine)
		: base(controller, machine)
	{
		_state = CharacterStates.HARDFALL;
	}

	public override void Enter()
	{
		base.Enter();
		controller.SetAnimation("FallHard");
	}

	public override void HandleInput(Inputs input)
	{
		switch (input)
		{
		case Inputs.HANG:
			controller.SetState(CharacterStates.HANG);
			break;
		case Inputs.JUMP:
			if (StaticInstance<AssistModeManager>.Instance.InfiniteJumps)
			{
				controller.SetState(CharacterStates.JUMP);
			}
			break;
		}
	}

	public override void LogicUpdate()
	{
		base.LogicUpdate();
		if (controller.GetGrounded())
		{
			controller.SetState(CharacterStates.LAND);
		}
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
	}

	public override void Exit()
	{
		base.Exit();
	}
}
