using UnityEngine;

public class PenguinAction : ScriptableObject
{
	public PenguinActionsController Controller;

	protected Transform _controllerTransform;

	[SerializeField]
	private PenguinActions _type;

	[SerializeField]
	protected bool _fall;

	[SerializeField]
	protected bool _grounded;

	[SerializeField]
	private bool _movement;

	[SerializeField]
	private bool _flip;

	[SerializeField]
	private bool _maxDistance;

	public PenguinActions Type => _type;

	public virtual void Initialize()
	{
		_controllerTransform = Controller.transform;
	}

	public virtual void Enter()
	{
	}

	public virtual void LogicUpdate()
	{
		if (_grounded)
		{
			Grounded();
		}
		if (_fall)
		{
			Fall();
		}
		if (_maxDistance)
		{
			MaxDistanceDetector();
		}
		if (_flip)
		{
			if (Controller.Movement < 0f)
			{
				Flip(value: true);
			}
			else if (Controller.Movement > 0f)
			{
				Flip(value: false);
			}
		}
	}

	public virtual void PhysicsUpdate()
	{
		if (_movement)
		{
			Move();
		}
	}

	public virtual void Exit()
	{
	}

	public virtual void Clean()
	{
	}

	private void MaxDistanceDetector()
	{
		if (Controller.CurrentPenguinDistance > Controller.MaxPenguinDistance + 0.1f && _controllerTransform.position.y < Controller.CompanionTransform.position.y)
		{
			Controller.SendPlayerMakerEvent("Fall");
		}
	}

	public virtual void Grounded()
	{
		if (Controller.IsGrounded)
		{
			Controller.SendPlayerMakerEvent("Grounded");
		}
	}

	public void Move()
	{
		if (!Physics2D.Raycast(_controllerTransform.position, new Vector3(Controller.Movement, 0f, 0f), 0.4f, Controller.GroundLayer))
		{
			Vector3 vector = new Vector3(Controller.Movement, 0f, 0f) * Time.fixedDeltaTime * Controller.Speed;
			_controllerTransform.position += vector;
		}
	}

	public void Flip(bool value)
	{
		if (value)
		{
			_controllerTransform.localScale = new Vector3(-1f, 1f, 1f);
		}
		else
		{
			_controllerTransform.localScale = new Vector3(1f, 1f, 1f);
		}
	}

	public virtual void Fall()
	{
		if (!Controller.IsGrounded)
		{
			Controller.SendPlayerMakerEvent("Fall");
		}
	}

	public void RotateCharacter()
	{
		Vector3 zero = Vector3.zero;
		zero.z = 57.29578f * Mathf.Atan2(Controller.CompanionTransform.position.y - Controller.transform.position.y, Controller.CompanionTransform.position.x - Controller.transform.position.x);
		zero.z -= 90f;
		Controller.Sprite.transform.rotation = Quaternion.Euler(zero);
	}
}
