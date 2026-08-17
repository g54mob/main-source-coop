using System;
using UnityEngine;

[Serializable]
public class WeightState : State
{
	public WeightState(CharacterStateController controller, StateMachine machine)
		: base(controller, machine)
	{
		_state = CharacterStates.WEIGHT;
	}

	public override void Enter()
	{
		base.Enter();
		RaycastHit2D raycastHit2D = Physics2D.Raycast(controller.transform.position, Vector2.down, 1f, controller.groundLayer);
		if ((bool)raycastHit2D)
		{
			controller.transform.position = new Vector2(controller.transform.position.x, raycastHit2D.point.y + 0.65f);
		}
		controller.SetAnimation("Weight");
		controller.SetPlayerImmobile();
	}

	public override void LogicUpdate()
	{
		base.LogicUpdate();
		if (!controller.GetGrounded())
		{
			controller.SetState(CharacterStates.FALL);
		}
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
	}

	public override void HandleInput(Inputs input)
	{
		switch (input)
		{
		case Inputs.WEIGHT:
			controller.SetState(CharacterStates.GROUNDED);
			controller.SetAnimation("Idle");
			break;
		case Inputs.JUMP:
			controller.SetState(CharacterStates.JUMP);
			break;
		}
	}

	public override void Exit()
	{
		base.Exit();
		controller.SetPlayerMobile();
	}
}
