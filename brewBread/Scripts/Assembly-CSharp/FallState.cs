using System;
using UnityEngine;

[Serializable]
public class FallState : State
{
	[Tooltip("Time you must fall, before entering Hard Fall mode")]
	[SerializeField]
	private float fallTime;

	public float coyoteTime;

	private float fallTimeAux;

	public FallState(CharacterStateController controller, StateMachine machine)
		: base(controller, machine)
	{
		_state = CharacterStates.FALL;
	}

	public override void Enter()
	{
		base.Enter();
		controller.Jump = false;
		fallTimeAux = fallTime;
		controller.SetAnimation("Fall");
	}

	public override void HandleInput(Inputs input)
	{
		switch (input)
		{
		case Inputs.GRAB:
			if (controller.IsTouchingWall())
			{
				controller.SetState(CharacterStates.GRAB);
			}
			break;
		case Inputs.HANG:
			controller.SetState(CharacterStates.HANG);
			break;
		case Inputs.JUMP:
			if (coyoteTime >= 0f || StaticInstance<AssistModeManager>.Instance.InfiniteJumps)
			{
				controller.SetState(CharacterStates.JUMP);
			}
			break;
		case Inputs.WEIGHT:
			break;
		}
	}

	public override void LogicUpdate()
	{
		base.LogicUpdate();
		if (controller.GetGrounded())
		{
			controller.SetAnimation("Land");
			controller.SetState(CharacterStates.GROUNDED);
		}
		if (fallTimeAux <= 0f)
		{
			controller.SetState(CharacterStates.HARDFALL);
		}
		fallTimeAux -= Time.deltaTime;
		coyoteTime -= Time.deltaTime;
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
		controller.transform.position += new Vector3(controller.movement, 0f, 0f) * Time.fixedDeltaTime * controller.speed;
	}

	public override void Exit()
	{
		base.Exit();
	}
}
