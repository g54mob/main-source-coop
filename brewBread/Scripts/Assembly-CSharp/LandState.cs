using System;
using UnityEngine;

[Serializable]
public class LandState : State
{
	[SerializeField]
	private float stunTime = 0.8f;

	private float t;

	public LandState(CharacterStateController controller, StateMachine machine)
		: base(controller, machine)
	{
		_state = CharacterStates.LAND;
	}

	public override void Enter()
	{
		base.Enter();
		controller.SetAnimation("LandHard");
		controller.GamepadRumble();
		StaticInstance<CameraController>.Instance.ShakeCamera();
		t = stunTime;
	}

	public override void HandleInput(Inputs input)
	{
		if (t <= 0f)
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
	}

	public override void LogicUpdate()
	{
		base.LogicUpdate();
		if (!controller.GetGrounded())
		{
			controller.SetState(CharacterStates.HARDFALL);
		}
		if (controller.movement != 0f && t <= 0f)
		{
			controller.SetAnimation("Idle");
			controller.SetState(CharacterStates.GROUNDED);
		}
		t -= Time.deltaTime;
	}

	public override void Exit()
	{
		base.Exit();
		controller.animator.SetBool("Land", value: false);
	}
}
