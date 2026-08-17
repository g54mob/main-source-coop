using System;
using UnityEngine;

[Serializable]
public class GrabState : State
{
	[Header("Wall Grab")]
	[Tooltip("GameObject that checks if a wall is being hit")]
	public Transform t_wallCheck_Front;

	[Tooltip("GameObject that checks if a wall is being hit")]
	public Transform t_wallCheck_Back;

	[Tooltip("Time you are able to grab to the Walls")]
	public float grabTimer;

	[Tooltip("How much fast is the timer drained when the companion is hanging: grabTimer*timeMultiplicator")]
	public float timeMultiplicator;

	[Tooltip("Max distance the player can be from a wall, before it's detected as grabable")]
	public float radiusCheck;

	[Tooltip("Unity's layer for chain")]
	public LayerMask chainLayer;

	[SerializeField]
	private ParticleSystem sweatParticles;

	[SerializeField]
	[Tooltip("Distance the other penguin must be before releasing the grab")]
	private float _releaseThreshhold = 2.7f;

	private float previusState = 1f;

	private Vector3 newPos = Vector3.zero;

	private int _direction;

	public GrabState(CharacterStateController controller, StateMachine machine)
		: base(controller, machine)
	{
		_state = CharacterStates.GRAB;
	}

	public override void Enter()
	{
		base.Enter();
		previusState = 1f;
		RaycastHit2D raycastHit2D = Physics2D.Raycast(controller.transform.position, Vector2.left, 1f, (int)controller.groundLayer | (int)chainLayer);
		float num = 0f;
		if ((bool)raycastHit2D)
		{
			if ((float)raycastHit2D.collider.gameObject.layer == Mathf.Log((int)controller.groundLayer, 2f))
			{
				num = 0.4f;
			}
			if (controller.transform.localScale.x == -1f)
			{
				previusState = -1f;
			}
			else if (controller.spriteRenderer.transform.localRotation.eulerAngles.z >= 0f && controller.spriteRenderer.transform.localRotation.eulerAngles.z <= 90f)
			{
				previusState = -1f;
			}
			_direction = -1;
			newPos = new Vector2(raycastHit2D.point.x + num, controller.transform.position.y);
		}
		else
		{
			raycastHit2D = Physics2D.Raycast(controller.transform.position, Vector2.right, 1f, (int)controller.groundLayer | (int)chainLayer);
			if (!raycastHit2D)
			{
				controller.SetState(CharacterStates.FALL);
				return;
			}
			if ((float)raycastHit2D.collider.gameObject.layer == Mathf.Log((int)controller.groundLayer, 2f))
			{
				num = 0.4f;
			}
			if (controller.transform.localScale.x == 1f)
			{
				previusState = 1f;
			}
			else if (controller.spriteRenderer.transform.localRotation.eulerAngles.z <= 0f && controller.spriteRenderer.transform.localRotation.eulerAngles.z >= -90f)
			{
				previusState = 1f;
			}
			newPos = new Vector2(raycastHit2D.point.x - num, controller.transform.position.y);
			_direction = 1;
		}
		controller.transform.localScale = new Vector3(previusState, 1f, 1f);
		controller.SetPlayerImmobile();
		controller.transform.position = newPos;
		float num2 = controller.timerAux / grabTimer;
		controller.SetAnimation("Grab", num2);
		sweatParticles.Play();
		ParticleSystem.EmissionModule emission = sweatParticles.emission;
		emission.rateOverTime = 2f + num2 * 6f;
	}

	public override void HandleInput(Inputs input)
	{
		if (input == Inputs.GRAB)
		{
			controller.SetState(CharacterStates.FALL);
		}
	}

	public override void LogicUpdate()
	{
		base.LogicUpdate();
		controller.timerAux += Time.deltaTime;
		if (controller.GetDistanceToCompanion() > _releaseThreshhold)
		{
			controller.SetState(CharacterStates.FALL);
		}
		RaycastHit2D raycastHit2D = default(RaycastHit2D);
		switch (_direction)
		{
		case 1:
			raycastHit2D = Physics2D.Raycast(controller.transform.position, Vector2.right, 1f, (int)controller.groundLayer | (int)chainLayer);
			break;
		case -1:
			raycastHit2D = Physics2D.Raycast(controller.transform.position, Vector2.left, 1f, (int)controller.groundLayer | (int)chainLayer);
			break;
		}
		if (!raycastHit2D)
		{
			controller.SetState(CharacterStates.FALL);
		}
		if (controller.timerAux > grabTimer)
		{
			controller.SetState(CharacterStates.FALL);
		}
		StaminaController();
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
	}

	public override void Exit()
	{
		base.Exit();
		controller.SetPlayerMobile();
		ParticleSystem.EmissionModule emission = sweatParticles.emission;
		emission.rateOverTime = 2f;
		sweatParticles.Stop();
	}

	private void StaminaController()
	{
		float num = controller.timerAux / grabTimer;
		if (num <= 1f)
		{
			controller.spriteRenderer.material.SetFloat("Vector1_ed0278251b2f42e8b7484ceb6b45bd6c", num);
		}
		ParticleSystem.EmissionModule emission = sweatParticles.emission;
		emission.rateOverTime = 2f + num * 6f;
	}
}
