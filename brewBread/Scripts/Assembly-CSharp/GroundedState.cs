using System;
using UnityEngine;

[Serializable]
public class GroundedState : State
{
	public GroundedState(CharacterStateController controller, StateMachine machine)
		: base(controller, machine)
	{
		_state = CharacterStates.GROUNDED;
	}

	public override void Enter()
	{
		base.Enter();
		controller.animator.SetInteger("IdleState", 0);
	}

	public override void HandleInput(Inputs input)
	{
		switch (input)
		{
		case Inputs.JUMP:
			controller.SetState(CharacterStates.JUMP);
			break;
		case Inputs.WEIGHT:
			controller.SetState(CharacterStates.WEIGHT);
			break;
		}
	}

	public override void LogicUpdate()
	{
		base.LogicUpdate();
		if (!controller.GetGrounded())
		{
			controller.SetState(CharacterStates.FALL);
			controller.fall.coyoteTime = 0.1f;
		}
		if (controller.movement != 0f)
		{
			controller.animator.SetInteger("IdleState", 0);
			controller.animator.SetBool("Walk", value: true);
			controller._skinAnimator.SetInteger("IdleState", 0);
			controller._skinAnimator.SetBool("Walk", value: true);
			if (controller.GetCompanionState() == CharacterStates.WALLFALL)
			{
				controller.animator.SetBool("Pulling", value: true);
				controller._skinAnimator.SetBool("Pulling", value: true);
			}
			else
			{
				controller.animator.SetBool("Pulling", value: false);
				controller._skinAnimator.SetBool("Pulling", value: false);
			}
		}
		else if (controller.animator.GetBool("Walk"))
		{
			controller.animator.SetBool("Walk", value: false);
			controller.animator.SetBool("Pulling", value: false);
			controller._skinAnimator.SetBool("Walk", value: false);
			controller._skinAnimator.SetBool("Pulling", value: false);
		}
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
		controller.transform.position += new Vector3(controller.movement, 0f, 0f) * Time.fixedDeltaTime * controller.speed;
	}

	public override void Exit()
	{
		base.Exit();
		controller.animator.SetBool("Land", value: false);
		controller.animator.SetBool("Walk", value: false);
	}
}
