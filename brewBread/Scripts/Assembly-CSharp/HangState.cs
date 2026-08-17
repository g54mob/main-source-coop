using System;
using UnityEngine;

[Serializable]
public class HangState : State
{
	[Header("Hang")]
	[Tooltip("Force Applied while hanging")]
	[SerializeField]
	private float swingForce;

	[Tooltip("Force added evey time you balance")]
	[SerializeField]
	private float additiveForce;

	[Tooltip("Time before force reset to default")]
	[SerializeField]
	private float swingTime;

	[Tooltip("Time you can stay on top of the loop, without falling")]
	[SerializeField]
	private float restTime;

	[Tooltip("Time the penguin should be in the situation, before applying any force")]
	[SerializeField]
	private float timeBeforeFalling;

	[SerializeField]
	private ParticleSystem _smokeParticles;

	private Rigidbody2D rb;

	private float swingForceAux;

	private float restTimeAux;

	private float wallFallTime;

	public HangState(CharacterStateController controller, StateMachine machine)
		: base(controller, machine)
	{
		_state = CharacterStates.HANG;
	}

	public override void Enter()
	{
		base.Enter();
		EventManager.instance.OnPlayerHang(controller.transform);
		controller.SetAnimation("Hang");
		rb = controller.rb;
		swingForceAux = swingForce;
		restTimeAux = restTime;
		wallFallTime = 0f;
		controller.animator.SetBool("Swing", value: false);
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
		case Inputs.WEIGHT:
			if (controller.transform.position.y > controller.companion.transform.position.y)
			{
				controller.SetState(CharacterStates.FALL);
			}
			break;
		}
	}

	public override void LogicUpdate()
	{
		base.LogicUpdate();
		RotateCharacter();
		if (controller.GetGrounded())
		{
			controller.SetState(CharacterStates.GROUNDED);
			controller.SetAnimation("Idle");
		}
		if (!controller.GetCompanionGrounded() && controller.GetCompanionState() != CharacterStates.GRAB)
		{
			controller.SetState(CharacterStates.FALL);
		}
		if (controller.companion.transform.position.y > controller.transform.position.y)
		{
			if ((bool)Physics2D.Raycast(controller.transform.position, Vector2.left, 0.6f, controller.groundLayer) || (bool)Physics2D.Raycast(controller.transform.position, Vector2.right, 0.6f, controller.groundLayer))
			{
				wallFallTime += Time.deltaTime;
			}
			else if (wallFallTime > 0f)
			{
				wallFallTime -= Time.deltaTime;
			}
			if (wallFallTime > timeBeforeFalling && controller.GetCompanionGrounded() && controller.GetCompanionState() != CharacterStates.WEIGHT)
			{
				controller.SetState(CharacterStates.WALLFALL);
			}
		}
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
		if (controller.movement == 0f)
		{
			rb.AddForce(-rb.velocity / 4f);
		}
		Vector2 vector = (controller.companion.transform.position - controller.transform.position).normalized;
		Vector2 vector2 = Vector2.zero;
		if (controller.movement > 0f)
		{
			vector2 = new Vector2(vector.y, 0f - vector.x);
		}
		if (controller.movement < 0f)
		{
			vector2 = new Vector2(0f - vector.y, vector.x);
		}
		Vector2 force = vector2.normalized * swingForceAux;
		if (Mathf.Sign(force.x) == Mathf.Sign(rb.velocity.x))
		{
			if (controller.movement != 0f)
			{
				controller.animator.SetBool("Swing", value: true);
				swingForceAux += additiveForce;
				_smokeParticles.Play();
			}
			else
			{
				_smokeParticles.Stop();
			}
			rb.AddForce(force, ForceMode2D.Force);
		}
		else if (swingTime <= 0f)
		{
			swingForceAux = swingForce;
			swingTime = 0.5f;
		}
		else
		{
			swingTime -= Time.fixedDeltaTime;
		}
		float num = 0f;
		if (controller.movement != 0f)
		{
			num = 23f;
			restTimeAux = restTime;
		}
		else if (restTimeAux <= 0f)
		{
			restTimeAux = restTime;
			controller.animator.SetBool("Swing", value: false);
			if (controller.companion.transform.position.y < controller.transform.position.y)
			{
				controller.SetState(CharacterStates.FALL);
			}
		}
		else
		{
			num = 23f;
			restTimeAux -= Time.fixedDeltaTime;
		}
		Vector2 vector3 = (controller.transform.position - controller.companion.transform.position).normalized;
		rb.AddForce(vector3 * num, ForceMode2D.Force);
	}

	public override void Exit()
	{
		base.Exit();
		controller.spriteRenderer.gameObject.transform.rotation = Quaternion.identity;
		EventManager.instance.OnPlayerStopHangging(controller.transform);
	}

	public void RotateCharacter()
	{
		Vector3 zero = Vector3.zero;
		zero.z = 57.29578f * Mathf.Atan2(controller.companion.transform.position.y - controller.transform.position.y, controller.companion.transform.position.x - controller.transform.position.x);
		zero.z -= 90f;
		controller.spriteRenderer.gameObject.transform.rotation = Quaternion.Euler(zero);
	}
}
