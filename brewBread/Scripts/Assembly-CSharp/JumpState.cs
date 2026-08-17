using System;
using UnityEngine;

[Serializable]
public class JumpState : State
{
	private float lastPosition;

	public JumpState(CharacterStateController controller, StateMachine machine)
		: base(controller, machine)
	{
		_state = CharacterStates.JUMP;
	}

	public override void Enter()
	{
		controller.rb.velocity = Vector2.up * controller.jumpForce;
		lastPosition = controller.transform.position.y;
		controller.SetAnimation("Jump");
		base.Enter();
	}

	public override void HandleInput(Inputs input)
	{
		if (input == Inputs.GRAB && controller.IsTouchingWall())
		{
			controller.SetState(CharacterStates.GRAB);
		}
	}

	public override void LogicUpdate()
	{
		base.LogicUpdate();
		if (lastPosition > controller.transform.position.y)
		{
			controller.SetState(CharacterStates.FALL);
		}
		else
		{
			lastPosition = controller.transform.position.y;
		}
		if (controller.Jump && StaticInstance<AssistModeManager>.Instance.InfiniteJumps)
		{
			controller.rb.velocity = Vector2.up * controller.jumpForce;
			lastPosition = controller.transform.position.y;
			controller.SetAnimation("Jump");
		}
		controller.Jump = false;
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
		controller.transform.position += new Vector3(controller.movement, 0f, 0f) * Time.fixedDeltaTime * controller.speed;
	}

	public override void Exit()
	{
		base.Exit();
		controller.SetAnimation("Fall");
	}
}
