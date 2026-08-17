using System;
using UnityEngine;

[Serializable]
public class WallFallState : State
{
	[Header("Wall Fall")]
	[Tooltip("Force curve described by the dragging")]
	[SerializeField]
	private AnimationCurve dragForceCurve;

	[SerializeField]
	private float maxTimeStuck = 1f;

	private float pullForce;

	private float dragForce;

	private float timedragging;

	private float initialPos;

	private float direction;

	private float previusPos;

	private float timeStuck;

	private float companionDirection;

	public WallFallState(CharacterStateController controller, StateMachine machine)
		: base(controller, machine)
	{
		_state = CharacterStates.WALLFALL;
	}

	public override void Enter()
	{
		base.Enter();
		timedragging = 0f;
		controller.animator.SetBool("Swing", value: false);
		initialPos = controller.companion.transform.position.y;
		direction = Mathf.Sign(controller.transform.position.x - controller.companion.transform.position.x);
		timeStuck = 0f;
		companionDirection = controller.transform.position.x - controller.companion.transform.position.x;
	}

	public override void HandleInput(Inputs input)
	{
		base.HandleInput(input);
	}

	public override void LogicUpdate()
	{
		base.LogicUpdate();
		RotateCharacter();
		if (Mathf.Sign(companionDirection) != Mathf.Sign(controller.transform.position.x - controller.companion.transform.position.x) && timeStuck < 1f)
		{
			controller.SetState(CharacterStates.HANG);
		}
		if (!controller.GetCompanionGrounded() && controller.companion.transform.position.y < initialPos - 0.1f)
		{
			controller.SetState(CharacterStates.FALL);
		}
		if (controller.GetGrounded())
		{
			controller.SetState(CharacterStates.GROUNDED);
			controller.SetAnimation("Idle");
		}
		if (controller.transform.position.y > controller.companion.transform.position.y)
		{
			controller.SetState(CharacterStates.FALL);
		}
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
		if (controller.CompanionStateController.movement == 0f)
		{
			timedragging += Time.fixedDeltaTime;
			pullForce = 0f;
		}
		else if (Mathf.Sign(direction) != Mathf.Sign(controller.CompanionStateController.movement))
		{
			if (previusPos == controller.companion.transform.position.x)
			{
				timeStuck += Time.fixedDeltaTime;
				pullForce = 0f;
			}
			else if (timeStuck < maxTimeStuck && controller.GetCompanionState() != CharacterStates.JUMP)
			{
				previusPos = controller.companion.transform.position.x;
				if (timedragging < 0.8f)
				{
					pullForce = Mathf.Abs(controller.transform.position.x - controller.companion.transform.position.x) * 1.2f;
					timedragging -= Time.fixedDeltaTime / 2f;
				}
				else
				{
					pullForce = 0f;
				}
			}
			if (timeStuck >= maxTimeStuck)
			{
				timedragging += Time.fixedDeltaTime;
				pullForce = 0f;
			}
		}
		else
		{
			timedragging += Time.fixedDeltaTime;
			pullForce = 0f;
		}
		Vector3 vector = new Vector2(0f, 10f) * pullForce;
		controller.rb.AddForce(vector);
		dragForce = dragForceCurve.Evaluate(timedragging);
		if (!controller.CompanionStateController.IsPullingRope)
		{
			controller.companion.transform.position += new Vector3(direction, 0f, 0f) * Time.fixedDeltaTime * dragForce;
		}
	}

	public override void Exit()
	{
		base.Exit();
		controller.spriteRenderer.gameObject.transform.rotation = Quaternion.identity;
	}

	public void RotateCharacter()
	{
		Vector3 zero = Vector3.zero;
		zero.z = 57.29578f * Mathf.Atan2(controller.companion.transform.position.y - controller.transform.position.y, controller.companion.transform.position.x - controller.transform.position.x);
		zero.z -= 90f;
		controller.spriteRenderer.gameObject.transform.rotation = Quaternion.Euler(zero);
	}
}
